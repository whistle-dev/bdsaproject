using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitInsight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Repos",
                columns: table => new
                {
                    Path = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LatestCommitSha = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repos", x => x.Path);
                });

            migrationBuilder.CreateTable(
                name: "Commits",
                columns: table => new
                {
                    Sha = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepoPath = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commits", x => x.Sha);
                    table.ForeignKey(
                        name: "FK_Commits_Repos_RepoPath",
                        column: x => x.RepoPath,
                        principalTable: "Repos",
                        principalColumn: "Path",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commits_RepoPath",
                table: "Commits",
                column: "RepoPath");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_Sha",
                table: "Commits",
                column: "Sha",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repos_Path",
                table: "Repos",
                column: "Path",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commits");

            migrationBuilder.DropTable(
                name: "Repos");
        }
    }
}
