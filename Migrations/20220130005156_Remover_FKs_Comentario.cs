using Microsoft.EntityFrameworkCore.Migrations;

namespace Midas.Migrations
{
    public partial class Remover_FKs_Comentario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_AspNetUsers_ApplicationUserId",
                table: "Comentario");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_Comentario_ComentariosID",
                table: "Comentario");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_Produto_ProdutoID",
                table: "Comentario");

            migrationBuilder.DropIndex(
                name: "IX_Comentario_ApplicationUserId",
                table: "Comentario");

            migrationBuilder.DropIndex(
                name: "IX_Comentario_ComentariosID",
                table: "Comentario");

            migrationBuilder.DropIndex(
                name: "IX_Comentario_ProdutoID",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "ComentarioID",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "ComentariosID",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "Mensagem",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "ProdutoID",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "UsuarioID",
                table: "Comentario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Comentario",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComentarioID",
                table: "Comentario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ComentariosID",
                table: "Comentario",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mensagem",
                table: "Comentario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProdutoID",
                table: "Comentario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioID",
                table: "Comentario",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_ApplicationUserId",
                table: "Comentario",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_ComentariosID",
                table: "Comentario",
                column: "ComentariosID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_ProdutoID",
                table: "Comentario",
                column: "ProdutoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_AspNetUsers_ApplicationUserId",
                table: "Comentario",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_Comentario_ComentariosID",
                table: "Comentario",
                column: "ComentariosID",
                principalTable: "Comentario",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_Produto_ProdutoID",
                table: "Comentario",
                column: "ProdutoID",
                principalTable: "Produto",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
