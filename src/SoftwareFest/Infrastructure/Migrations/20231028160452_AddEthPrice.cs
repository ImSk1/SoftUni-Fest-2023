using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftwareFest.Migrations
{
    /// <inheritdoc />
    public partial class AddEthPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 28, 16, 4, 52, 312, DateTimeKind.Utc).AddTicks(8028),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 28, 14, 6, 12, 404, DateTimeKind.Utc).AddTicks(8785));

            migrationBuilder.AddColumn<decimal>(
                name: "EthPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EthPrice",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 28, 14, 6, 12, 404, DateTimeKind.Utc).AddTicks(8785),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 28, 16, 4, 52, 312, DateTimeKind.Utc).AddTicks(8028));
        }
    }
}
