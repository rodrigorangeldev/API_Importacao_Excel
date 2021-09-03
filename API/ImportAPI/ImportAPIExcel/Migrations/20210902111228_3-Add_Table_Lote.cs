using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImportAPIExcel.Migrations
{
    public partial class _3Add_Table_Lote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoteRefId",
                table: "ExcelFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataImportacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lote", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExcelFiles_LoteRefId",
                table: "ExcelFiles",
                column: "LoteRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExcelFiles_Lote_LoteRefId",
                table: "ExcelFiles",
                column: "LoteRefId",
                principalTable: "Lote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExcelFiles_Lote_LoteRefId",
                table: "ExcelFiles");

            migrationBuilder.DropTable(
                name: "Lote");

            migrationBuilder.DropIndex(
                name: "IX_ExcelFiles_LoteRefId",
                table: "ExcelFiles");

            migrationBuilder.DropColumn(
                name: "LoteRefId",
                table: "ExcelFiles");
        }
    }
}
