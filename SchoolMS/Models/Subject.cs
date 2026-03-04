using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SchoolMS.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public int ClassId { get; set; }

        [ValidateNever]  // ← Fix 1
        public Class Class { get; set; } = null!;

        [ValidateNever]  // ← Fix 2
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

        [ValidateNever]  // ← Fix 3
        public ICollection<StudentMark> StudentMarks { get; set; } = new List<StudentMark>();
    }
}