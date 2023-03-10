using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsVisitationsWPF.Migrations
{
    public partial class FixedIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Visitations_Date",
                table: "Visitations",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Name",
                table: "Subjects",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FIO",
                table: "Students",
                column: "FIO");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name",
                table: "Groups",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Visitations_Date",
                table: "Visitations");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_Name",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Students_FIO",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Groups_Name",
                table: "Groups");
        }
    }
}
