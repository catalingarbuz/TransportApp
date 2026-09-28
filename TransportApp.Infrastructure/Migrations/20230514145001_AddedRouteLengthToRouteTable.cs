using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportApp.Infrastructure.Migrations
{
    public partial class AddedRouteLengthToRouteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RouteLength",
                table: "Route",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteLength",
                table: "Route");
        }
    }
}

