using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateIdentityManager.Migrations
{
    /// <inheritdoc />
    public partial class AddPrimeiroLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PrimeiroLogin",
                table: "Pessoas",
                type: "tinyint(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimeiroLogin",
                table: "Pessoas");
        }
    }
}
