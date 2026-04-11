using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateIdentityManager.Migrations
{
    /// <inheritdoc />
    public partial class AddUnidadeOrganizacionalGrupo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnidadeOrganizacionalGrupos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnidadeOrganizacionalId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GrupoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Excluido = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadeOrganizacionalGrupos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnidadeOrganizacionalGrupos_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnidadeOrganizacionalGrupos_UnidadesOrganizacionais_UnidadeO~",
                        column: x => x.UnidadeOrganizacionalId,
                        principalTable: "UnidadesOrganizacionais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuarioLicencas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LicencaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GrupoOrigemId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DataAtribuicao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Manual = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HerdadaDeGrupo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    GrupoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Excluido = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioLicencas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioLicencas_Grupos_GrupoOrigemId",
                        column: x => x.GrupoOrigemId,
                        principalTable: "Grupos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsuarioLicencas_Licencas_LicencaId",
                        column: x => x.LicencaId,
                        principalTable: "Licencas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioLicencas_Pessoas_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadeOrganizacionalGrupos_GrupoId",
                table: "UnidadeOrganizacionalGrupos",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadeOrganizacionalGrupos_UnidadeOrganizacionalId",
                table: "UnidadeOrganizacionalGrupos",
                column: "UnidadeOrganizacionalId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioLicencas_GrupoOrigemId",
                table: "UsuarioLicencas",
                column: "GrupoOrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioLicencas_LicencaId",
                table: "UsuarioLicencas",
                column: "LicencaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioLicencas_UsuarioId",
                table: "UsuarioLicencas",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnidadeOrganizacionalGrupos");

            migrationBuilder.DropTable(
                name: "UsuarioLicencas");
        }
    }
}
