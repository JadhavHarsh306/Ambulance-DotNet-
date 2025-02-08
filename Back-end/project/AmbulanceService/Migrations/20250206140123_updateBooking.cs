using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmbulanceService.Migrations
{
    public partial class updateBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_ambulances_AmbulanceId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_users_UserId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_guestUsers_bookings_BookingId",
                table: "guestUsers");

            migrationBuilder.DropIndex(
                name: "IX_guestUsers_BookingId",
                table: "guestUsers");

            migrationBuilder.DropIndex(
                name: "IX_bookings_AmbulanceId",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_UserId",
                table: "bookings");

            migrationBuilder.AlterColumn<int>(
                name: "AmbulanceId",
                table: "bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_guestUsers_BookingId",
                table: "guestUsers",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_guestUsers_bookings_BookingId",
                table: "guestUsers",
                column: "BookingId",
                principalTable: "bookings",
                principalColumn: "Bid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_guestUsers_bookings_BookingId",
                table: "guestUsers");

            migrationBuilder.DropIndex(
                name: "IX_guestUsers_BookingId",
                table: "guestUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AmbulanceId",
                table: "bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_guestUsers_BookingId",
                table: "guestUsers",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_AmbulanceId",
                table: "bookings",
                column: "AmbulanceId");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_UserId",
                table: "bookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_ambulances_AmbulanceId",
                table: "bookings",
                column: "AmbulanceId",
                principalTable: "ambulances",
                principalColumn: "AmbulanceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_users_UserId",
                table: "bookings",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_guestUsers_bookings_BookingId",
                table: "guestUsers",
                column: "BookingId",
                principalTable: "bookings",
                principalColumn: "Bid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
