using Microsoft.EntityFrameworkCore.Migrations;

namespace Midas.Migrations
{
    public partial class Add_FKs_Comentario2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_AspNetUsers_ApplicationUserId",
                table: "Comentario");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_Comentario_ComentariosID",
                table: "Comentario");

            migrationBuilder.DropIndex(
                name: "IX_Comentario_ApplicationUserId",
                table: "Comentario");

            migrationBuilder.DropIndex(
                name: "IX_Comentario_ComentariosID",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Comentario");

            migrationBuilder.DropColumn(
                name: "ComentariosID",
                table: "Comentario");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_ComentarioID",
                table: "Comentario",
                column: "ComentarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_UsuarioID",
                table: "Comentario",
                column: "UsuarioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_AspNetUsers_UsuarioID",
                table: "Comentario",
                column: "UsuarioID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_Comentario_ComentarioID",
                table: "Comentario",
                column: "ComentarioID",
                principalTable: "Comentario",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_AspNetUsers_UsuarioID",
                table: "Comentario");

            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_Comentario_ComentarioID",
                table: "Comentario");

            migrationBuilder.DropIndex(
                name: "IX_Comentario_ComentarioID",
                table: "Comentario");

            migrationBuilder.DropIndex(
                name: "IX_Comentario_UsuarioID",
                table: "Comentario");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Comentario",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComentariosID",
                table: "Comentario",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_ApplicationUserId",
                table: "Comentario",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_ComentariosID",
                table: "Comentario",
                column: "ComentariosID");

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
        }
    }
}
