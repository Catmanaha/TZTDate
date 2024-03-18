using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TZTDate.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SearchingAgeEnd",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SearchingAgeStart",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SearchingGender",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interests",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SearchingAgeEnd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SearchingAgeStart",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SearchingGender",
                table: "AspNetUsers");
        }
    }
}
