using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SeniorVlogger.DataAccess.Migrations
{
    public partial class AddBlogPostModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Slug = table.Column<string>(maxLength: 50, nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Category = table.Column<string>(maxLength: 50, nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Mailed = table.Column<bool>(nullable: false),
                    Scratch = table.Column<bool>(nullable: false),
                    NextId = table.Column<int>(nullable: true),
                    PreviousId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPosts_BlogPosts_NextId",
                        column: x => x.NextId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogPosts_BlogPosts_PreviousId",
                        column: x => x.PreviousId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_NextId",
                table: "BlogPosts",
                column: "NextId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PreviousId",
                table: "BlogPosts",
                column: "PreviousId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPosts");
        }
    }
}
