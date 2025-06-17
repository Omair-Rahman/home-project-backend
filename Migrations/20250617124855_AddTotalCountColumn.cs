using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeProject.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalCountColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalMediaContents",
                table: "Profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "MediaContents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "MediaContents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalMediaContents",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "MediaContents");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "MediaContents");
        }
    }
}
