using CorporateIdentityManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly RelatorioService _relatorioService;

        public RelatorioController(RelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarRelatorioRequest request)
        {
            var id = await _relatorioService.CriarRelatorio(request.UsuarioId, request.Titulo, request.Conteudo, request.Departamento);
            return Ok(new { id });
        }

        [HttpGet("departamento/{departamento}")]
        public async Task<IActionResult> Listar(string departamento)
        {
            var relatorios = await _relatorioService.ListarPorDepartamento(departamento);
            return Ok(relatorios.Select(r => new {
                r.Id, r.Titulo, r.Conteudo, r.Aprovado, r.DataCriacao,
                Usuario = r.Usuario?.Nome
            }));
        }

        [HttpPost("{id}/aprovar")]
        public async Task<IActionResult> Aprovar(Guid id)
        {
            await _relatorioService.AprovarRelatorio(id);
            return Ok();
        }
    }

    public class CriarRelatorioRequest
    {
        public Guid UsuarioId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
    }
}
