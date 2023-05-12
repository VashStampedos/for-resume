using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebWithEntity.Entity;

namespace WebWithEntity
{
    public partial class Tshop2Context : DbContext
    {
        public Tshop2Context()
        {
        }
        public Tshop2Context(DbContextOptions<Tshop2Context> options): base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Examples> Examples { get; set; }
        public virtual DbSet<Orderexample> Orderexample { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Favorites> Favorites { get; set; }
        public virtual DbSet<Baskets> Baskets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Tshop2;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Baskets>(entity =>
            {
                entity.ToTable("BASKETS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdExample).HasColumnName("idExample");

                entity.HasOne(e => e.IdUserNavigation)
                .WithMany(p => p.Baskets)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.IdExampleNavigation)
               .WithMany(p => p.Baskets)
               .HasForeignKey(d => d.IdExample)
               .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Favorites>(entity =>
            {
                entity.ToTable("FAVORITES");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdProduct).HasColumnName("idProduct");

                entity.HasOne(e => e.IdUserNavigation)
                .WithMany(p => p.Favorites)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.IdProductNavigation)
               .WithMany(p => p.Favorites)
               .HasForeignKey(d => d.IdProduct)
               .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("CATEGORIES");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryName)
                    .HasColumnName("categoryName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Examples>(entity =>
            {
                entity.ToTable("EXAMPLES");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdProduct).HasColumnName("idProduct");

                

                entity.Property(e => e.Size)
                    .HasColumnName("size")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.Examples)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__EXAMPLES__idProd__29572725");
            });

            modelBuilder.Entity<Orderexample>(entity =>
            {
                entity.ToTable("ORDEREXAMPLE");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.IdExample).HasColumnName("idExample");

                entity.Property(e => e.IdOrder).HasColumnName("idOrder");

                entity.HasOne(d => d.IdExampleNavigation)
                    .WithMany(p => p.Orderexample)
                    .HasForeignKey(d => d.IdExample)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ORDEREXAM__idExa__31EC6D26");

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.Orderexample)
                    .HasForeignKey(d => d.IdOrder)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ORDEREXAM__idOrd__30F848ED");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("ORDERS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Adress)
                    .HasColumnName("adress")
                    .HasMaxLength(50);

                entity.Property(e => e.DateOrder)
                    .HasColumnName("date_order")
                    .HasColumnType("date");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Status)
                    .HasColumnName("status_")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ORDERS__idUser__2E1BDC42");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("PRODUCTS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(50);

                entity.Property(e => e.IdCategory).HasColumnName("idCategory");

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ProductNname)
                    .HasColumnName("productNname")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.IdCategory)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__PRODUCTS__idCate__267ABA7A");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("USERS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.Login)
                    .HasColumnName("login_")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasColumnName("password_")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
