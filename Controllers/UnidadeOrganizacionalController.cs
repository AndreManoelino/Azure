using CorporateIdentityManager.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadeOrganizacionalController : ControllerBase
    {
        private readonly ActiveDirectoryDbContext _context;

        public UnidadeOrganizacionalController(ActiveDirectoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var unidades = await _context.UnidadesOrganizacionais.ToListAsync();
            return Ok(unidades);
        }
    }
}