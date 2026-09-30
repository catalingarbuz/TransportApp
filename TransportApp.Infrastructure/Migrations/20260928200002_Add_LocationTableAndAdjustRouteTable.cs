using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportApp.Infrastructure.Migrations
{
    public partial class Add_LocationTableAndAdjustRouteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "RouteLength",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "RouteName",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "ArrivalPlace",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "DeparturePlace",
                table: "Booking");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                table: "Route",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureTime",
                table: "Route",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "FinalLocationId",
                table: "Route",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StartingLocationId",
                table: "Route",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    Adress = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Route_Location_FinalLocationId",
                table: "Route",
                column: "FinalLocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Route_Location_StartingLocationId",
                table: "Route",
                column: "StartingLocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Route_Location_FinalLocationId",
                table: "Route");

            migrationBuilder.DropForeignKey(
                name: "FK_Route_Location_StartingLocationId",
                table: "Route");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Route_FinalLocationId",
                table: "Route");

            migrationBuilder.DropIndex(
                name: "IX_Route_StartingLocationId",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "DepartureTime",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "FinalLocationId",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "StartingLocationId",
                table: "Route");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Route",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RouteLength",
                table: "Route",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteName",
                table: "Route",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArrivalPlace",
                table: "Booking",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeparturePlace",
                table: "Booking",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }
    }
}
