using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSRMGMT.Migrations
{
    /// <inheritdoc />
    public partial class addtableprokjectallocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectAllocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    AllocationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAllocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectAllocation_CsrProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "CsrProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectAllocation_ProjectAgency_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "ProjectAgency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAllocation_AgencyId",
                table: "ProjectAllocation",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAllocation_ProjectId",
                table: "ProjectAllocation",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectAllocation");
        }
    }
}
