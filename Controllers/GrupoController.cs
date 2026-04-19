using CorporateIdentityManager.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrupoController : ControllerBase
    {
        private readonly ActiveDirectoryDbContext _context;
        public GrupoController(ActiveDirectoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var grupos = await _context.Grupos.ToListAsync();
            return Ok(grupos);
        }

    }
}