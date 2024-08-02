using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddCopiesToItem : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "AvailableCopies",
            table: "Items",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "TotalCopies",
            table: "Items",
            nullable: false,
            defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AvailableCopies",
            table: "Items");

        migrationBuilder.DropColumn(
            name: "TotalCopies",
            table: "Items");
    }
}
