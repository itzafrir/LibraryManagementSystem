using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    public partial class UpdateFine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Fines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LoanRequests_ItemId",
                table: "LoanRequests",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanRequests_Items_ItemId",
                table: "LoanRequests",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanRequests_Items_ItemId",
                table: "LoanRequests");

            migrationBuilder.DropIndex(
                name: "IX_LoanRequests_ItemId",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Fines");
        }
    }
}
