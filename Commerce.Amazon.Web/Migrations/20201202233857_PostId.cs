using Microsoft.EntityFrameworkCore.Migrations;

namespace Commerce.Amazon.Web.Migrations
{
    public partial class PostId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostPlanings_Posts_IdPost",
                table: "PostPlanings");

            migrationBuilder.DropForeignKey(
                name: "FK_PostPlanings_Users_IdUser",
                table: "PostPlanings");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "PostPlanings",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdPost",
                table: "PostPlanings",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostPlanings_IdUser",
                table: "PostPlanings",
                newName: "IX_PostPlanings_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostPlanings_Posts_PostId",
                table: "PostPlanings",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostPlanings_Users_UserId",
                table: "PostPlanings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostPlanings_Posts_PostId",
                table: "PostPlanings");

            migrationBuilder.DropForeignKey(
                name: "FK_PostPlanings_Users_UserId",
                table: "PostPlanings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PostPlanings",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "PostPlanings",
                newName: "IdPost");

            migrationBuilder.RenameIndex(
                name: "IX_PostPlanings_UserId",
                table: "PostPlanings",
                newName: "IX_PostPlanings_IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_PostPlanings_Posts_IdPost",
                table: "PostPlanings",
                column: "IdPost",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostPlanings_Users_IdUser",
                table: "PostPlanings",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
