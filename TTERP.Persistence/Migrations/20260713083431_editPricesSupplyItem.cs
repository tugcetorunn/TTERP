using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editPricesSupplyItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountRate",
                table: "SupplyItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetAmount",
                table: "SupplyItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SupplierMaterialId",
                table: "SupplyItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "SupplyItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 8, 34, 30, 306, DateTimeKind.Utc).AddTicks(2436));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 8, 34, 30, 306, DateTimeKind.Utc).AddTicks(2445));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 8, 34, 30, 306, DateTimeKind.Utc).AddTicks(2446));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 13, 8, 34, 30, 306, DateTimeKind.Utc).AddTicks(2447));

            migrationBuilder.CreateIndex(
                name: "IX_SupplyItems_SupplierMaterialId",
                table: "SupplyItems",
                column: "SupplierMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyItems_SupplierMaterials_SupplierMaterialId",
                table: "SupplyItems",
                column: "SupplierMaterialId",
                principalTable: "SupplierMaterials",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyItems_SupplierMaterials_SupplierMaterialId",
                table: "SupplyItems");

            migrationBuilder.DropIndex(
                name: "IX_SupplyItems_SupplierMaterialId",
                table: "SupplyItems");

            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "SupplyItems");

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "SupplyItems");

            migrationBuilder.DropColumn(
                name: "SupplierMaterialId",
                table: "SupplyItems");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "SupplyItems");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 21, 5, 32, 697, DateTimeKind.Utc).AddTicks(3133));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 21, 5, 32, 697, DateTimeKind.Utc).AddTicks(3138));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 21, 5, 32, 697, DateTimeKind.Utc).AddTicks(3139));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 12, 21, 5, 32, 697, DateTimeKind.Utc).AddTicks(3140));
        }
    }
}
