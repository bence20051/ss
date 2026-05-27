using InformatikaiEszkozok_LIB.DATA;
using InformatikaiEszkozok_LIB.DTO;
using InformatikaiEszkozok_LIB.MODEL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InformatikaiEszkozok.API.Controllers
{
    /// <summary>
    /// A rendelésekkel kapcsolatos HTTP végpontokat kezeli.
    /// Alap URL: api/Rendeles/...
    /// </summary>
    [Route ("api/[controller]")]
    [ApiController]
    public class RendelesController : ControllerBase
    {
        private readonly InfotermekDbContext context;

        private const int DIAK_KEDVEZMENY = 20;       // dik z %  

        public RendelesController(InfotermekDbContext context)
        {
            this.context = context;
        }

        [HttpGet("RendelesLista")]
        public async Task<IActionResult> RendelesLista()
        {
            var rendelesek = await (
                from r in context.Rendeles
                join v in context.Vasarlo on r.VasarloId equals v.Id
                select new RendelesOsszesitesDto
                {
                    Id = r.Id,
                    KedvezmenyAzonosito = v.Azonosito,
                    Nev = v.Nev,
                    Telefon = v.Telefon,
                    Email = v.Email,

                    Kedvezmeny = string.IsNullOrEmpty(v.Azonosito) ? 0 : DIAK_KEDVEZMENY,

                    RendelesErtek = (
                        from rt in context.RendelesTetel
                        join t in context.Termek on rt.TermekId equals t.Id
                        where rt.RendelesId == r.Id
                        select rt.Mennyiseg * t.Egysegar
                    ).Sum()
                }
            ).ToListAsync();

            return Ok(rendelesek);
        }

        [HttpGet("RendelesTetelek/{rendelesId}")]
        public async Task<IActionResult> RendelesTetelek(int rendelesId)
        {
            
            var rendeles = await context.Rendeles
                .Where(r => r.Id == rendelesId)
                .FirstOrDefaultAsync();

            if (rendeles == null)
            {
                return NotFound();
            }

            var vasarlo = await context.Vasarlo.FindAsync(rendeles.VasarloId);
            int kedvezmenySzazalek =
                (vasarlo != null && !string.IsNullOrEmpty(vasarlo.Azonosito))
                    ? DIAK_KEDVEZMENY
                    : 0;

           var tetelek = await (
                from rt in context.RendelesTetel
                join t in context.Termek on rt.TermekId equals t.Id
                where rt.RendelesId == rendelesId
                select new RendelesTetelDto
                {
                    Id = rt.Id,
                    RendelesId = rt.RendelesId,
                    Termeknev = t.Nev,
                    Mennyiseg = rt.Mennyiseg,
                    Egysegar = t.Egysegar,
                    Kedvezmeny = kedvezmenySzazalek,
                    Ar = rt.Mennyiseg * t.Egysegar * (100 - kedvezmenySzazalek) / 100
                }
            ).ToListAsync();

            return Ok(tetelek);
        }

       
        [HttpPost("RendelesRogzites")]
        public async Task<IActionResult> RendelesRogzites([FromBody] RendelesRogzitesAdat adat)
        {
            if (adat?.Vasarlo == null)
            {
                return BadRequest("Hiányzó vásárló adat.");
            }

            if (adat.Vasarlo.Id == 0)
            {
                context.Vasarlo.Add(adat.Vasarlo);
                await context.SaveChangesAsync();
            }

            var rendeles = new Rendeles
            {
                VasarloId = adat.Vasarlo.Id,
                Datum = DateOnly.FromDateTime(DateTime.Now)
            };

            context.Rendeles.Add(rendeles);
            await context.SaveChangesAsync();   

            
            foreach (var tetel in adat.Tetelek)
            {
                tetel.Id = 0;                      
                tetel.RendelesId = rendeles.Id;
            }

            context.RendelesTetel.AddRange(adat.Tetelek);
            await context.SaveChangesAsync();

            return Ok(adat.Vasarlo);
        }
    }
}
