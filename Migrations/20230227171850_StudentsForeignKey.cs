using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsVisitationsWPF.Migrations
{
    public partial class StudentsForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_Students_StudentID",
                table: "Visitations");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Visitations",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Visitations_StudentID",
                table: "Visitations",
                newName: "IX_Visitations_StudentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "Visitations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_Students_StudentId",
                table: "Visitations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_Students_StudentId",
                table: "Visitations");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Visitations",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Visitations_StudentId",
                table: "Visitations",
                newName: "IX_Visitations_StudentID");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentID",
                table: "Visitations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_Students_StudentID",
                table: "Visitations",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
