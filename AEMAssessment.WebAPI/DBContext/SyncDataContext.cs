using AEMAssessment.WebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AEMAssessment.WebAPI.DBContext
{
    public class SyncDataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public SyncDataContext(DbContextOptions<SyncDataContext> options) : base(options) { }

        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Well> Wells { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mylocodb;Database=AssessmentBE;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.UniqueName).IsRequired();
                entity.Property(e => e.Latitude).IsRequired();
                entity.Property(e => e.Longitude).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.HasMany(e => e.Wells)
                      .WithOne(w => w.Platform)
                      .HasForeignKey(w => w.PlatformId);
            });

            modelBuilder.Entity<Well>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.PlatformId).IsRequired();
                entity.Property(e => e.UniqueName).IsRequired();
                entity.Property(e => e.Latitude).IsRequired();
                entity.Property(e => e.Longitude).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
            });
        }
    }
}
