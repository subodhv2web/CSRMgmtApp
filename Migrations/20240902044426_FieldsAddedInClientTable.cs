using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSRMGMT.Migrations
{
    /// <inheritdoc />
    public partial class FieldsAddedInClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltPhoneNo",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "SpocPhoneNo",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "SpocName",
                table: "Client",
                newName: "SPOCName");

            migrationBuilder.RenameColumn(
                name: "SpocEmail",
                table: "Client",
                newName: "SPOCEmail");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "Client",
                newName: "ModifiedDate");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Client",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Client",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Client",
                newName: "IsActive");

            migrationBuilder.AlterColumn<string>(
                name: "SPOCName",
                table: "Client",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "SPOCEmail",
                table: "Client",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Client",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Client",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Client",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Client",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Client",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Client",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SPOCPhoneNumber",
                table: "Client",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SPOCPosition",
                table: "Client",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Client",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Client",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "SPOCPhoneNumber",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "SPOCPosition",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "SPOCName",
                table: "Client",
                newName: "SpocName");

            migrationBuilder.RenameColumn(
                name: "SPOCEmail",
                table: "Client",
                newName: "SpocEmail");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Client",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "Client",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Client",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Client",
                newName: "CreatedOn");

            migrationBuilder.AlterColumn<string>(
                name: "SpocName",
                table: "Client",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "SpocEmail",
                table: "Client",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Client",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "AltPhoneNo",
                table: "Client",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpocPhoneNo",
                table: "Client",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
