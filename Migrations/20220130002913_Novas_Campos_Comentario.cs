using Microsoft.EntityFrameworkCore.Migrations;

namespace Midas.Migrations
{
    public partial class Novas_Campos_Comentario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mensagem",
                table: "Comentario",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mensagem",
                table: "Comentario");
        }
    }
}
