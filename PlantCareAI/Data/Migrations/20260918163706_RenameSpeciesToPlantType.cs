using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantCareAI.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameSpeciesToPlantType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Species",
                table: "Plants",
                newName: "PlantType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlantType",
                table: "Plants",
                newName: "Species");
        }
    }
}