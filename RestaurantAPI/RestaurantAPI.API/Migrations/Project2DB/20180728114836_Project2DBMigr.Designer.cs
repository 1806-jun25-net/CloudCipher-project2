﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestaurantAPI.Data;

namespace RestaurantAPI.API.Migrations.Project2DB
{
    [DbContext(typeof(Project2DBContext))]
    [Migration("20180728114836_Project2DBMigr")]
    partial class Project2DBMigr
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RestaurantAPI.Data.AppUser", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(128);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int>("UserType");

                    b.HasKey("Username");

                    b.ToTable("AppUser","RestaurantSite");
                });

            modelBuilder.Entity("RestaurantAPI.Data.Blacklist", b =>
                {
                    b.Property<int>("RestaurantId")
                        .HasColumnName("RestaurantID");

                    b.Property<string>("Username")
                        .HasMaxLength(128);

                    b.HasKey("RestaurantId", "Username");

                    b.HasIndex("Username");

                    b.ToTable("Blacklist","RestaurantSite");
                });

            modelBuilder.Entity("RestaurantAPI.Data.Favorite", b =>
                {
                    b.Property<int>("RestaurantId")
                        .HasColumnName("RestaurantID");

                    b.Property<string>("Username")
                        .HasMaxLength(128);

                    b.HasKey("RestaurantId", "Username");

                    b.HasIndex("Username");

                    b.ToTable("Favorite","RestaurantSite");
                });

            modelBuilder.Entity("RestaurantAPI.Data.Keyword", b =>
                {
                    b.Property<string>("Word")
                        .HasMaxLength(128);

                    b.HasKey("Word");

                    b.ToTable("Keyword","RestaurantSite");
                });

            modelBuilder.Entity("RestaurantAPI.Data.Query", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Location")
                        .HasMaxLength(128);

                    b.Property<string>("Location2")
                        .HasMaxLength(128);

                    b.Property<DateTime>("QueryTime")
                        .HasColumnType("datetime");

                    b.Property<int?>("Radius");

                    b.Property<DateTime?>("ReservationTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("Username");

                    b.ToTable("Query","RestaurantSite");
                });

            modelBuilder.Entity("RestaurantAPI.Data.QueryKeywordJunction", b =>
                {
                    b.Property<int>("QueryId")
                        .HasColumnName("QueryID");

                    b.Property<string>("Word")
                        .HasMaxLength(128);

                    b.HasKey("QueryId", "Word");

                    b.HasIndex("Word");

                    b.ToTable("QueryKeywordJunction","RestaurantSite");
                });

            modelBuilder.Entity("RestaurantAPI.Data.Restaurant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hours")
                        .HasMaxLength(128);

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Location2")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Owner")
                        .HasMaxLength(128);

                    b.Property<string>("Phone")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("Owner");

                    b.ToTable("Restaurant","RestaurantSite");
                });

            modelBuilder.Entity("RestaurantAPI.Data.RestaurantKeywordJunction", b =>
                {
                    b.Property<int>("RestaurantId")
                        .HasColumnName("RestaurantID");

                    b.Property<string>("Word")
                        .HasMaxLength(128);

                    b.HasKey("RestaurantId", "Word");

                    b.HasIndex("Word");

                    b.ToTable("RestaurantKeywordJunction","RestaurantSite");
                });

            modelBuilder.Entity("RestaurantAPI.Data.Blacklist", b =>
                {
                    b.HasOne("RestaurantAPI.Data.Restaurant", "Restaurant")
                        .WithMany("Blacklist")
                        .HasForeignKey("RestaurantId")
                        .HasConstraintName("FK__Blacklist__Resta__02FC7413");

                    b.HasOne("RestaurantAPI.Data.AppUser", "UsernameNavigation")
                        .WithMany("Blacklist")
                        .HasForeignKey("Username")
                        .HasConstraintName("FK__Blacklist__Usern__03F0984C");
                });

            modelBuilder.Entity("RestaurantAPI.Data.Favorite", b =>
                {
                    b.HasOne("RestaurantAPI.Data.Restaurant", "Restaurant")
                        .WithMany("Favorite")
                        .HasForeignKey("RestaurantId")
                        .HasConstraintName("FK__Favorite__Restau__7F2BE32F");

                    b.HasOne("RestaurantAPI.Data.AppUser", "UsernameNavigation")
                        .WithMany("Favorite")
                        .HasForeignKey("Username")
                        .HasConstraintName("FK__Favorite__Userna__00200768");
                });

            modelBuilder.Entity("RestaurantAPI.Data.Query", b =>
                {
                    b.HasOne("RestaurantAPI.Data.AppUser", "UsernameNavigation")
                        .WithMany("Query")
                        .HasForeignKey("Username")
                        .HasConstraintName("FK__Query__Username__66603565");
                });

            modelBuilder.Entity("RestaurantAPI.Data.QueryKeywordJunction", b =>
                {
                    b.HasOne("RestaurantAPI.Data.Query", "Query")
                        .WithMany("QueryKeywordJunction")
                        .HasForeignKey("QueryId")
                        .HasConstraintName("FK__QueryKeyw__Query__06CD04F7");

                    b.HasOne("RestaurantAPI.Data.Keyword", "WordNavigation")
                        .WithMany("QueryKeywordJunction")
                        .HasForeignKey("Word")
                        .HasConstraintName("FK__QueryKeywo__Word__07C12930");
                });

            modelBuilder.Entity("RestaurantAPI.Data.Restaurant", b =>
                {
                    b.HasOne("RestaurantAPI.Data.AppUser", "OwnerNavigation")
                        .WithMany("Restaurant")
                        .HasForeignKey("Owner")
                        .HasConstraintName("FK__Restauran__Owner__6383C8BA");
                });

            modelBuilder.Entity("RestaurantAPI.Data.RestaurantKeywordJunction", b =>
                {
                    b.HasOne("RestaurantAPI.Data.Restaurant", "Restaurant")
                        .WithMany("RestaurantKeywordJunction")
                        .HasForeignKey("RestaurantId")
                        .HasConstraintName("FK__Restauran__Resta__0A9D95DB");

                    b.HasOne("RestaurantAPI.Data.Keyword", "WordNavigation")
                        .WithMany("RestaurantKeywordJunction")
                        .HasForeignKey("Word")
                        .HasConstraintName("FK__Restaurant__Word__0B91BA14");
                });
#pragma warning restore 612, 618
        }
    }
}
