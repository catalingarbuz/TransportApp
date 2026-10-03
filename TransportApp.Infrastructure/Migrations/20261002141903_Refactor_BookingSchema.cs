using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportApp.Infrastructure.Migrations
{
    public partial class Refactor_BookingSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Car_CarId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Driver_DriverId",
                table: "Booking");

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Car",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Booking",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CarId",
                table: "Booking",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Car_DriverId",
                table: "Car",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Car_CarId",
                table: "Booking",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Driver_DriverId",
                table: "Booking",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Driver_DriverId",
                table: "Car",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Car_CarId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Driver_DriverId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_Driver_DriverId",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Car_DriverId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Car");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Booking",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CarId",
                table: "Booking",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Car_CarId",
                table: "Booking",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Driver_DriverId",
                table: "Booking",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
