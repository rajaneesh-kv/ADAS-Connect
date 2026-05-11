using CvTracker.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CvTracker.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateStatusHistory> CandidateStatusHistories { get; set; }
        public DbSet<SchoolRegistration> SchoolRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<CandidateStatusHistory>()
                .HasOne(x => x.Candidate)
                .WithMany(x => x.StatusHistory)
                .HasForeignKey(x => x.CandidateId);

            modelBuilder.Entity<SchoolRegistration>()
                .Property(x => x.DateOfBirth)
                .HasColumnType("date");

            modelBuilder.Entity<SchoolRegistration>()
                .Property(x => x.PhotoPath)
                .IsRequired(false);
        }
    }
}
