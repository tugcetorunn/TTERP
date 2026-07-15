using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deleteIndexInMaterialWarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MaterialWarehouses_MaterialId_WarehouseId",
                table: "MaterialWarehouses");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 11, 57, 47, 429, DateTimeKind.Utc).AddTicks(1391));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 11, 57, 47, 429, DateTimeKind.Utc).AddTicks(1400));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 11, 57, 47, 429, DateTimeKind.Utc).AddTicks(1401));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 11, 57, 47, 429, DateTimeKind.Utc).AddTicks(1402));

            migrationBuilder.CreateIndex(
                name: "IX_MaterialWarehouses_MaterialId",
                table: "MaterialWarehouses",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MaterialWarehouses_MaterialId",
                table: "MaterialWarehouses");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 11, 49, 1, 235, DateTimeKind.Utc).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 11, 49, 1, 235, DateTimeKind.Utc).AddTicks(2360));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 11, 49, 1, 235, DateTimeKind.Utc).AddTicks(2362));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 11, 49, 1, 235, DateTimeKind.Utc).AddTicks(2363));

            migrationBuilder.CreateIndex(
                name: "IX_MaterialWarehouses_MaterialId_WarehouseId",
                table: "MaterialWarehouses",
                columns: new[] { "MaterialId", "WarehouseId" },
                unique: true);
        }
    }
}
