using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SchoolMS.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? RollNumber { get; set; }

        [Required]
        public int ClassId { get; set; }

        [ValidateNever]
        public Class Class { get; set; } = null!;

        [MaxLength(200)]
        public string? GuardianName { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? PhotoPath { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.UtcNow;

        [ValidateNever]  // ← Add this
        public ICollection<StudentMark> Marks { get; set; } = new List<StudentMark>();

        [ValidateNever]  // ← Add this
        public ICollection<StudentAttendance> Attendances { get; set; } = new List<StudentAttendance>();
    }
}