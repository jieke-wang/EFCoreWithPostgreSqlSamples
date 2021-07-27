using System;
using System.Diagnostics.CodeAnalysis;

using Entities;

using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore
{
    public class PostgreDbContext : DbContext
    {
        public PostgreDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity => 
            {
                entity.Property(p => p.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(p => p.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(20);

                entity.Property(p => p.Age)
                    .IsRequired();

                entity.Property(p => p.RegisterDate);

                entity.Property(p => p.RegisteredUser)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(p => p.Balance)
                    .IsRequired();
            });
        }
    }
}
