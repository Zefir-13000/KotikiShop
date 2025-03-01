using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KotikiShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class catFamilyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cats_CatFamilies_CatFamilyId",
                table: "Cats");

            migrationBuilder.AlterColumn<int>(
                name: "CatFamilyId",
                table: "Cats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cats_CatFamilies_CatFamilyId",
                table: "Cats",
                column: "CatFamilyId",
                principalTable: "CatFamilies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cats_CatFamilies_CatFamilyId",
                table: "Cats");

            migrationBuilder.AlterColumn<int>(
                name: "CatFamilyId",
                table: "Cats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Cats_CatFamilies_CatFamilyId",
                table: "Cats",
                column: "CatFamilyId",
                principalTable: "CatFamilies",
                principalColumn: "Id");
        }
    }
}
