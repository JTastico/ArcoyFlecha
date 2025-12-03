namespace ArcheryAcademy.Application.DTOs.SettingsDto;

public class AppSettingDto
{
    public Guid Id { get; set; }
    public string AcademyName { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
    public string Language { get; set; }
    public string Timezone { get; set; }
    
    public bool EmailReminders { get; set; }
    public bool SmsReminders { get; set; }
    public int ReminderHours { get; set; }
    public int SecondReminderHours { get; set; }
    
    public bool AutoWaitlist { get; set; }
    public bool MonthlyRecalc { get; set; }
    public int DefaultCapacity { get; set; }
    public int MinBookingHours { get; set; }
    public int MaxBookingDays { get; set; }
    public int CancellationHours { get; set; }
}