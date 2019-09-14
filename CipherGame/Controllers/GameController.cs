using CipherGame.Models;
using CipherGameData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CipherGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly CipherGameContext _context;
        public GameController(CipherGameContext context)
        {
            _context = context;
        }

        // POST: api/Game
        [HttpPost]
        public async Task<ActionResult<TeamStateModel>> GetTeamState([FromForm]string teamCode)
        {
            await LogActivity(teamCode, $"GetTeamState(teamCode:{teamCode})");

            var (state, errorModel) = GetFirstUnresolvedState(teamCode);
            
            return errorModel;
        }

        [HttpPost("SetPlaceCode")]
        public async Task<ActionResult<TeamStateModel>> SetPlaceCode([FromForm]string teamCode, [FromForm]string placeCode)
        {
            await LogActivity(teamCode, $"SetPlaceCode(teamCode:{teamCode}, placeCode: {placeCode})");

            var (state, errorModel) = GetFirstUnresolvedState(teamCode);
            if(state == null)
            {
                return errorModel;
            }

            if(state.IsPlaceFound)
            {
                // don't compare place core, return 
                return errorModel;
            }

            if(string.IsNullOrWhiteSpace(placeCode))
            {
                errorModel.Message = "Není vyplněný kód stanoviště.";
                return errorModel;
            }

            if(state.Cipher.Place.ToUpper() == placeCode.ToUpper())
            {
                state.IsPlaceFound = true;
                await _context.SaveChangesAsync();
                errorModel.IsPlaceFound = true;
                errorModel.Message = "Kód stanoviště je správný!";
                return errorModel;
            }

            errorModel.Message = "Kód stanoviště není správný";
            return errorModel;
        }

        [HttpPost("SetCipherResult")]
        public async Task<ActionResult<TeamStateModel>> SetCipherResult([FromForm]string teamCode, [FromForm]string result)
        {
            await LogActivity(teamCode, $"SetCipherResult(teamCode:{teamCode}, result: {result})");

            var (state, errorModel) = GetFirstUnresolvedState(teamCode);
            if (state == null)
            {
                return errorModel;
            }

            if (!state.IsPlaceFound)
            {
                errorModel.Message = "Kód stanoviště není správný";
                return errorModel;
            }

            if (state.Cipher.Answer.ToUpper() == result.ToUpper())
            {
                state.IsAnswerFound = true;
                await _context.SaveChangesAsync();
                var (newState, newModel) = GetFirstUnresolvedState(teamCode);
                return newModel;
            }

            errorModel.Message = "Kód stanoviště není správný";
            return errorModel;
        }

        private (GameState, TeamStateModel) GetFirstUnresolvedState(string teamCode)
        {
            var team = _context.Teams.FirstOrDefault(e => e.Code == teamCode);
            if (team == null)
            {
                return (null, new TeamStateModel
                {
                    Message = $"Spatně zadaný kód tymu. Tým s kódem „{teamCode}“ není evidován v systému.",
                    CipherCode = String.Empty,
                    IsPlaceFound = false,
                    TeamName = String.Empty
                });
            }

            var allTeamStates = _context.GameStates.Where(x => x.TeamCode == teamCode).ToList();
            if (!allTeamStates.Any())
            {
                return (null, new TeamStateModel
                {
                    Message = $"Tým „{teamCode}“ nemá přidělenou žádnou Šifru.",
                    CipherCode = String.Empty,
                    IsPlaceFound = false,
                    TeamName = team.Name
                });
            }

            var unresolvedState = allTeamStates
                .Where(x => !x.IsAnswerFound || !x.IsPlaceFound)
                .OrderBy(x => x.Order)
                .FirstOrDefault();

            if (unresolvedState == null)
            {
                return (null, new TeamStateModel
                {
                    Message = $"Tým „{teamCode}“ má vyřešeny všechny šifry.",
                    CipherCode = String.Empty,
                    IsPlaceFound = false,
                    TeamName = team.Name
                });
            }

            return (unresolvedState, new TeamStateModel
            {
                Message = unresolvedState.IsPlaceFound ? "Kód stanoviště správný!" : unresolvedState.Cipher.Place,
                CipherCode = unresolvedState.Cipher.Code,
                IsPlaceFound = unresolvedState.IsPlaceFound,
                TeamName = team.Name
            });
        }

        private async Task LogActivity(string teamCode, string message)
        {

            await _context.AuditLogs.AddAsync(new AuditLog { Created = DateTime.Now, Log = message });
            await _context.SaveChangesAsync();
        }
    }
}