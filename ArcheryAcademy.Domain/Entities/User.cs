using System;
using System.Collections.Generic;

namespace ArcheryAcademy.Domain.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public string Status { get; set; } = "A";

    public DateTime? CreatedAt { get; set; }

    public Guid? InstructorId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual User? Instructor { get; set; }

    public virtual ICollection<User> InverseInstructor { get; set; } = new List<User>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<UserPlan> UserPlans { get; set; } = new List<UserPlan>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
