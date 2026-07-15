using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addMaterialStockReservationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialStockReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionId = table.Column<int>(type: "int", nullable: false),
                    ProductionItemId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantity = table.Column<double>(type: "float", nullable: false),
                    ConsumedQuantity = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    IsReleased = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleasedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    LanguageSupportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialStockReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialStockReservations_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialStockReservations_ProductionItems_ProductionItemId",
                        column: x => x.ProductionItemId,
                        principalTable: "ProductionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialStockReservations_Productions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "Productions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialStockReservations_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_MaterialStockReservations_MaterialId_WarehouseId_IsReleased",
                table: "MaterialStockReservations",
                columns: new[] { "MaterialId", "WarehouseId", "IsReleased" });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialStockReservations_ProductionId_ProductionItemId",
                table: "MaterialStockReservations",
                columns: new[] { "ProductionId", "ProductionItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialStockReservations_ProductionItemId",
                table: "MaterialStockReservations",
                column: "ProductionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialStockReservations_WarehouseId",
                table: "MaterialStockReservations",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialStockReservations");

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
        }
    }
}
