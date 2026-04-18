using CorporateIdentityManager.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizacaoController : ControllerBase
    {
        private readonly ActiveDirectoryDbContext _context;

        public OrganizacaoController(ActiveDirectoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var organizacoes = await _context.Organizacoes.ToListAsync();
            return Ok(organizacoes);
        }
    }
}