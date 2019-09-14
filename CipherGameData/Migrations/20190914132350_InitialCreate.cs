using Microsoft.EntityFrameworkCore.Migrations;

namespace CipherGameData.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ciphers",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Place = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciphers", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: false),
                    TeamCode = table.Column<string>(nullable: true),
                    CipherCode = table.Column<string>(nullable: true),
                    IsPlaceFound = table.Column<bool>(nullable: false),
                    IsAnswerFound = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameStates_Ciphers_CipherCode",
                        column: x => x.CipherCode,
                        principalTable: "Ciphers",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameStates_Teams_TeamCode",
                        column: x => x.TeamCode,
                        principalTable: "Teams",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_CipherCode",
                table: "GameStates",
                column: "CipherCode");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_TeamCode",
                table: "GameStates",
                column: "TeamCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "Ciphers");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
