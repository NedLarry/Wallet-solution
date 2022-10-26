using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet_solution.Migrations
{
    public partial class updateOnWallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WalletType",
                table: "Wallets",
                newName: "Denomination");

            migrationBuilder.AlterColumn<long>(
                name: "AccountNumber",
                table: "Wallets",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Denomination",
                table: "Wallets",
                newName: "WalletType");

            migrationBuilder.AlterColumn<int>(
                name: "AccountNumber",
                table: "Wallets",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
