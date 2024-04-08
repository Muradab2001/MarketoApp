using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketo.DataAccess.Migrations
{
    public partial class WishListitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "WishlistItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "WishlistItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FurnitureId",
                table: "WishlistItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_AppUserId",
                table: "WishlistItems",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_FurnitureId",
                table: "WishlistItems",
                column: "FurnitureId");

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_AspNetUsers_AppUserId",
                table: "WishlistItems",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_Furnitures_FurnitureId",
                table: "WishlistItems",
                column: "FurnitureId",
                principalTable: "Furnitures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_AspNetUsers_AppUserId",
                table: "WishlistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_Furnitures_FurnitureId",
                table: "WishlistItems");

            migrationBuilder.DropIndex(
                name: "IX_WishlistItems_AppUserId",
                table: "WishlistItems");

            migrationBuilder.DropIndex(
                name: "IX_WishlistItems_FurnitureId",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "FurnitureId",
                table: "WishlistItems");
        }
    }
}
