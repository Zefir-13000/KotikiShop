using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KotikiShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CatCommentUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CatComments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CatComments_UserId",
                table: "CatComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatComments_AspNetUsers_UserId",
                table: "CatComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatComments_AspNetUsers_UserId",
                table: "CatComments");

            migrationBuilder.DropIndex(
                name: "IX_CatComments_UserId",
                table: "CatComments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CatComments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
