using InformatikaiEszkozok_LIB.DATA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InformatikaiEszkozok.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VasarloController : ControllerBase
    {
        private readonly InfotermekDbContext context;

        public VasarloController(InfotermekDbContext context)
        {
            this.context = context;
        }

        // GET api/Vasarlo/Kereses?email=...&telefon=...
        [HttpGet("Kereses")]
        public async Task<IActionResult> Kereses(string email, string telefon)
        {
            var vasarlo = await context.Vasarlo
                .FirstOrDefaultAsync(v =>
                    v.Email == email &&
                    v.Telefon == telefon);

            if (vasarlo == null)
            {
                return NotFound();
            }

            return Ok(vasarlo);
        }
    }
}