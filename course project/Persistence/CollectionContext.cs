using course_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace course_project.Persistence
{
    public class CollectionContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public CollectionContext()
        {
        }

        public CollectionContext(DbContextOptions<CollectionContext> options)
            : base(options)
        { 
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Content> Contents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshToken>()
                .HasKey(t => new { t.UserId, t.Token });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CollectionName)
                    .HasMaxLength(56)
                    .HasColumnName("CollectionName")
                    .IsRequired();
            });

            modelBuilder.Entity<Content>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.NameOfContent)
                    .HasMaxLength(56)
                    .HasColumnName("NameOfContent")
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(1024)
                    .HasColumnName("Description")
                    .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(56)
                    .HasColumnName ("Name")
                    .IsRequired();
                entity.Property(e => e.Surname)
                    .HasMaxLength(56)
                    .HasColumnName("Surname")
                    .IsRequired();
            });
        }
    }
}
