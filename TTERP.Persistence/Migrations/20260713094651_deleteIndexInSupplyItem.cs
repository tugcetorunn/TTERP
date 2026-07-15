using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deleteIndexInSupplyItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SupplyItems_MaterialId_WarehouseId",
                table: "SupplyItems");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 9, 46, 50, 359, DateTimeKind.Utc).AddTicks(8784));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 9, 46, 50, 359, DateTimeKind.Utc).AddTicks(8793));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 9, 46, 50, 359, DateTimeKind.Utc).AddTicks(8795));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 9, 46, 50, 359, DateTimeKind.Utc).AddTicks(8796));

            migrationBuilder.CreateIndex(
                name: "IX_SupplyItems_MaterialId",
                table: "SupplyItems",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SupplyItems_MaterialId",
                table: "SupplyItems");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 8, 40, 14, 218, DateTimeKind.Utc).AddTicks(5645));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 8, 40, 14, 218, DateTimeKind.Utc).AddTicks(5659));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 8, 40, 14, 218, DateTimeKind.Utc).AddTicks(5660));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 8, 40, 14, 218, DateTimeKind.Utc).AddTicks(5661));

            migrationBuilder.CreateIndex(
                name: "IX_SupplyItems_MaterialId_WarehouseId",
                table: "SupplyItems",
                columns: new[] { "MaterialId", "WarehouseId" },
                unique: true);
        }
    }
}
