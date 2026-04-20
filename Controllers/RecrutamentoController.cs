using CorporateIdentityManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecrutamentoController : ControllerBase
    {
        private readonly RecrutamentoService _recrutamentoService;

        public RecrutamentoController(RecrutamentoService recrutamentoService)
        {
            _recrutamentoService = recrutamentoService;
        }

        [HttpPost("vagas")]
        public async Task<IActionResult> CriarVaga([FromBody] CriarVagaRequest request)
        {
            var id = await _recrutamentoService.CriarVaga(request.Titulo, request.Descricao, request.Departamento);
            return Ok(new { id });
        }

        [HttpGet("vagas")]
        public async Task<IActionResult> ListarVagas()
        {
            return Ok(await _recrutamentoService.ListarVagasAbertas());
        }

        [HttpPost("candidatos")]
        public async Task<IActionResult> AdicionarCandidato([FromBody] AdicionarCandidatoRequest request)
        {
            var id = await _recrutamentoService.AdicionarCandidato(request.VagaId, request.Nome, request.Email, request.Telefone, request.Curriculo);
            return Ok(new { id });
        }

        [HttpGet("candidatos/{vagaId}")]
        public async Task<IActionResult> ListarCandidatos(Guid vagaId)
        {
            return Ok(await _recrutamentoService.ListarCandidatosPorVaga(vagaId));
        }
    }

    public class CriarVagaRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
    }

    public class AdicionarCandidatoRequest
    {
        public Guid VagaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Curriculo { get; set; } = string.Empty;
    }
}
