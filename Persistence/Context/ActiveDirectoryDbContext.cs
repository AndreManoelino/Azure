using CorporateIdentityManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Persistence.Context
{
    public class ActiveDirectoryDbContext : DbContext
    {
        public ActiveDirectoryDbContext(
            DbContextOptions<ActiveDirectoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Endereco> Enderecos { get; set; }

        public DbSet<Organizacao> Organizacoes { get; set; }

        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<UnidadeOrganizacional> UnidadesOrganizacionais { get; set; }
    }
}