using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CipherGameData;

namespace CipherGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly CipherGameContext _context;

        public AuditLogsController(CipherGameContext context)
        {
            _context = context;
        }

        // GET: api/AuditLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetAuditLogs()
        {
            var logs = await _context.AuditLogs.ToListAsync();
            return logs.OrderByDescending(x => x.Created).ToList();
        }

        
        // DELETE: api/AuditLogs/5
        [HttpDelete]
        public async Task<ActionResult<IEnumerable<AuditLog>>> DeleteAuditLogs()
        {
            var logs = await _context.AuditLogs.ToArrayAsync();
            
            foreach(var log in logs)
            {
                _context.AuditLogs.Remove(log);
            }

            
            await _context.SaveChangesAsync();

            return await _context.AuditLogs.ToListAsync();
        }
    }
}
