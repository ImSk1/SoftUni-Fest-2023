using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftwareFest.Migrations
{
    /// <inheritdoc />
    public partial class AddedStripeUserIdToBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 27, 9, 33, 57, 454, DateTimeKind.Utc).AddTicks(7749),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 27, 9, 24, 46, 934, DateTimeKind.Utc).AddTicks(4356));

            migrationBuilder.AddColumn<string>(
                name: "StripeUserId",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeUserId",
                table: "Businesses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 27, 9, 24, 46, 934, DateTimeKind.Utc).AddTicks(4356),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 27, 9, 33, 57, 454, DateTimeKind.Utc).AddTicks(7749));
        }
    }
}
