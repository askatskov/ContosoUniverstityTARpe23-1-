using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoUniverstity.Migrations
{
    /// <inheritdoc />
    public partial class blablalhkhskkik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Instructor_InstructorID",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_InstructorID",
                table: "Departments");

            migrationBuilder.AddColumn<int>(
                name: "AdministratorId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_AdministratorId",
                table: "Departments",
                column: "AdministratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Instructor_AdministratorId",
                table: "Departments",
                column: "AdministratorId",
                principalTable: "Instructor",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Instructor_AdministratorId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_AdministratorId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "AdministratorId",
                table: "Departments");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_InstructorID",
                table: "Departments",
                column: "InstructorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Instructor_InstructorID",
                table: "Departments",
                column: "InstructorID",
                principalTable: "Instructor",
                principalColumn: "Id");
        }
    }
}
