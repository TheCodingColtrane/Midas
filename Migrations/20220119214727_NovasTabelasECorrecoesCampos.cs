using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Midas.Migrations
{
    public partial class NovasTabelasECorrecoesCampos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desconto",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "DescontoAplicado",
                table: "Produto");

            migrationBuilder.RenameColumn(
                name: "AnuacianteID",
                table: "Produto",
                newName: "AnucianteID");

            migrationBuilder.RenameColumn(
                name: "Previsao_Chegada",
                table: "Estoque",
                newName: "PrevisaoChegada");

            migrationBuilder.RenameColumn(
                name: "Codigu_Unico",
                table: "Endereco",
                newName: "CodigoUnico");

            migrationBuilder.AddColumn<int>(
                name: "DepartamentoID",
                table: "Produto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PromocaoID",
                table: "Produto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "AnucianteID",
                table: "Estoque",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Promocao",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Porcentagem = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Inicio = table.Column<DateTime>(type: "Date", nullable: false),
                    Fim = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocao", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_DepartamentoID",
                table: "Produto",
                column: "DepartamentoID");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_PromocaoID",
                table: "Produto",
                column: "PromocaoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Departamento_DepartamentoID",
                table: "Produto",
                column: "DepartamentoID",
                principalTable: "Departamento",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Promocao_PromocaoID",
                table: "Produto",
                column: "PromocaoID",
                principalTable: "Promocao",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Departamento_DepartamentoID",
                table: "Produto");

            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Promocao_PromocaoID",
                table: "Produto");

            migrationBuilder.DropTable(
                name: "Departamento");

            migrationBuilder.DropTable(
                name: "Promocao");

            migrationBuilder.DropIndex(
                name: "IX_Produto_DepartamentoID",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Produto_PromocaoID",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "DepartamentoID",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "PromocaoID",
                table: "Produto");

            migrationBuilder.RenameColumn(
                name: "AnucianteID",
                table: "Produto",
                newName: "AnuacianteID");

            migrationBuilder.RenameColumn(
                name: "PrevisaoChegada",
                table: "Estoque",
                newName: "Previsao_Chegada");

            migrationBuilder.RenameColumn(
                name: "CodigoUnico",
                table: "Endereco",
                newName: "Codigu_Unico");

            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                table: "Produto",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "DescontoAplicado",
                table: "Produto",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "AnucianteID",
                table: "Estoque",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
