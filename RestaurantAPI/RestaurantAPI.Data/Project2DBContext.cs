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
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:kirk-1806.database.windows.net,1433;Initial Catalog=Project2DB;Persist Security Info=False;User ID=bjkirk;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
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
