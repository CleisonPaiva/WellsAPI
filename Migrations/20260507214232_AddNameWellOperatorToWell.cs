using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNameWellOperatorToWell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameWellOperator",
                table: "Wells",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameWellOperator",
                table: "Wells");
        }
    }
}
