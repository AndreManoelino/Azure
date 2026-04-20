using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Application.Services
{
    public class FotoPerfilService(ActiveDirectoryDbContext context)
    {
        private readonly ActiveDirectoryDbContext _context = context;

        public async Task SalvarFoto(Guid usuarioId, string base64, string tipo)
        {
            var fotoExistente = await _context.FotosPerfil.FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.TipoSessao == tipo);
            if (fotoExistente != null)
            {
                _context.FotosPerfil.Remove(fotoExistente);
            }

            var foto = new FotoPerfil(base64, tipo, usuarioId);
            _context.FotosPerfil.Add(foto);
            await _context.SaveChangesAsync();
        }

        public async Task<FotoPerfil?> ObterFoto(Guid usuarioId, string tipo)
        {
            return await _context.FotosPerfil.FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.TipoSessao == tipo);
        }
    }
}
