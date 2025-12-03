using System.ComponentModel.DataAnnotations;

namespace ArcheryAcademy.Domain.Entities;

public class AppSetting
{
    public Guid Id { get; set; }

    // General
    public string AcademyName { get; set; } = "Mi Academia";
    public string ContactEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Language { get; set; } = "es";
    public string Timezone { get; set; } = "europe-madrid";

    // Notificaciones
    public bool EmailReminders { get; set; } = true;
    public bool SmsReminders { get; set; } = false;
    public int ReminderHours { get; set; } = 24;
    public int SecondReminderHours { get; set; } = 1;

    // Horarios
    public bool AutoWaitlist { get; set; } = true;
    public bool MonthlyRecalc { get; set; } = true;
    public int DefaultCapacity { get; set; } = 4;
    public int MinBookingHours { get; set; } = 24;
    public int MaxBookingDays { get; set; } = 30;
    public int CancellationHours { get; set; } = 24;
}