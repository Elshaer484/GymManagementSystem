using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagment.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "plans",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "plans",
                newName: "createdAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedAt",
                table: "plans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "plans",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "plans",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "plans",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "plans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "plans",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
