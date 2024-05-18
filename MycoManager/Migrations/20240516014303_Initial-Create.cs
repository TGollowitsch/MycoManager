using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MycoManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EdgeSensors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetTemperature = table.Column<double>(type: "float", nullable: false),
                    SetHumidity = table.Column<double>(type: "float", nullable: false),
                    SetCO2PPM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeSensors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MycoStrains",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MycoStrains", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EdgeSensors");

            migrationBuilder.DropTable(
                name: "MycoStrains");
        }
    }
}
