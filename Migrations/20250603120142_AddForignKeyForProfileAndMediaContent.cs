using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeProject.Migrations
{
    /// <inheritdoc />
    public partial class AddForignKeyForProfileAndMediaContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MediaContents_ProfileId",
                table: "MediaContents",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaContents_Profiles_ProfileId",
                table: "MediaContents",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaContents_Profiles_ProfileId",
                table: "MediaContents");

            migrationBuilder.DropIndex(
                name: "IX_MediaContents_ProfileId",
                table: "MediaContents");
        }
    }
}
