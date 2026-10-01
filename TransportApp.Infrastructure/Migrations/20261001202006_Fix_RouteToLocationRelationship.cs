using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportApp.Infrastructure.Migrations
{
    public partial class Fix_RouteToLocationRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Route_FinalLocationId",
                table: "Route");

            migrationBuilder.DropIndex(
                name: "IX_Route_StartingLocationId",
                table: "Route");

            migrationBuilder.CreateIndex(
                name: "IX_Route_FinalLocationId",
                table: "Route",
                column: "FinalLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Route_StartingLocationId",
                table: "Route",
                column: "StartingLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Route_FinalLocationId",
                table: "Route");

            migrationBuilder.DropIndex(
                name: "IX_Route_StartingLocationId",
                table: "Route");

            migrationBuilder.CreateIndex(
                name: "IX_Route_FinalLocationId",
                table: "Route",
                column: "FinalLocationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Route_StartingLocationId",
                table: "Route",
                column: "StartingLocationId",
                unique: true);
        }
    }
}
