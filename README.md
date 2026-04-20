# Enterprise Active Directory & Intune Simulator

Um sistema completo de simulação de infraestrutura corporativa, construído para replicar comportamentos e processos reais encontrados na administração de Microsoft Entra ID (Azure AD), Active Directory on-premises e Microsoft Intune. 

Além da gestão centralizada de identidades, a aplicação atua como um ERP departamental escalável, controlando permissões avançadas (RBAC) e processos interdepartamentais de uma corporação moderna.

## Arquitetura e Tecnologias

- **Backend:** C# com ASP.NET Core 8 Web API
- **Banco de Dados:** MySQL (Pomelo EntityFramework Core)
- **Frontend:** React (Vite) + Vanilla CSS Modular
- **Autenticação:** JWT com Políticas Baseadas em Grupos (Roles)
- **Segurança:** Hashing de Senhas (BCrypt) e interceptação de First-Login

## Capacidades do Sistema

A arquitetura foi modelada visando separação em domínios para representar perfeitamente o ambiente de uma empresa.

### 1. Gestão de Identidades (Identity Management)
- **Provisionamento:** Criação de colaboradores atrelados a Organizações (Tenants), Departamentos e Unidades Organizacionais (OU).
- **Hard & Soft Delete:** Bloqueio de contas em vez de exclusão física para conformidade legal (Compliance).
- **Revogação de Licenças:** Rotinas automáticas de revogação de licenças financeiras em caso de bloqueio.
- **Forced Password Change:** Fluxo obrigatório de alteração de senha no primeiro login.

### 2. Controle de Acesso Baseado em Cargos (RBAC e Menus Dinâmicos)
Os acessos são governados diretamente pelos Grupos do AD. 
A interface e as ações backend respondem dinamicamente a grupos predefinidos:
- **Administração / TI Senior / Builtin:** Acesso global. Provisionamento, diretório de colaboradores e bloqueios de contas de alto risco.
- **TI Junior:** Permissão apenas para consulta de diretório de identidades (Read-only security roles).
- **Supervisores / Coordenadores:** Módulo focado em aprovações de relatórios operacionais.
- **Recursos Humanos:** Módulos de auditoria e admissão.
- **Manutenção:** Módulo prático contendo upload de evidências físicas (Fotos Base64) e relatórios operacionais.
- **Administrativo Financeiro:** Gestão de custos, relatórios de licenciamentos e cloud billing.

### 3. Modelagem de Dados Relacional
Além da estrutura organizacional padrão (Departamentos, OUs, Grupos e Políticas), o domínio engloba:
- **`DocumentoRelatorio`**: Fluxos de envio e aprovação (Workflow) ligando a base operacional aos coordenadores.
- **`FotoPerfil`**: Sistema de Storage (simulado via Blob em Base64) integrando inspeções físicas ou perfis ao Active Directory.

## Como Executar

### 1. Backend (API)
```bash
# Navegue até a pasta raiz
dotnet ef database update  # Roda as migrações (Cria as tabelas no MySQL)
dotnet run                 # Inicia o servidor ASP.NET Core (porta 5199/7268)
```

### 2. Frontend (Dashboard React)
```bash
# Abra um novo terminal e navegue para a pasta frontend
cd frontend
npm install
npm run dev                # Inicia o painel na porta 5173
```

O projeto continuará evoluindo para abraçar simulações de Intune (MDM/MAM) com conformidade de endpoints e restrições automatizadas.
