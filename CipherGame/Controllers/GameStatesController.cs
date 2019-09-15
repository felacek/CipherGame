using CipherGameData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CipherGame.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class GameStatesController : ControllerBase
    {
        private readonly CipherGameContext _context;

        public GameStatesController(CipherGameContext context)
        {
            _context = context;
        }

        // GET: api/GameStates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetGameStates()
        {
            var states = await _context.GameStates
                .Select(x => new
                {
                    TeamCode = x.TeamCode,
                    CipherCode = x.Cipher.Code,
                    IsPlaceFound = x.IsPlaceFound,
                    IsAnswerFound = x.IsAnswerFound,
                })
                .ToListAsync();
            return states.OrderBy( x => x.TeamCode). ThenBy(x => x.CipherCode)
            .ToList();
        }

        // POST: api/GameStates
        [HttpPost]
        public async Task<ActionResult<IEnumerable<object>>> CreateGameStates()
        {
            var exists = await _context.GameStates.ToListAsync();
            if (exists.Any())
            {
                throw new Exception("State is not empty!");
            }

            var teams = await _context.Teams.ToListAsync();
            var ciphers = await _context.Ciphers.ToListAsync();

            int counter = 0;
            foreach (var team in teams)
            {
                int order = 0;
                foreach (var cipher in ciphers)
                {

                    _context.GameStates.Add(new GameState
                    {
                        Id = ++counter,
                        Order = ++order,
                        Team = team,
                        Cipher = cipher,
                        IsPlaceFound = false,
                        IsAnswerFound = false
                    });
                }
            }

            await _context.SaveChangesAsync();

            return await _context.GameStates
                .Select(x => new
                {
                    TeamCode = x.TeamCode,
                    CipherCode = x.Cipher.Code,
                    IsPlaceFound = x.IsPlaceFound,
                    IsAnswerFound = x.IsAnswerFound,
                })
                .ToListAsync();
        }

        // DELETE: api/GameStates
        [HttpDelete]
        public async Task<ActionResult<IEnumerable<GameState>>> DeleteAllGameState()
        {
            var list = await _context.GameStates.ToListAsync();
            foreach (var item in list)
            {
                _context.GameStates.Remove(item);
            }
            await _context.SaveChangesAsync();

            return await _context.GameStates.ToListAsync();
        }
    }
}
