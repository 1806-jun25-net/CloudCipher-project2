using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RestaurantAPI.Data
{
    public partial class Project2DBContext : DbContext
    {
        public Project2DBContext()
        {
        }

        public Project2DBContext(DbContextOptions<Project2DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("Users", "RestaurantSite");

                entity.Property(e => e.Username)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();
            });
        }
    }
}
