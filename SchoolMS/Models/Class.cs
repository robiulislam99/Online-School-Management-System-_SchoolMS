using System.ComponentModel.DataAnnotations;

namespace SchoolMS.Models
{
    public class Class
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public ICollection<ClassFee> ClassFees { get; set; } = new List<ClassFee>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}