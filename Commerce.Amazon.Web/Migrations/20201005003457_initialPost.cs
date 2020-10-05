using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Commerce.Amazon.Web.Migrations
{
    public partial class initialPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "IdSociete",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocieteId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephon",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Societe",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Societe", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SocieteId",
                table: "Users",
                column: "SocieteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Societe_SocieteId",
                table: "Users",
                column: "SocieteId",
                principalTable: "Societe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Societe_SocieteId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Societe");

            migrationBuilder.DropIndex(
                name: "IX_Users_SocieteId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdSociete",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SocieteId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Telephon",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserGuid",
                table: "Users",
                nullable: false,
                defaultValue: "");
        }
    }
}
