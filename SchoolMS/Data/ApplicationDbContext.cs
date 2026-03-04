using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolMS.Models;

namespace SchoolMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<ClassFee> ClassFees { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<StudentMark> StudentMarks { get; set; }
        public DbSet<TeacherAttendance> TeacherAttendances { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TeacherSubject>()
                .HasIndex(ts => new { ts.TeacherId, ts.SubjectId })
                .IsUnique();

            builder.Entity<StudentAttendance>()
                .HasIndex(sa => new { sa.StudentId, sa.AttendanceDate })
                .IsUnique();

            builder.Entity<TeacherAttendance>()
                .HasIndex(ta => new { ta.TeacherId, ta.AttendanceDate })
                .IsUnique();

            builder.Entity<ClassFee>()
                .Property(f => f.Amount)
                .HasPrecision(18, 2);

            builder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            builder.Entity<StudentMark>()
                .Property(m => m.ObtainedMarks)
                .HasPrecision(5, 2);

            builder.Entity<StudentMark>()
                .Property(m => m.TotalMarks)
                .HasPrecision(5, 2);
        }
    }
}