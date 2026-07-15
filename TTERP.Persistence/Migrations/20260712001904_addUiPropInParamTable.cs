using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addUiPropInParamTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UiColor",
                table: "ParameterValues",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UiIcon",
                table: "ParameterValues",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DefaultValue",
                table: "ParameterDefinitions",
                type: "int",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UiColor",
                table: "ParameterValues");

            migrationBuilder.DropColumn(
                name: "UiIcon",
                table: "ParameterValues");

            migrationBuilder.AlterColumn<int>(
                name: "DefaultValue",
                table: "ParameterDefinitions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 11, 11, 52, 53, 860, DateTimeKind.Utc).AddTicks(9014));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 11, 11, 52, 53, 860, DateTimeKind.Utc).AddTicks(9022));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 11, 11, 52, 53, 860, DateTimeKind.Utc).AddTicks(9024));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 11, 11, 52, 53, 860, DateTimeKind.Utc).AddTicks(9025));
        }
    }
}
