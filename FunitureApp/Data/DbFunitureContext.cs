using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunitureApp.Models;
using Microsoft.EntityFrameworkCore;
namespace FunitureApp.Data
{
    public class DbFunitureContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
        public DbSet<UserOrderItem> UserOrderItems { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<Comment> Comments { get; set; }

        private const string connectionString = "server=localhost;database=funitureapp;user=root;password=root;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //map ten bang voi ten class
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "funitureapp");
            });


            modelBuilder.Entity<UserAddress>(entity =>
                entity.ToTable("user_address", "funitureapp")

            );
            modelBuilder.Entity<Product>(entity =>
                entity.ToTable("product", "funitureapp")

            );
            modelBuilder.Entity<ProductAttribute>(entity =>
                entity.ToTable("product_attribute", "funitureapp")

            );
            modelBuilder.Entity<Category>(entity =>
                  entity.ToTable("category", "funitureapp")
            );
            modelBuilder.Entity<Favorites>(entity =>
                entity.ToTable("favorite", "funitureapp")

            );
            modelBuilder.Entity<UserOrder>(entity =>
               entity.ToTable("user_order", "funitureapp")

           );
            modelBuilder.Entity<Comment>(entity =>
                   entity.ToTable("comment", "funitureapp")

               );
            modelBuilder.Entity<UserOrderItem>(entity =>
                  entity.ToTable("user_order_items", "funitureapp")

              );

        }

    }
}
