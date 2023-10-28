using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftwareFest.Migrations
{
    /// <inheritdoc />
    public partial class EthPricePrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 28, 16, 53, 31, 855, DateTimeKind.Utc).AddTicks(5987),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 28, 16, 33, 56, 520, DateTimeKind.Utc).AddTicks(7039));

            migrationBuilder.AlterColumn<decimal>(
                name: "EthPrice",
                table: "Products",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 28, 16, 33, 56, 520, DateTimeKind.Utc).AddTicks(7039),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 28, 16, 53, 31, 855, DateTimeKind.Utc).AddTicks(5987));

            migrationBuilder.AlterColumn<decimal>(
                name: "EthPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);
        }
    }
}
