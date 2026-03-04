using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SchoolMS.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        public string? PhotoPath { get; set; }
        public string? UserId { get; set; }

        [ValidateNever]
        public ApplicationUser? User { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

        [ValidateNever]
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

        [ValidateNever]
        public ICollection<TeacherAttendance> Attendances { get; set; } = new List<TeacherAttendance>();
    }
}