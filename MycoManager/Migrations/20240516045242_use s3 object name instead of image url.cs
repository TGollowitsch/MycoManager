using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MycoManager.Migrations
{
    /// <inheritdoc />
    public partial class uses3objectnameinsteadofimageurl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "MycoStrains",
                newName: "S3ObjectName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "S3ObjectName",
                table: "MycoStrains",
                newName: "ImageUrl");
        }
    }
}
