using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeProject.Migrations
{
    /// <inheritdoc />
    public partial class ImageTypeModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageFile",
                table: "Profiles",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFile",
                table: "Profiles");
        }
    }
}
