using Microsoft.EntityFrameworkCore.Migrations;

namespace Commerce.Amazon.Web.Migrations
{
    public partial class userGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_IdUser",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_IdGroup",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdGroup",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Posts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_IdUser",
                table: "Posts",
                newName: "IX_Posts_UserId");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoutUsers",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUsers", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupUsers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupId",
                table: "GroupUsers",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CoutUsers",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Posts",
                newName: "IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                newName: "IX_Posts_IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdGroup",
                table: "Users",
                column: "IdGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_IdUser",
                table: "Posts",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_IdGroup",
                table: "Users",
                column: "IdGroup",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
