using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Midas.Migrations
{
    public partial class FK_Nula_Comentario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_Comentario_ComentarioID",
                table: "Comentario");

            migrationBuilder.AlterColumn<int>(
                name: "ComentarioID",
                table: "Comentario",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AlteradoEm",
                table: "Comentario",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_Comentario_ComentarioID",
                table: "Comentario",
                column: "ComentarioID",
                principalTable: "Comentario",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_Comentario_ComentarioID",
                table: "Comentario");

            migrationBuilder.AlterColumn<int>(
                name: "ComentarioID",
                table: "Comentario",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AlteradoEm",
                table: "Comentario",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_Comentario_ComentarioID",
                table: "Comentario",
                column: "ComentarioID",
                principalTable: "Comentario",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
