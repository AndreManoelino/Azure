using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Usuario : Pessoa
    {
        public string UPN { get; protected set; } =string.Empty;
        public string EmployeeId { get; protected set; } = string.Empty;
        public string Dominio { get; protected set; } = string.Empty;
        public string SenhaHash { get; protected set; } = string.Empty;
        public bool MFAHabilitado { get; protected set;}
        public bool ContaBloqueada { get; protected set;}
        public int TentativasLogin { get; protected set; }
        public DateTime? UltimoLogin { get; protected set; }
        public DateTime ExpiracaoSenha { get; protected set; }
        public Guid OrganizacaoId { get; protected set; }
        public Organizacao? Organizacao { get; protected set; }
        public Guid DepartamentoId { get; protected set;}
        public Departamento? Departamento { get; protected set; }
        public Guid UnidadeOrganizacionalId { get; protected set;}
        public UnidadeOrganizacional? UnidadeOrganizacional { get; protected set; }
        public ICollection<UsuarioGrupo> UsuarioGrupos { get; protected set; } = new List<UsuarioGrupo>();
        public ICollection<UsuarioRole> UsuarioRoles { get; protected set; } = new List<UsuarioRole>();
        public bool PrimeiroLogin { get; protected set; }
        public Usuario(){}
        public Usuario(string nome, string sobrenome, string cpf,
            string email, string telefone, DateTime dataNascimento,
            string upn, string employeeId, string dominio, string senhaHash, Guid organizacaoId,
            Guid departamentoId, Guid unidadeOrganizacionalId)
            : base(nome, sobrenome, cpf, email, telefone, dataNascimento)
        {
            UPN = upn;
            EmployeeId = employeeId;
            Dominio = dominio;
            SenhaHash = senhaHash;
            MFAHabilitado = false;
            ContaBloqueada = false;
            TentativasLogin = 0;
            ExpiracaoSenha = DateTime.UtcNow.AddDays(90);
            OrganizacaoId = organizacaoId;
            DepartamentoId = departamentoId;
            UnidadeOrganizacionalId = unidadeOrganizacionalId;
            PrimeiroLogin = true;
        }
        public void DefinirNovaSenha(string novaSenha)
        {
            SenhaHash = novaSenha;
            PrimeiroLogin = false;
            TentativasLogin = 0;
        }

    }
}