using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    public partial class AddStatusToFine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FinePayRequests");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Fines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_ItemId",
                table: "Reviews",
                columns: new[] { "UserId", "ItemId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId_ItemId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Fines");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FinePayRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");
        }
    }
}
