using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KotikiShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CatImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cats",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cats");
        }
    }
}
