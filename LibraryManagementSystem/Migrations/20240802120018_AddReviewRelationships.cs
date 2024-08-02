using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    public partial class AddReviewRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanRequest_Users_UserId",
                table: "LoanRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Items_ItemId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanRequest",
                table: "LoanRequest");

            migrationBuilder.RenameTable(
                name: "LoanRequest",
                newName: "LoanRequests");

            migrationBuilder.RenameIndex(
                name: "IX_LoanRequest_UserId",
                table: "LoanRequests",
                newName: "IX_LoanRequests_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanRequests",
                table: "LoanRequests",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanRequests_Users_UserId",
                table: "LoanRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Items_ItemId",
                table: "Reviews",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanRequests_Users_UserId",
                table: "LoanRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Items_ItemId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanRequests",
                table: "LoanRequests");

            migrationBuilder.RenameTable(
                name: "LoanRequests",
                newName: "LoanRequest");

            migrationBuilder.RenameIndex(
                name: "IX_LoanRequests_UserId",
                table: "LoanRequest",
                newName: "IX_LoanRequest_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Reviews",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanRequest",
                table: "LoanRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanRequest_Users_UserId",
                table: "LoanRequest",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Items_ItemId",
                table: "Reviews",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
