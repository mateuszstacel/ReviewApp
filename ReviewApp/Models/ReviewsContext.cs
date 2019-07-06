using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ReviewApp.Models
{
    public partial class ReviewsContext : DbContext
    {
        public ReviewsContext()
        {
        }

        public ReviewsContext(DbContextOptions<ReviewsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PurchasedItems> PurchasedItems { get; set; }
        public virtual DbSet<SellersItems> SellersItems { get; set; }
        public virtual DbSet<UsersAccount> UsersAccount { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Reviews;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchasedItems>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BuyerId).HasColumnName("BuyerID");

                entity.Property(e => e.DateOfPurchased).HasColumnType("datetime");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.Ppemail)
                    .HasColumnName("PPEmail")
                    .HasMaxLength(255);

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<SellersItems>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Categories).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.ItemName).HasMaxLength(255);

                entity.Property(e => e.KeyWords).HasMaxLength(300);

                entity.Property(e => e.Ppfee).HasColumnName("PPFee");

                entity.Property(e => e.Price).HasMaxLength(255);

                entity.Property(e => e.SellerEmail).HasMaxLength(255);

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.StoreName).HasMaxLength(255);
            });

            modelBuilder.Entity<UsersAccount>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ComputerName).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(50);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Role).HasMaxLength(50);
            });
        }
    }
}
