using System;
using System.Collections.Generic;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InventoryAPI.Data
{
    public partial class InventoryContext : DbContext
    {
        public InventoryContext()
        {
        }

        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<DefectiveProduct> DefectiveProducts { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Sale> Sales { get; set; } = null!;
        public virtual DbSet<SaleItem> SaleItems { get; set; } = null!;
        public virtual DbSet<Sensor> Sensors { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<StoreProduct> StoreProducts { get; set; } = null!;
        public virtual DbSet<Subscription> Subscriptions { get; set; } = null!;
        public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<SupplierRequest> SupplierRequests { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = DESKTOP-0D2M39V; Database=Inventory;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            modelBuilder.Entity<DefectiveProduct>(entity =>
            {
                entity.ToTable("Defective_Products");

                entity.Property(e => e.DefectiveProductId).HasColumnName("defective_product_id");

                entity.Property(e => e.DateDetected).HasColumnType("date");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Reason).HasMaxLength(255);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DefectiveProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Defective__produ__52593CB8");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Position).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__Employee__store___3C69FB99");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductName).HasMaxLength(100);

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Product__categor__4316F928");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__Product__supplie__440B1D61");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sale");

                entity.Property(e => e.SaleId).HasColumnName("sale_id");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");

                entity.Property(e => e.SaleDate).HasColumnType("datetime");

                entity.Property(e => e.StoreId).HasColumnName("Store_ID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Sale__Employee_I__46E78A0C");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__Sale__Store_ID__47DBAE45");
            });

            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.ToTable("Sale_Items");

                entity.Property(e => e.SaleItemId).HasColumnName("sale_item_id");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.SaleId).HasColumnName("Sale_ID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SaleItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Sale_Item__Produ__4BAC3F29");

                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleItems)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK__Sale_Item__Sale___4AB81AF0");
            });

            modelBuilder.Entity<Sensor>(entity =>
            {
                entity.ToTable("Sensor");

                entity.Property(e => e.SensorId).HasColumnName("sensor_id");

                entity.Property(e => e.StoreId).HasColumnName("Store_ID");

                entity.Property(e => e.Temperature).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Sensors)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__Sensor__Store_ID__5DCAEF64");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName).HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Store__user_id__398D8EEE");
            });

            modelBuilder.Entity<StoreProduct>(entity =>
            {
                entity.ToTable("Store_Products");

                entity.Property(e => e.StoreProductId).HasColumnName("store_product_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.StoreProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Store_Pro__produ__4F7CD00D");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreProducts)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__Store_Pro__store__4E88ABD4");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.ToTable("Subscription");

                entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.SubscriptionStatus).HasMaxLength(50);

                entity.Property(e => e.SubscriptionTypeId).HasColumnName("SubscriptionType_ID");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.SubscriptionType)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.SubscriptionTypeId)
                    .HasConstraintName("FK__Subscript__Subsc__59FA5E80");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Subscript__user___5AEE82B9");
            });

            modelBuilder.Entity<SubscriptionType>(entity =>
            {
                entity.ToTable("SubscriptionType");

                entity.Property(e => e.SubscriptionTypeId).HasColumnName("subscription_type_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            });

            modelBuilder.Entity<SupplierRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK__Supplier__18D3B90F48696F5B");

                entity.ToTable("SupplierRequest");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.RequestDate).HasColumnType("datetime");

                entity.Property(e => e.RequestStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StoreProductId).HasColumnName("store_product_id");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.StoreProduct)
                    .WithMany(p => p.SupplierRequests)
                    .HasForeignKey(d => d.StoreProductId)
                    .HasConstraintName("FK__SupplierR__store__5535A963");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
