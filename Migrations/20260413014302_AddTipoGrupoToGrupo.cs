using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateIdentityManager.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoGrupoToGrupo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoGrupo",
                table: "Grupos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoGrupo",
                table: "Grupos");
        }
    }
}
