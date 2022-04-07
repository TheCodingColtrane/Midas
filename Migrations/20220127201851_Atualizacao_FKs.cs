using Microsoft.EntityFrameworkCore.Migrations;

namespace Midas.Migrations
{
    public partial class Atualizacao_FKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnucianteID",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "AnucianteID",
                table: "Estoque");

            migrationBuilder.AddColumn<string>(
                name: "AnuncianteID",
                table: "Produto",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnuncianteID",
                table: "Estoque",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioID",
                table: "Comentario",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnuncianteID",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "AnuncianteID",
                table: "Estoque");

            migrationBuilder.AddColumn<string>(
                name: "AnucianteID",
                table: "Produto",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnucianteID",
                table: "Estoque",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioID",
                table: "Comentario",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
