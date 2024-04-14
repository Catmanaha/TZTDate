using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TZTDate.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class Addfollowmembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "FollowedId",
                table: "AspNetUsers",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "FollowersId",
                table: "AspNetUsers",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowedId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FollowersId",
                table: "AspNetUsers");
        }
    }
}
