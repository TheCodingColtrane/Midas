using Microsoft.EntityFrameworkCore.Migrations;

namespace Midas.Migrations
{
    public partial class NovosCampos_TBL_Promocao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Promocao",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstaAtiva",
                table: "Promocao",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Promocao");

            migrationBuilder.DropColumn(
                name: "EstaAtiva",
                table: "Promocao");
        }
    }
}
