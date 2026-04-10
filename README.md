 Active Directory, Entra ID e Intune



Com base na minha vivência e estudos voltados para administração de ambientes corporativos utilizando **Active Directory**, **Microsoft Entra ID** e **Microsoft Intune**, desenvolvi este projeto com o objetivo de simular em software diversos processos e estruturas presentes no gerenciamento de identidade empresarial.

A proposta é transformar conhecimentos práticos de infraestrutura e administração de diretórios em uma aplicação desenvolvida em **.NET 8**, aplicando conceitos de arquitetura limpa, modelagem orientada a objetos e boas práticas de desenvolvimento backend.

O sistema busca representar de forma estruturada cenários comuns encontrados em ambientes corporativos, como provisionamento de usuários, organização hierárquica, autenticação, controle de permissões, políticas de segurança e gerenciamento de dispositivos.

---

## Tecnologias Utilizadas

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core 8
* MySQL
* Pomelo EntityFramework Provider
* Swagger / OpenAPI
* JWT Authentication
* AutoMapper
* FluentValidation
* MediatR
* BCrypt
* Serilog

---

## Objetivo da Aplicação

O sistema está sendo estruturado para simular recursos encontrados em ambientes empresariais reais, permitindo aplicar conceitos administrativos e técnicos relacionados a identidade e segurança digital.

Principais objetivos da aplicação:

* Simular gerenciamento de usuários corporativos
* Representar estruturas hierárquicas organizacionais
* Controlar departamentos e unidades organizacionais
* Implementar autenticação corporativa e políticas de acesso
* Simular grupos de segurança e permissões
* Estruturar futura gestão de dispositivos e políticas de compliance
* Simular mecanismos de MFA e segurança de identidade

---

## Estrutura Atual do Projeto

A arquitetura foi organizada com separação de responsabilidades em camadas, visando escalabilidade e manutenção:

```bash
Domain/
Application/
Infrastructure/
Persistence/
Controllers/
Configurations/
```

---

## Modelagem Implementada Até o Momento

### BaseEntity

Classe base responsável por fornecer propriedades comuns para todas as entidades:

* Id único (GUID)
* Data de criação
* Data de atualização
* Controle de ativo/inativo
* Soft Delete lógico

---

### Pessoa

Entidade responsável por armazenar informações pessoais:

* Nome
* Sobrenome
* CPF
* E-mail
* Telefone
* Data de nascimento
* Endereço relacionado

---

### Usuario

Extensão da entidade Pessoa contendo informações corporativas:

* UPN/Login
* Employee ID
* Domínio
* Senha Hash
* MFA
* Tentativas de Login
* Bloqueio de Conta
* Expiração de Senha
* Último Login
* Organização vinculada
* Departamento vinculado
* Unidade Organizacional vinculada

---

### Endereco

Entidade separada para controle de localização:

* CEP
* Rua
* Número
* Bairro
* Cidade
* Estado
* País
* Complemento

---

### Organizacao

Representa a empresa/tenant principal:

* Nome
* CNPJ
* Domínio Principal
* Tenant ID

---

### Departamento

Representa divisões internas da organização:

* Nome
* Descrição
* Organização vinculada

---

### UnidadeOrganizacional

Simula estrutura hierárquica de OU/UO:

* Nome
* Descrição
* Departamento vinculado
* Unidade Pai
* Hierarquia recursiva

---

## Próximas Implementações Planejadas

* Sistema de Grupos e Permissões
* RBAC (Role Based Access Control)
* Licenciamento Microsoft por Grupo
* MFA Completo
* Controle de Sessões/Login
* Gestão de Máquinas/Endpoints
* Simulação de Políticas do Intune
* Restrição de Softwares/Instalações
* Compliance e Inventário de Dispositivos

---

## Finalidade Técnica

Além do desenvolvimento da aplicação, este projeto também tem como finalidade consolidar conhecimentos práticos em:

* Modelagem de domínio orientada a objetos
* Arquitetura backend enterprise
* Simulação de estruturas de diretório corporativo
* Regras de autenticação/autorização
* Integração entre conceitos de infraestrutura e desenvolvimento de software

---
