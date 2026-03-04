using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SchoolMS.Models
{
    public class ClassFee
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        [ValidateNever]
        public Class Class { get; set; } = null!;
        [Required, MaxLength(100)]
        public string FeeType { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
        [MaxLength(300)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class TeacherSubject
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        [ValidateNever]
        public Teacher Teacher { get; set; } = null!;
        public int SubjectId { get; set; }
        [ValidateNever]
        public Subject Subject { get; set; } = null!;
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }

    public class Expense
    {
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime ExpenseDate { get; set; }
        [MaxLength(100)]
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class StudentMark
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        [ValidateNever]
        public Student Student { get; set; } = null!;
        public int SubjectId { get; set; }
        [ValidateNever]
        public Subject Subject { get; set; } = null!;
        [Range(0, 100)]
        public decimal ObtainedMarks { get; set; }
        [Range(0, 100)]
        public decimal TotalMarks { get; set; }
        [MaxLength(100)]
        public string? ExamType { get; set; }
        public DateTime ExamDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class TeacherAttendance
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        [ValidateNever]
        public Teacher Teacher { get; set; } = null!;
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
        [MaxLength(200)]
        public string? Remarks { get; set; }
    }

    public class StudentAttendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        [ValidateNever]
        public Student Student { get; set; } = null!;
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
        [MaxLength(200)]
        public string? Remarks { get; set; }
        public int? TakenByTeacherId { get; set; }
        [ValidateNever]
        public Teacher? TakenByTeacher { get; set; }
    }
}