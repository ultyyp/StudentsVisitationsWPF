using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsVisitationsWPF.Migrations
{
    public partial class AddingForeingKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "STUDENTID",
                table: "Visitations",
                newName: "StudentID");

            migrationBuilder.RenameColumn(
                name: "DATE",
                table: "Visitations",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Visitations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EMAIL",
                table: "Students",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Students",
                newName: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Visitations_StudentID",
                table: "Visitations",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_Students_StudentID",
                table: "Visitations",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_Students_StudentID",
                table: "Visitations");

            migrationBuilder.DropIndex(
                name: "IX_Visitations_StudentID",
                table: "Visitations");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Visitations",
                newName: "STUDENTID");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Visitations",
                newName: "DATE");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Visitations",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Students",
                newName: "EMAIL");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "ID");
        }
    }
}
