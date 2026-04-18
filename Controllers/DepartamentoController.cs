using CorporateIdentityManager.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly ActiveDirectoryDbContext _context;
        public DepartamentoController(ActiveDirectoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var departamentos = await _context.Departamentos.ToListAsync();
            return Ok(departamentos);
        }
    }
}