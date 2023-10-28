using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftwareFest.Migrations
{
    /// <inheritdoc />
    public partial class WalletAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 28, 16, 33, 56, 520, DateTimeKind.Utc).AddTicks(7039),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 28, 16, 4, 52, 312, DateTimeKind.Utc).AddTicks(8028));

            migrationBuilder.AddColumn<string>(
                name: "EthereumWalletAddress",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EthereumWalletAddress",
                table: "Businesses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 28, 16, 4, 52, 312, DateTimeKind.Utc).AddTicks(8028),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 28, 16, 33, 56, 520, DateTimeKind.Utc).AddTicks(7039));
        }
    }
}
