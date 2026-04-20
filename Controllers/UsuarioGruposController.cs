using CorporateIdentityManager.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioGruposController : ControllerBase
    {
        private readonly ActiveDirectoryDbContext _context;

        public UsuarioGruposController(ActiveDirectoryDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var usuarioGrupos = await _context.UsuarioGrupos.ToListAsync();
            return Ok(usuarioGrupos.Select(ug => new
            {
                ug.Id,
                ug.UsuarioId,
                ug.GrupoId,
                ug.DataAssociacao
            }));
        }

    }
}