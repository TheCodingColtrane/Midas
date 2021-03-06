using Microsoft.EntityFrameworkCore.Migrations;

namespace Midas.Migrations
{
    public partial class Adicionar_FKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoque_AspNetUsers_ApplicationUserId",
                table: "Estoque");

            migrationBuilder.DropForeignKey(
                name: "FK_Produto_AspNetUsers_ApplicationUserId",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Produto_ApplicationUserId",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Estoque_ApplicationUserId",
                table: "Estoque");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Estoque");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_AnuncianteID",
                table: "Produto",
                column: "AnuncianteID");

            migrationBuilder.CreateIndex(
                name: "IX_Estoque_AnuncianteID",
                table: "Estoque",
                column: "AnuncianteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Estoque_AspNetUsers_AnuncianteID",
                table: "Estoque",
                column: "AnuncianteID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_AspNetUsers_AnuncianteID",
                table: "Produto",
                column: "AnuncianteID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoque_AspNetUsers_AnuncianteID",
                table: "Estoque");

            migrationBuilder.DropForeignKey(
                name: "FK_Produto_AspNetUsers_AnuncianteID",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Produto_AnuncianteID",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Estoque_AnuncianteID",
                table: "Estoque");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Produto",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Estoque",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produto_ApplicationUserId",
                table: "Produto",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Estoque_ApplicationUserId",
                table: "Estoque",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estoque_AspNetUsers_ApplicationUserId",
                table: "Estoque",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_AspNetUsers_ApplicationUserId",
                table: "Produto",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
