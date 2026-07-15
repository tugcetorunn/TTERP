using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editUiPropInParamTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UiIcon",
                table: "ParameterValues",
                newName: "Icon");

            migrationBuilder.RenameColumn(
                name: "UiColor",
                table: "ParameterValues",
                newName: "CssClass");

            migrationBuilder.AddColumn<string>(
                name: "BadgeColor",
                table: "ParameterValues",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ParameterValues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                table: "ParameterValues",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "ParameterValues",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 11, 23, 17, 733, DateTimeKind.Utc).AddTicks(265));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 11, 23, 17, 733, DateTimeKind.Utc).AddTicks(272));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 11, 23, 17, 733, DateTimeKind.Utc).AddTicks(273));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 11, 23, 17, 733, DateTimeKind.Utc).AddTicks(275));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BadgeColor",
                table: "ParameterValues");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ParameterValues");

            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "ParameterValues");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "ParameterValues");

            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "ParameterValues",
                newName: "UiIcon");

            migrationBuilder.RenameColumn(
                name: "CssClass",
                table: "ParameterValues",
                newName: "UiColor");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 0, 19, 3, 25, DateTimeKind.Utc).AddTicks(5960));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 0, 19, 3, 25, DateTimeKind.Utc).AddTicks(5969));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 0, 19, 3, 25, DateTimeKind.Utc).AddTicks(5971));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 0, 19, 3, 25, DateTimeKind.Utc).AddTicks(5972));
        }
    }
}
