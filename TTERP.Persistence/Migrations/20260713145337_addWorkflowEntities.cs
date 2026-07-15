using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addWorkflowEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplyStatusHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplyId = table.Column<int>(type: "int", nullable: false),
                    FromStatusCode = table.Column<int>(type: "int", nullable: true),
                    ToStatusCode = table.Column<int>(type: "int", nullable: false),
                    ChangedByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "WorkflowHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowType = table.Column<int>(type: "int", nullable: false),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    FromStatusCode = table.Column<int>(type: "int", nullable: true),
                    ToStatusCode = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_WorkflowHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowHistories_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowTransitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowType = table.Column<int>(type: "int", nullable: false),
                    FromStatusCode = table.Column<int>(type: "int", nullable: false),
                    ToStatusCode = table.Column<int>(type: "int", nullable: false),
                    ActionCode = table.Column<int>(type: "int", nullable: false),
                    RequiredRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequiresConfirmation = table.Column<bool>(type: "bit", nullable: false),
                    CreatesStockMovement = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_WorkflowTransitions", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "WorkflowTransitions",
                columns: new[] { "Id", "ActionCode", "CreatedDate", "CreatesStockMovement", "DeletedBy", "DeletedDate", "DisplayOrder", "FromStatusCode", "IsActive", "LanguageSupportId", "RequiredRole", "RequiresConfirmation", "ToStatusCode", "UpdatedBy", "UpdatedDate", "WorkflowType" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 1, 1, true, 1, null, false, 2, null, null, 1 },
                    { 2, 3, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 2, 2, true, 1, null, false, 3, null, null, 1 },
                    { 3, 4, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, null, 3, 3, true, 1, null, true, 4, null, null, 1 },
                    { 4, 5, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 4, 1, true, 1, null, true, 5, null, null, 1 },
                    { 5, 5, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 5, 2, true, 1, null, true, 5, null, null, 1 },
                    { 6, 5, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 6, 3, true, 1, null, true, 5, null, null, 1 },
                    { 7, 2, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 1, 1, true, 1, null, true, 2, null, null, 2 },
                    { 8, 3, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 2, 2, true, 1, null, false, 3, null, null, 2 },
                    { 9, 2, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 3, 3, true, 1, null, false, 2, null, null, 2 },
                    { 10, 4, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, null, 4, 2, true, 1, null, true, 4, null, null, 2 },
                    { 11, 5, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 5, 1, true, 1, null, true, 5, null, null, 2 },
                    { 12, 5, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 6, 2, true, 1, null, true, 5, null, null, 2 },
                    { 13, 5, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 7, 3, true, 1, null, true, 5, null, null, 2 },
                    { 14, 2, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 1, 1, true, 1, null, false, 2, null, null, 3 },
                    { 15, 3, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 2, 2, true, 1, null, true, 3, null, null, 3 },
                    { 16, 4, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 3, 2, true, 1, null, true, 4, null, null, 3 },
                    { 17, 5, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, null, 4, 3, true, 1, null, true, 5, null, null, 3 },
                    { 18, 6, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 5, 1, true, 1, null, true, 6, null, null, 3 },
                    { 19, 6, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 6, 2, true, 1, null, true, 6, null, null, 3 },
                    { 20, 6, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 7, 3, true, 1, null, true, 6, null, null, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplyStatusHistories_ChangedByEmployeeId",
                table: "SupplyStatusHistories",
                column: "ChangedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyStatusHistories_SupplyId",
                table: "SupplyStatusHistories",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistories_EmployeeId",
                table: "WorkflowHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitions_WorkflowType_FromStatusCode_ToStatusCode",
                table: "WorkflowTransitions",
                columns: new[] { "WorkflowType", "FromStatusCode", "ToStatusCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplyStatusHistories");

            migrationBuilder.DropTable(
                name: "WorkflowHistories");

            migrationBuilder.DropTable(
                name: "WorkflowTransitions");

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
        }
    }
}
