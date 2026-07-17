using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TTERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addOrderEntityStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkflowTransitions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.AddColumn<bool>(
                name: "CanChangeShipping",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanCreateInvoice",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanTakePayment",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "InvoicedAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsStockProcessed",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StockProcessedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OverrideUnitPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 17, 2, 35, 8, 284, DateTimeKind.Utc).AddTicks(1907));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 17, 2, 35, 8, 284, DateTimeKind.Utc).AddTicks(1919));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 17, 2, 35, 8, 284, DateTimeKind.Utc).AddTicks(1920));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 17, 2, 35, 8, 284, DateTimeKind.Utc).AddTicks(1921));

            migrationBuilder.InsertData(
                table: "WorkflowTransitions",
                columns: new[] { "Id", "ActionCode", "CreatedDate", "CreatesStockMovement", "DeletedBy", "DeletedDate", "DisplayOrder", "FromStatusCode", "IsActive", "LanguageSupportId", "RequiredRole", "RequiresConfirmation", "ToStatusCode", "UpdatedBy", "UpdatedDate", "WorkflowType" },
                values: new object[,]
                {
                    { 21, 2, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 1, 1, true, 1, null, false, 2, null, null, 7 },
                    { 22, 3, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 2, 2, true, 1, null, true, 3, null, null, 7 },
                    { 23, 4, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 3, 3, true, 1, null, false, 4, null, null, 7 },
                    { 24, 5, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 5, 4, true, 1, null, true, 5, null, null, 7 },
                    { 25, 6, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 6, 1, true, 1, null, true, 6, null, null, 7 },
                    { 26, 6, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 7, 2, true, 1, null, true, 6, null, null, 7 },
                    { 27, 6, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 8, 3, true, 1, null, true, 6, null, null, 7 },
                    { 28, 6, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 9, 4, true, 1, null, true, 6, null, null, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkflowTransitions",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "WorkflowTransitions",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "WorkflowTransitions",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "WorkflowTransitions",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "WorkflowTransitions",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "WorkflowTransitions",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "WorkflowTransitions",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "WorkflowTransitions",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DropColumn(
                name: "CanChangeShipping",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CanCreateInvoice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CanTakePayment",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "InvoicedAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsStockProcessed",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RemainingAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StockProcessedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OverrideUnitPrice",
                table: "OrderItems");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 16, 22, 46, 14, 303, DateTimeKind.Utc).AddTicks(7426));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 16, 22, 46, 14, 303, DateTimeKind.Utc).AddTicks(7432));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 16, 22, 46, 14, 303, DateTimeKind.Utc).AddTicks(7434));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 7, 16, 22, 46, 14, 303, DateTimeKind.Utc).AddTicks(7435));

            migrationBuilder.InsertData(
                table: "WorkflowTransitions",
                columns: new[] { "Id", "ActionCode", "CreatedDate", "CreatesStockMovement", "DeletedBy", "DeletedDate", "DisplayOrder", "FromStatusCode", "IsActive", "LanguageSupportId", "RequiredRole", "RequiresConfirmation", "ToStatusCode", "UpdatedBy", "UpdatedDate", "WorkflowType" },
                values: new object[] { 17, 5, new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, null, 4, 3, true, 1, null, true, 5, null, null, 3 });
        }
    }
}
