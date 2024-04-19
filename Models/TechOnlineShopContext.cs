using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineTechShop.Models;

public partial class TechOnlineShopContext : DbContext
{
    public TechOnlineShopContext()
    {
    }

    public TechOnlineShopContext(DbContextOptions<TechOnlineShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<OrderTable> OrderTables { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<ShoppingCartProduct> ShoppingCartProducts { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TechOnlineShop;Trusted_Connection=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2A1B1BAB20DC");

            entity.ToTable("Address");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Country).HasMaxLength(255);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Street).HasMaxLength(255);
        });

        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId).HasName("PK__Attribut__C189298A6792B814");

            entity.HasIndex(e => e.AttributeName, "UQ__Attribut__B0EBDF2F5055C81E").IsUnique();

            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.AttributeName).HasMaxLength(255);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B635BE58C");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PK__Manufact__357E5CA197F1D4AA");

            entity.ToTable("Manufacturer");

            entity.HasIndex(e => e.ManufacturerName, "UQ__Manufact__3B9CDE2E36DF0B97").IsUnique();

            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.ManufacturerName).HasMaxLength(255);
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => e.OrderProductId).HasName("PK__OrderPro__29B019E292032550");

            entity.Property(e => e.OrderProductId).HasColumnName("OrderProductID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderProd__Order__3D5E1FD2");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderProd__Produ__3E52440B");
        });

        modelBuilder.Entity<OrderTable>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderTab__C3905BAF51EC2AC0");

            entity.ToTable("OrderTable");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.OrderTables)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OrderTabl__UserI__32E0915F");

            entity.HasMany(d => d.Addresses).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderAddress",
                    r => r.HasOne<Address>().WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__OrderAddr__Addre__3A81B327"),
                    l => l.HasOne<OrderTable>().WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__OrderAddr__Order__398D8EEE"),
                    j =>
                    {
                        j.HasKey("OrderId", "AddressId").HasName("PK__OrderAdd__6301990EB38E2DB3");
                        j.ToTable("OrderAddress");
                        j.IndexerProperty<int>("OrderId").HasColumnName("OrderID");
                        j.IndexerProperty<int>("AddressId").HasColumnName("AddressID");
                    });
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED447C76CA");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductImage).HasMaxLength(255);
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.StockCount).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__35BCFE0A");

            entity.HasMany(d => d.Manufacturers).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductManufacturer",
                    r => r.HasOne<Manufacturer>().WithMany()
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductMa__Manuf__45F365D3"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductMa__Produ__44FF419A"),
                    j =>
                    {
                        j.HasKey("ProductId", "ManufacturerId").HasName("PK__ProductM__B75B232746617755");
                        j.ToTable("ProductManufacturer");
                        j.IndexerProperty<int>("ProductId").HasColumnName("ProductID");
                        j.IndexerProperty<int>("ManufacturerId").HasColumnName("ManufacturerID");
                    });
        });

        modelBuilder.Entity<ProductAttribute>(entity =>
        {
            entity.HasKey(e => e.ProductAttributesId).HasName("PK__ProductA__596DE210DF6ABBE1");

            entity.Property(e => e.ProductAttributesId).HasColumnName("ProductAttributesID");
            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Attribute).WithMany(p => p.ProductAttributes)
                .HasForeignKey(d => d.AttributeId)
                .HasConstraintName("FK__ProductAt__Attri__412EB0B6");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductAttributes)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductAt__Produ__4222D4EF");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79AEED7F23A8");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Review__ProductI__48CFD27E");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Review__UserID__49C3F6B7");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.ShoppingCartId).HasName("PK__Shopping__7A789A8499C9002A");

            entity.ToTable("ShoppingCart");

            entity.Property(e => e.ShoppingCartId).HasColumnName("ShoppingCartID");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ShoppingC__UserI__4CA06362");
        });

        modelBuilder.Entity<ShoppingCartProduct>(entity =>
        {
            entity.HasKey(e => e.ShoppingCartProductsId).HasName("PK__Shopping__FAA9F86B719819B8");

            entity.Property(e => e.ShoppingCartProductsId).HasColumnName("ShoppingCartProductsID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ShoppingCartId).HasColumnName("ShoppingCartID");

            entity.HasOne(d => d.Product).WithMany(p => p.ShoppingCartProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ShoppingC__Produ__5165187F");

            entity.HasOne(d => d.ShoppingCart).WithMany(p => p.ShoppingCartProducts)
                .HasForeignKey(d => d.ShoppingCartId)
                .HasConstraintName("FK__ShoppingC__Shopp__5070F446");
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserTabl__1788CCAC53869D80");

            entity.ToTable("UserTable");

            entity.HasIndex(e => e.Username, "UQ__UserTabl__536C85E423404161").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
