using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deleteIndexInProductWarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductWarehouses_ProductId_WarehouseId",
                table: "ProductWarehouses");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 15, 0, 33, 13, 527, DateTimeKind.Utc).AddTicks(3137));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 15, 0, 33, 13, 527, DateTimeKind.Utc).AddTicks(3144));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 15, 0, 33, 13, 527, DateTimeKind.Utc).AddTicks(3146));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 15, 0, 33, 13, 527, DateTimeKind.Utc).AddTicks(3147));

            migrationBuilder.CreateIndex(
                name: "IX_ProductWarehouses_ProductId",
                table: "ProductWarehouses",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductWarehouses_ProductId",
                table: "ProductWarehouses");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 19, 37, 19, 944, DateTimeKind.Utc).AddTicks(6412));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 19, 37, 19, 944, DateTimeKind.Utc).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 19, 37, 19, 944, DateTimeKind.Utc).AddTicks(6420));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 14, 19, 37, 19, 944, DateTimeKind.Utc).AddTicks(6421));

            migrationBuilder.CreateIndex(
                name: "IX_ProductWarehouses_ProductId_WarehouseId",
                table: "ProductWarehouses",
                columns: new[] { "ProductId", "WarehouseId" },
                unique: true);
        }
    }
}
