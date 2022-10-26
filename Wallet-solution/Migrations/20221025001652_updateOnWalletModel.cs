using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet_solution.Migrations
{
    public partial class updateOnWalletModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Denomination",
                table: "Wallets",
                newName: "WalletType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WalletType",
                table: "Wallets",
                newName: "Denomination");
        }
    }
}
