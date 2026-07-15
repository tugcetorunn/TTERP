using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editSupplierMaterialIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SupplierMaterials_SupplierId_MaterialId",
                table: "SupplierMaterials");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "Supplies",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CustomerBalance",
                table: "Customers",
                type: "decimal(18,2)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_SupplierMaterials_SupplierId_MaterialId_Currency",
                table: "SupplierMaterials",
                columns: new[] { "SupplierId", "MaterialId", "Currency" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SupplierMaterials_SupplierId_MaterialId_Currency",
                table: "SupplierMaterials");

            migrationBuilder.DropColumn(
                name: "CustomerBalance",
                table: "Customers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "Supplies",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

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

            migrationBuilder.CreateIndex(
                name: "IX_SupplierMaterials_SupplierId_MaterialId",
                table: "SupplierMaterials",
                columns: new[] { "SupplierId", "MaterialId" },
                unique: true);
        }
    }
}
