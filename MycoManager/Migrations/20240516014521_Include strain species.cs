using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MycoManager.Migrations
{
    /// <inheritdoc />
    public partial class Includestrainspecies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Species",
                table: "MycoStrains",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Species",
                table: "MycoStrains");
        }
    }
}
