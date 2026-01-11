using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsRepeatedGuest = table.Column<bool>(type: "bit", nullable: false),
                    TotalPreviousCancellations = table.Column<int>(type: "int", nullable: false),
                    TotalPreviousBookings = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    MealPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PricePerPerson = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.MealPlanId);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    RoomTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaxOccupancy = table.Column<int>(type: "int", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.RoomTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    RoomTypeId = table.Column<int>(type: "int", nullable: false),
                    MealPlanId = table.Column<int>(type: "int", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoOfWeekendNights = table.Column<int>(type: "int", nullable: false),
                    NoOfWeekNights = table.Column<int>(type: "int", nullable: false),
                    NoOfAdults = table.Column<int>(type: "int", nullable: false),
                    NoOfChildren = table.Column<int>(type: "int", nullable: false),
                    RequiredCarParking = table.Column<bool>(type: "bit", nullable: false),
                    NoOfSpecialRequests = table.Column<int>(type: "int", nullable: false),
                    AvgPricePerRoom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LeadTime = table.Column<int>(type: "int", nullable: false),
                    MarketSegmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_MealPlans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "MealPlans",
                        principalColumn: "MealPlanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bookings_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "RoomTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingHistories",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ChangedByUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChangeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChangeDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OldStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NewStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_BookingHistories_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreatedDate", "Email", "FirstName", "IsRepeatedGuest", "LastName", "PhoneNumber", "TotalPreviousBookings", "TotalPreviousCancellations" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", "John", false, "Doe", "+40712345678", 0, 0 },
                    { 2, new DateTime(2025, 6, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), "jane.smith@example.com", "Jane", true, "Smith", "+40723456789", 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "MealPlans",
                columns: new[] { "MealPlanId", "Description", "IsActive", "PlanName", "PricePerPerson" },
                values: new object[,]
                {
                    { 1, "No meals included", true, "Not Selected", 0.00m },
                    { 2, "Breakfast included", true, "Meal Plan 1", 15.00m },
                    { 3, "Half Board (Breakfast + Dinner)", true, "Meal Plan 2", 35.00m },
                    { 4, "Full Board (All meals)", true, "Meal Plan 3", 50.00m }
                });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "RoomTypeId", "BasePrice", "Description", "IsActive", "MaxOccupancy", "RoomTypeName" },
                values: new object[,]
                {
                    { 1, 50.00m, "Standard Single Room", true, 1, "Room Type 1" },
                    { 2, 75.00m, "Standard Double Room", true, 2, "Room Type 2" },
                    { 3, 100.00m, "Deluxe Room", true, 2, "Room Type 3" },
                    { 4, 150.00m, "Family Room", true, 4, "Room Type 4" },
                    { 5, 200.00m, "Suite", true, 3, "Room Type 5" },
                    { 6, 300.00m, "Penthouse Suite", true, 4, "Room Type 6" },
                    { 7, 500.00m, "Presidential Suite", true, 6, "Room Type 7" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistories_BookingId",
                table: "BookingHistories",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingCode",
                table: "Bookings",
                column: "BookingCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MealPlanId",
                table: "Bookings",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomTypeId",
                table: "Bookings",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingHistories");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "MealPlans");

            migrationBuilder.DropTable(
                name: "RoomTypes");
        }
    }
}
