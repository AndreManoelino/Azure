using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly ActiveDirectoryDbContext _context;

        public InventarioController(ActiveDirectoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarEquipamentos()
        {
            // O TI tá de olho no patrimônio da empresa
            var equipamentos = await _context.Equipamentos
                .Include(e => e.Usuario)
                .ToListAsync();
                
            return Ok(equipamentos.Select(e => new {
                e.Id,
                e.Nome,
                e.Marca,
                e.ServiceTag,
                e.Tipo,
                e.Status,
                UsuarioAlocado = e.Usuario != null ? e.Usuario.Nome : "Nenhum"
            }));
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEquipamento([FromBody] RegistrarEquipamentoRequest req)
        {
            var equip = new Equipamento(req.Nome, req.Marca, req.ServiceTag, req.Tipo);
            _context.Equipamentos.Add(equip);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Ativo registrado no inventário corporativo." });
        }

        [HttpPost("{equipamentoId}/alocar/{usuarioId}")]
        public async Task<IActionResult> Alocar(Guid equipamentoId, Guid usuarioId)
        {
            var equip = await _context.Equipamentos.FindAsync(equipamentoId);
            if (equip == null) return NotFound("Equipamento não existe.");

            equip.AlocarParaUsuario(usuarioId);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Máquina alocada com sucesso!" });
        }
    }

    public class RegistrarEquipamentoRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string ServiceTag { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }
}
