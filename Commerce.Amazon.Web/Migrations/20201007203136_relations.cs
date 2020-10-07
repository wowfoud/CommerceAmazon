using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Commerce.Amazon.Web.Migrations
{
    public partial class relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostPlanings_Posts_PostId",
                table: "PostPlanings");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Group_GroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_PostPlanings_PostId",
                table: "PostPlanings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Group",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "PostPlanings");

            migrationBuilder.RenameTable(
                name: "Group",
                newName: "Groups");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreate",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountNotifyPerDay",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountUsersCanNotify",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxDays",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdGroup",
                table: "Users",
                column: "IdGroup");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IdUser",
                table: "Posts",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_PostPlanings_IdUser",
                table: "PostPlanings",
                column: "IdUser");

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
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostPlanings_Posts_IdPost",
                table: "PostPlanings");

            migrationBuilder.DropForeignKey(
                name: "FK_PostPlanings_Users_IdUser",
                table: "PostPlanings");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_IdUser",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_IdGroup",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdGroup",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Posts_IdUser",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_PostPlanings_IdUser",
                table: "PostPlanings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CountNotifyPerDay",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CountUsersCanNotify",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "MaxDays",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "Group");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreate",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "PostPlanings",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Group",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Group",
                table: "Group",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostPlanings_PostId",
                table: "PostPlanings",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostPlanings_Posts_PostId",
                table: "PostPlanings",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Group_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
