using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsVisitationsWPF.Migrations
{
    public partial class AddedSubjectsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Visitations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visitations_SubjectId",
                table: "Visitations",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_Subjects_SubjectId",
                table: "Visitations",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_Subjects_SubjectId",
                table: "Visitations");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Visitations_SubjectId",
                table: "Visitations");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Visitations");
        }
    }
}
