using DevSchoolCache;
using Microsoft.EntityFrameworkCore;

namespace Model;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Position)
                    .HasConversion<string>();

                entity.HasOne(e => e.School)
                    .WithMany()
                    .HasForeignKey("SchoolId");
            });

        modelBuilder.Entity<School>(entity => { entity.HasKey(e => e.Id); });
    }
}