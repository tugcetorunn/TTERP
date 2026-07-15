using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class exceptionHandlingForProgressesEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionProgresses_AspNetUsers_EmployeeId",
                table: "ProductionProgresses");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 8, 41, 3, 896, DateTimeKind.Utc).AddTicks(3084));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 8, 41, 3, 896, DateTimeKind.Utc).AddTicks(3093));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 8, 41, 3, 896, DateTimeKind.Utc).AddTicks(3094));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 8, 41, 3, 896, DateTimeKind.Utc).AddTicks(3095));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionProgresses_AspNetUsers_EmployeeId",
                table: "ProductionProgresses",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionProgresses_AspNetUsers_EmployeeId",
                table: "ProductionProgresses");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 1, 10, 1, 805, DateTimeKind.Utc).AddTicks(1582));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 1, 10, 1, 805, DateTimeKind.Utc).AddTicks(1593));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 1, 10, 1, 805, DateTimeKind.Utc).AddTicks(1594));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 1, 10, 1, 805, DateTimeKind.Utc).AddTicks(1596));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionProgresses_AspNetUsers_EmployeeId",
                table: "ProductionProgresses",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
