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

        public virtual DbSet<AppUser> AppUser { get; set; }
        public virtual DbSet<Blacklist> Blacklist { get; set; }
        public virtual DbSet<Favorite> Favorite { get; set; }
        public virtual DbSet<Keyword> Keyword { get; set; }
        public virtual DbSet<Query> Query { get; set; }
        public virtual DbSet<QueryKeywordJunction> QueryKeywordJunction { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<RestaurantKeywordJunction> RestaurantKeywordJunction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("AppUser", "RestaurantSite");

                entity.Property(e => e.Username)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Blacklist>(entity =>
            {
                entity.HasKey(e => new { e.RestaurantId, e.Username });

                entity.ToTable("Blacklist", "RestaurantSite");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.Username).HasMaxLength(128);

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Blacklist)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Blacklist__Resta__02FC7413");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Blacklist)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Blacklist__Usern__03F0984C");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => new { e.RestaurantId, e.Username });

                entity.ToTable("Favorite", "RestaurantSite");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.Username).HasMaxLength(128);

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Favorite)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Favorite__Restau__7F2BE32F");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Favorite)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Favorite__Userna__00200768");
            });

            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.HasKey(e => e.Word);

                entity.ToTable("Keyword", "RestaurantSite");

                entity.Property(e => e.Word)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Query>(entity =>
            {
                entity.ToTable("Query", "RestaurantSite");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Location).HasMaxLength(128);

                entity.Property(e => e.Location2).HasMaxLength(128);

                entity.Property(e => e.QueryTime).HasColumnType("datetime");

                entity.Property(e => e.ReservationTime).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Query)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Query__Username__66603565");
            });

            modelBuilder.Entity<QueryKeywordJunction>(entity =>
            {
                entity.HasKey(e => new { e.QueryId, e.Word });

                entity.ToTable("QueryKeywordJunction", "RestaurantSite");

                entity.Property(e => e.QueryId).HasColumnName("QueryID");

                entity.Property(e => e.Word).HasMaxLength(128);

                entity.HasOne(d => d.Query)
                    .WithMany(p => p.QueryKeywordJunction)
                    .HasForeignKey(d => d.QueryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QueryKeyw__Query__06CD04F7");

                entity.HasOne(d => d.WordNavigation)
                    .WithMany(p => p.QueryKeywordJunction)
                    .HasForeignKey(d => d.Word)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QueryKeywo__Word__07C12930");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("Restaurant", "RestaurantSite");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Hours).HasMaxLength(128);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Location2).HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Owner).HasMaxLength(128);

                entity.Property(e => e.Phone).HasMaxLength(128);

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Restaurant)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("FK__Restauran__Owner__6383C8BA");
            });

            modelBuilder.Entity<RestaurantKeywordJunction>(entity =>
            {
                entity.HasKey(e => new { e.RestaurantId, e.Word });

                entity.ToTable("RestaurantKeywordJunction", "RestaurantSite");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.Word).HasMaxLength(128);

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.RestaurantKeywordJunction)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Restauran__Resta__0A9D95DB");

                entity.HasOne(d => d.WordNavigation)
                    .WithMany(p => p.RestaurantKeywordJunction)
                    .HasForeignKey(d => d.Word)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Restaurant__Word__0B91BA14");
            });
        }
    }
}
