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
        public virtual DbSet<QueryRestaurantJunction> QueryRestaurantJunction { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<RestaurantKeywordJunction> RestaurantKeywordJunction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //moved connection string to user secrets
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

                entity.Property(e => e.RestaurantId)
                    .HasColumnName("RestaurantID")
                    .HasMaxLength(128);

                entity.Property(e => e.Username).HasMaxLength(128);

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Blacklist)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Blacklist__Resta__17F790F9");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Blacklist)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Blacklist__Usern__18EBB532");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => new { e.RestaurantId, e.Username });

                entity.ToTable("Favorite", "RestaurantSite");

                entity.Property(e => e.RestaurantId)
                    .HasColumnName("RestaurantID")
                    .HasMaxLength(128);

                entity.Property(e => e.Username).HasMaxLength(128);

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Favorite)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Favorite__Restau__14270015");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Favorite)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Favorite__Userna__151B244E");
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

                entity.Property(e => e.Lat).HasMaxLength(128);

                entity.Property(e => e.Lon).HasMaxLength(128);

                entity.Property(e => e.QueryTime).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Query)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Query__Username__114A936A");
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
                    .HasConstraintName("FK__QueryKeyw__Query__1BC821DD");

                entity.HasOne(d => d.WordNavigation)
                    .WithMany(p => p.QueryKeywordJunction)
                    .HasForeignKey(d => d.Word)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QueryKeywo__Word__1CBC4616");
            });

            modelBuilder.Entity<QueryRestaurantJunction>(entity =>
            {
                entity.HasKey(e => new { e.QueryId, e.RestaurantId });

                entity.ToTable("QueryRestaurantJunction", "RestaurantSite");

                entity.Property(e => e.QueryId).HasColumnName("QueryID");

                entity.Property(e => e.RestaurantId)
                    .HasColumnName("RestaurantID")
                    .HasMaxLength(128);

                entity.HasOne(d => d.Query)
                    .WithMany(p => p.QueryRestaurantJunction)
                    .HasForeignKey(d => d.QueryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QueryRest__Query__2645B050");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.QueryRestaurantJunction)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QueryRest__Resta__2739D489");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("Restaurant", "RestaurantSite");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(128);

                entity.Property(e => e.Hours).HasMaxLength(256);

                entity.Property(e => e.Lat)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Lon)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Owner).HasMaxLength(128);

                entity.Property(e => e.PriceLevel).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Rating).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Restaurant)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("FK__Restauran__Owner__0E6E26BF");
            });

            modelBuilder.Entity<RestaurantKeywordJunction>(entity =>
            {
                entity.HasKey(e => new { e.RestaurantId, e.Word });

                entity.ToTable("RestaurantKeywordJunction", "RestaurantSite");

                entity.Property(e => e.RestaurantId)
                    .HasColumnName("RestaurantID")
                    .HasMaxLength(128);

                entity.Property(e => e.Word).HasMaxLength(128);

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.RestaurantKeywordJunction)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Restauran__Resta__22751F6C");

                entity.HasOne(d => d.WordNavigation)
                    .WithMany(p => p.RestaurantKeywordJunction)
                    .HasForeignKey(d => d.Word)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Restaurant__Word__236943A5");
            });
        }
    }
}
