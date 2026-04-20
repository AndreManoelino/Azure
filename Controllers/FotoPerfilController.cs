using CorporateIdentityManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FotoPerfilController : ControllerBase
    {
        private readonly FotoPerfilService _fotoService;

        public FotoPerfilController(FotoPerfilService fotoService)
        {
            _fotoService = fotoService;
        }

        [HttpPost]
        public async Task<IActionResult> SalvarFoto([FromBody] SalvarFotoRequest request)
        {
            await _fotoService.SalvarFoto(request.UsuarioId, request.Base64, request.Tipo);
            return Ok();
        }

        [HttpGet("{usuarioId}/{tipo}")]
        public async Task<IActionResult> ObterFoto(Guid usuarioId, string tipo)
        {
            var foto = await _fotoService.ObterFoto(usuarioId, tipo);
            if (foto == null) return NotFound();
            return Ok(new { foto.Base64Content });
        }
    }

    public class SalvarFotoRequest
    {
        public Guid UsuarioId { get; set; }
        public string Base64 { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }
}
