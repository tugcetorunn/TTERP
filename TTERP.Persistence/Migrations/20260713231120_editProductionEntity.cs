using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editProductionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "Productions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedDate",
                table: "Productions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 23, 11, 18, 335, DateTimeKind.Utc).AddTicks(8111));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 23, 11, 18, 335, DateTimeKind.Utc).AddTicks(8118));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 23, 11, 18, 335, DateTimeKind.Utc).AddTicks(8119));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 23, 11, 18, 335, DateTimeKind.Utc).AddTicks(8120));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "Productions");

            migrationBuilder.DropColumn(
                name: "StartedDate",
                table: "Productions");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 14, 53, 36, 102, DateTimeKind.Utc).AddTicks(2300));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 14, 53, 36, 102, DateTimeKind.Utc).AddTicks(2306));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 14, 53, 36, 102, DateTimeKind.Utc).AddTicks(2308));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 14, 53, 36, 102, DateTimeKind.Utc).AddTicks(2309));
        }
    }
}
