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


            try
            {
                var (state, errorModel) = GetFirstUnresolvedState(teamCode);
                await LogActivity(teamCode, $"GetTeamState returns: {errorModel}");

                // we dont send cipherCode on this place
                errorModel.CipherCode = string.Empty;
                return errorModel;
            }
            catch(Exception exc)
            {
                await LogActivity(teamCode, $"Exception: {exc}");

                return new TeamStateModel
                {
                    TeamName = teamCode,
                    Message = exc.Message,
                    CipherCode = string.Empty,
                    IsPlaceFound = false
                };
            }

            
        }

        [HttpPost("SetPlaceCode")]
        public async Task<ActionResult<TeamStateModel>> SetPlaceCode([FromForm]string teamCode, [FromForm]string placeCode)
        {
            await LogActivity(teamCode, $"SetPlaceCode(teamCode:{teamCode}, placeCode: {placeCode})");
            try
            {
                var (state, errorModel) = GetFirstUnresolvedState(teamCode);
                if (state == null)
                {
                    await LogActivity(teamCode, $"SetPlaceCode returns: {errorModel}");
                    return errorModel;
                }

                if (state.IsPlaceFound)
                {
                    await LogActivity(teamCode, $"SetPlaceCode returns: {errorModel}");
                    return errorModel;
                }

                if (string.IsNullOrWhiteSpace(placeCode))
                {
                    errorModel.Message = "Není vyplněný kód stanoviště";

                    await LogActivity(teamCode, $"SetPlaceCode returns: {errorModel}");

                    // we dont send cipherCode on this place
                    errorModel.CipherCode = string.Empty;
                    return errorModel;
                }

                if (state.Cipher.Code.ToUpper() == placeCode.ToUpper())
                {
                    state.IsPlaceFound = true;
                    await _context.SaveChangesAsync();
                    errorModel.IsPlaceFound = true;
                    errorModel.Message = "Kód stanoviště je správný!";

                    await LogActivity(teamCode, $"SetPlaceCode returns: {errorModel}");

                    return errorModel;
                }

                errorModel.Message = "Kód stanoviště není správný";

                await LogActivity(teamCode, $"SetPlaceCode returns: {errorModel}");

                // we dont send cipherCode on this place
                errorModel.CipherCode = string.Empty;
                return errorModel;

            }catch(Exception exc)
            {
                await LogActivity(teamCode, $"Exception: {exc}");

                return new TeamStateModel
                {
                    TeamName = teamCode,
                    Message = exc.Message,
                    CipherCode = string.Empty,
                    IsPlaceFound = false
                };
            }
        }

            [HttpPost("SetCipherResult")]
            public async Task<ActionResult<TeamStateModel>> SetCipherResult([FromForm]string teamCode, [FromForm]string result)
        {
            await LogActivity(teamCode, $"SetCipherResult(teamCode:{teamCode}, result: {result})");

            try
            {
                var (state, errorModel) = GetFirstUnresolvedState(teamCode);
                if (state == null)
                {
                    await LogActivity(teamCode, $"SetCipherResult returns: {errorModel}");
                    return errorModel;
                }

                if (!state.IsPlaceFound)
                {
                    errorModel.Message = "Kód stanoviště ještě není vyplněn";

                    await LogActivity(teamCode, $"SetCipherResult returns: {errorModel}");
                    return errorModel;
                }

                if (state.Cipher.Answer.ToUpper() == result.ToUpper())
                {
                    state.IsAnswerFound = true;
                    await _context.SaveChangesAsync();
                    var (newState, newModel) = GetFirstUnresolvedState(teamCode);

                    await LogActivity(teamCode, $"SetCipherResult returns: {errorModel}");
                    return newModel;
                }

                errorModel.Message = "Špatná odpověď";

                await LogActivity(teamCode, $"SetCipherResult returns: {errorModel}");
                return errorModel;

            }catch( Exception exc)
            {
                await LogActivity(teamCode, $"Exception: {exc}");

                return new TeamStateModel
                {
                    TeamName = teamCode,
                    Message = exc.Message,
                    CipherCode = string.Empty,
                    IsPlaceFound = false
                };
            }
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
                    Message = $"Tým „{team.Name}“ nemá přidělenou žádnou šifru",
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
                    Message = $"Tým „{team.Name}“ má vyřešeny všechny šifry.",
                    CipherCode = String.Empty,
                    IsPlaceFound = false,
                    TeamName = team.Name
                });
            }

            _context.Entry(unresolvedState).Reference(x => x.Cipher).Load();
            _context.Entry(unresolvedState).Reference(x => x.Team).Load();

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