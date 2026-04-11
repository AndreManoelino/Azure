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

        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<UsuarioGrupo> UsuarioGrupos { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermissao> RolePermissoes { get; set; }
        public DbSet<UsuarioRole> UsuarioRoles { get; set; }
    }
}