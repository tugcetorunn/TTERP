using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addProductionProgressEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplyStatusHistories");

            migrationBuilder.CreateTable(
                name: "ProductionProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionId = table.Column<int>(type: "int", nullable: false),
                    ProducedQuantity = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProgressDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_ProductionProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProgresses_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionProgresses_Productions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "Productions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProgresses_EmployeeId",
                table: "ProductionProgresses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProgresses_ProductionId",
                table: "ProductionProgresses",
                column: "ProductionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionProgresses");

            migrationBuilder.CreateTable(
                name: "SupplyStatusHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangedByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    SupplyId = table.Column<int>(type: "int", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FromStatusCode = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LanguageSupportId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToStatusCode = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyStatusHistories_AspNetUsers_ChangedByEmployeeId",
                        column: x => x.ChangedByEmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplyStatusHistories_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_SupplyStatusHistories_ChangedByEmployeeId",
                table: "SupplyStatusHistories",
                column: "ChangedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyStatusHistories_SupplyId",
                table: "SupplyStatusHistories",
                column: "SupplyId");
        }
    }
}
