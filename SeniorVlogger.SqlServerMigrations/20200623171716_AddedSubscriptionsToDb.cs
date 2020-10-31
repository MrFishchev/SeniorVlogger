using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SeniorVlogger.SqlServerMigrations
{
    public partial class AddedSubscriptionsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: false),
                    IsSubscribed = table.Column<bool>(nullable: false),
                    SubscribeDate = table.Column<DateTime>(nullable: false),
                    UnsubscribeDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
