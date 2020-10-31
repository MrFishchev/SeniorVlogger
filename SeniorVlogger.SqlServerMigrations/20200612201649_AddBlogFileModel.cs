using Microsoft.EntityFrameworkCore.Migrations;

namespace SeniorVlogger.SqlServerMigrations
{
    public partial class AddBlogFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(maxLength: 100, nullable: false),
                    MimeType = table.Column<string>(maxLength: 30, nullable: true),
                    Encoding = table.Column<string>(maxLength: 30, nullable: true),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFiles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogFiles");
        }
    }
}
