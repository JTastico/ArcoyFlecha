namespace ArcheryAcademy.Application.DTOs.ReportDto;

public class AdminDashboardDto
{
    public BookingStatsDto Bookings { get; set; } = new();
    public List<ClassOccupancyDto> TopClasses { get; set; } = new();
    public PlanStatsDto Plans { get; set; } = new();
    public UserStatsDto Users { get; set; } = new();
}

// Sub-DTOs para organizar la info
public class BookingStatsDto
{
    public int TotalToday { get; set; }
    public int TotalThisWeek { get; set; } // En el repo se llama TotalRange
    
    // Desglosados del Diccionario
    public int Pending { get; set; }   
    public int Confirmed { get; set; } 
    public int Cancelled { get; set; } 
    public int Completed { get; set; } 
}

public class ClassOccupancyDto
{
    public string ScheduleInfo { get; set; } = string.Empty; // Ej: "10/10 10:00 - Juan"
    public int MaxCapacity { get; set; }
    public int CurrentBookings { get; set; }
    public double OccupancyPercentage { get; set; } // Ej: 85.5
}

public class PlanStatsDto
{
    public int TotalActive { get; set; }
    public int TotalExpired { get; set; }
    // Clave: Nombre del Plan (Ej: "Básico"), Valor: Cantidad
    public Dictionary<string, int> ByType { get; set; } = new(); 
}

public class UserStatsDto
{
    public int TotalUsers { get; set; }
    public int NewUsersThisMonth { get; set; }
    public int ActiveStudents { get; set; } // Reservaron en los últimos 30 días
}