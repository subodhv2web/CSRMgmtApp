using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSRMGMT.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnprojectcategoryid1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCategoryId",
                table: "Milestone");

            migrationBuilder.AddColumn<int>(
                name: "ProjectCategoryId",
                table: "CsrProject",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCategoryId",
                table: "CsrProject");

            migrationBuilder.AddColumn<int>(
                name: "ProjectCategoryId",
                table: "Milestone",
                type: "int",
                nullable: true);
        }
    }
}
