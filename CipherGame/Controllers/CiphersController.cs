using CipherGameData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CipherGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiphersController : ControllerBase
    {
        private readonly CipherGameContext _context;

        public CiphersController(CipherGameContext context)
        {
            _context = context;
        }

        // GET: api/Ciphers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cipher>>> GetCiphers()
        {
            return await _context.Ciphers.ToListAsync();
        }

        // GET: api/Ciphers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cipher>> GetCipher(string id)
        {
            var cipher = await _context.Ciphers.FindAsync(id);

            if (cipher == null)
            {
                return NotFound();
            }

            return cipher;
        }

        // PUT: api/Ciphers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCipher(string id, Cipher cipher)
        {
            if (id != cipher.Code)
            {
                return BadRequest();
            }

            _context.Entry(cipher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CipherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ciphers
        [HttpPost]
        public async Task<ActionResult<Cipher>> PostCipher(Cipher cipher)
        {
            _context.Ciphers.Add(cipher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCipher", new { id = cipher.Code }, cipher);
        }

        // DELETE: api/Ciphers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cipher>> DeleteCipher(string id)
        {
            var cipher = await _context.Ciphers.FindAsync(id);
            if (cipher == null)
            {
                return NotFound();
            }

            _context.Ciphers.Remove(cipher);
            await _context.SaveChangesAsync();

            return cipher;
        }

        private bool CipherExists(string id)
        {
            return _context.Ciphers.Any(e => e.Code == id);
        }
    }
}
