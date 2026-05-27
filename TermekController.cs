using InformatikaiEszkozok_LIB.DATA;
using InformatikaiEszkozok_LIB.DTO;
using InformatikaiEszkozok_LIB.MODEL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InformatikaiEszkozok.API.Controllers
{
    /// <summary>
    /// A termékekkel kapcsolatos HTTP végpontokat kezeli.
    /// Az alap URL: api/Termek/...
    /// </summary>
    [Route("api/[controller]")]   // a "[controller]" a Termek-re cserélődik
    [ApiController]
    public class TermekController : ControllerBase
    {
        private readonly InfotermekDbContext context;

        
        public TermekController(InfotermekDbContext context)
        {
            this.context = context;
        }

        [HttpGet("TermekLista")]
        public async Task<IActionResult> TermekLista()
        {
            var lista = await context.Termek
                .Select(t => new TermekListaDto
                {
                    Id = t.Id,
                    Nev = t.Nev,
                    Keszlet = t.Keszlet
                })
                .ToListAsync();

            return Ok(lista);
        }

        
        [HttpGet("KeszletLista")]
        public async Task<IActionResult> KeszletLista()
        {
            var lista = await context.Termek
                .Select(t => new KeszletListaDto
                {
                    Id = t.Id,
                    Nev = t.Nev,
                    Ertek = t.Keszlet * t.Egysegar
                })
                .ToListAsync();

            return Ok(lista);
        }

        
        [HttpGet("Termekek")]
        public async Task<IActionResult> Termekek()
        {
            var termekek = await context.Termek
                .Where(t => t.Elerheto)
                .ToListAsync();

            return Ok(termekek);
        }

      
       [HttpPost("TermekFelvitel")]
        public async Task<IActionResult> TermekFelvitel([FromBody] Termek termek)
        {
            context.Termek.Add(termek);
            await context.SaveChangesAsync();

            return Ok();
        } 

        
        [HttpPut("TermekModositas")]
        public async Task<IActionResult> TermekModositas([FromBody] Termek termek)
        {
            var regi = await context.Termek.FindAsync(termek.Id);

            if (regi == null)
            {
                return NotFound();
            }

            regi.Nev = termek.Nev;
            regi.Leiras = termek.Leiras;
            regi.Egysegar = termek.Egysegar;
            regi.Keszlet = termek.Keszlet;
            regi.Elerheto = termek.Elerheto;
            regi.KepFajlnev = termek.KepFajlnev;

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("TermekTorles/{id}")]
        public async Task<IActionResult> TermekTorles(int id)
        {
            var termek = await context.Termek.FindAsync(id);

            if (termek == null)
            {
                return NotFound();
            }

            context.Termek.Remove(termek);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
