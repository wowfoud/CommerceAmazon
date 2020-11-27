using Microsoft.EntityFrameworkCore.Migrations;

namespace Commerce.Amazon.Web.Migrations
{
    public partial class userGroups2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Societes_SocieteId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdGroup",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IdSociete",
                table: "Users",
                newName: "GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "SocieteId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Societes_SocieteId",
                table: "Users",
                column: "SocieteId",
                principalTable: "Societes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Societes_SocieteId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Users",
                newName: "IdSociete");

            migrationBuilder.AlterColumn<int>(
                name: "SocieteId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "IdGroup",
                table: "Users",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Societes_SocieteId",
                table: "Users",
                column: "SocieteId",
                principalTable: "Societes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
