using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    public partial class UpdateItemDiscriminator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ItemType",
                table: "Items",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Artist",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Book_Author",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Book_Genre",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Book_Language",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CD_Duration",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CD_Genre",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CD_ReleaseDate",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Edition",
                table: "Items",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileFormat",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FileSize",
                table: "Items",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IssueNumber",
                table: "Items",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Magazine_Genre",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PageCount",
                table: "Items",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Series",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Studio",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrackCount",
                table: "Items",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Artist",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Book_Author",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Book_Genre",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Book_Language",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CD_Duration",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CD_Genre",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CD_ReleaseDate",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Editor",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FileFormat",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Format",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IssueNumber",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Magazine_Genre",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PageCount",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Series",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Studio",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TrackCount",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "ItemType",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
