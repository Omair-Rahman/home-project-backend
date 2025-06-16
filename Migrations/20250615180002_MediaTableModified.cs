using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeProject.Migrations
{
    /// <inheritdoc />
    public partial class MediaTableModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullPath",
                table: "MediaContents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewPath",
                table: "MediaContents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullPath",
                table: "MediaContents");

            migrationBuilder.DropColumn(
                name: "PreviewPath",
                table: "MediaContents");
        }
    }
}
