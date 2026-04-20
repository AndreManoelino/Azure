using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoleriteController : ControllerBase
    {
        private readonly ActiveDirectoryDbContext _context;

        public HoleriteController(ActiveDirectoryDbContext context)
        {
            _context = context;
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> ListarMeusHolerites(Guid usuarioId)
        {

            var holerites = await _context.Holerites
                .Where(h => h.UsuarioId == usuarioId)
                .OrderByDescending(h => h.AnoReferencia)
                .ThenByDescending(h => h.MesReferencia)
                .ToListAsync();

            return Ok(holerites);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarHolerite([FromBody] CadastrarHoleriteRequest req)
        {

            var holerite = new Holerite(req.MesReferencia, req.AnoReferencia, req.ValorLiquido, req.Descricao, req.UsuarioId);
            _context.Holerites.Add(holerite);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Holerite gerado com sucesso na base de dados!" });
        }
    }

    public class CadastrarHoleriteRequest
    {
        public string MesReferencia { get; set; } = string.Empty;
        public string AnoReferencia { get; set; } = string.Empty;
        public decimal ValorLiquido { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public Guid UsuarioId { get; set; }
    }
}
