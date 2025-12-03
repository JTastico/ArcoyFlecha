using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcheryAcademy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAppSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AcademyName = table.Column<string>(type: "text", nullable: false),
                    ContactEmail = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    Timezone = table.Column<string>(type: "text", nullable: false),
                    EmailReminders = table.Column<bool>(type: "boolean", nullable: false),
                    SmsReminders = table.Column<bool>(type: "boolean", nullable: false),
                    ReminderHours = table.Column<int>(type: "integer", nullable: false),
                    SecondReminderHours = table.Column<int>(type: "integer", nullable: false),
                    AutoWaitlist = table.Column<bool>(type: "boolean", nullable: false),
                    MonthlyRecalc = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultCapacity = table.Column<int>(type: "integer", nullable: false),
                    MinBookingHours = table.Column<int>(type: "integer", nullable: false),
                    MaxBookingDays = table.Column<int>(type: "integer", nullable: false),
                    CancellationHours = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_settings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_settings");
        }
    }
}
