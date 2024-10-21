using Microsoft.EntityFrameworkCore;
using PragmaOnce.Core.src.Entities;
using PragmaOnce.Core.src.Entities.Shop;
using PragmaOnce.Core.src.Entities.Shop.OrderAggregate;
using PragmaOnce.Core.src.Entities.TalentHub;
using PragmaOnce.Core.src.ValueObjects;
using PragmaOnce.Core.src.ValueObjects.Shop;
using PragmaOnce.Service.src.Interfaces;
using PragmaOnce.WebAPI.src.ValueConversion;

namespace PragmaOnce.WebAPI.src.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        private readonly IPasswordService _passwordService;

        #region Shop
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        #endregion

        #region TalentHub
        public DbSet<CandidateProfile> Candidates { get; set; } = null!;
        public DbSet<RecruiterProfile> Recruiters {  get; set; } = null!;
        public DbSet<Vacancy> Vacancies { get; set; } = null!;
        #endregion

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql("local",
        //        ServerVersion.AutoDetect("local"));
        //}


        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config, IPasswordService passwordService) : base(options)
        {
            _config = config;
            _passwordService = passwordService;
            ChangeTracker.LazyLoadingEnabled = true;
        }

        static AppDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify default schema for the database
            modelBuilder.HasDefaultSchema("dbo");

            // User Entity Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id).HasName("users_pkey");
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(u => u.Name).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
            });

            #region TalentHub Create Models
            modelBuilder.Entity<CandidateProfile>(entity =>
            {
                entity.ToTable("candidate");
                entity.HasKey(c => c.Id).HasName("candidate_pkey");
                entity.Property(c => c.UserId).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(255);
                entity.Property(c => c.MinYearsExperience).IsRequired();
                entity.Property(c => c.MaxYearsExperience).IsRequired();
            });

            modelBuilder.Entity<Vacancy>(entity =>
            {
                entity.ToTable("vacancy");
                entity.HasKey(c => c.Id).HasName("vacancy_pkey");
                entity.Property(c => c.RecruiterProfileId).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(255);
                entity.Property(c => c.CompanyId);
            });

            modelBuilder.Entity<RecruiterProfile>(entity =>
            {
                entity.ToTable("recruiter");
                entity.HasKey(c => c.Id).HasName("recruiter_pkey");
                entity.Property(c => c.UserId).IsRequired().HasMaxLength(255);
                entity.Property(c => c.CompanyId);
            });

            #endregion

            #region Shop Create Models
            // Address Entity Configuration
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("addresses");
                entity.HasKey(a => a.Id).HasName("addresses_pkey");
                entity.Property(a => a.AddressLine).IsRequired().HasMaxLength(255);
                entity.Property(a => a.City).IsRequired().HasMaxLength(100);
                entity.Property(a => a.PostalCode).IsRequired();
                entity.Property(a => a.Country).IsRequired().HasMaxLength(100);
            });

            // Order Entity Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(o => o.Id).HasName("orders_pkey");
                entity.Property(o => o.CheckoutUrl);
                entity.Property(o => o.StripeSessionId);
                entity.Property(o => o.TotalPrice).HasPrecision(18, 2);
                entity.Property(o => o.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(o => o.UpdatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(o => o.ShippingAddress).WithOne().HasForeignKey<Order>(o => o.AddressId).OnDelete(DeleteBehavior.Cascade);
            });

            // Category Entity Configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(c => c.Id).HasName("categories_pkey");
                entity.Property(c => c.Name).IsRequired().HasMaxLength(255);
                entity.HasIndex(c => c.Name).IsUnique();
                entity.Property(c => c.Image);//.IsRequired();
            });

            // Product Entity Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(p => p.Id).HasName("products_pkey");
                entity.Property(p => p.Title).IsRequired().HasMaxLength(255);
                entity.HasIndex(p => p.Title).IsUnique();
                entity.Property(p => p.Price).HasColumnType("decimal").HasPrecision(18, 2);
                //entity.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.SetNull);
            });

            // ProductImage Entity Configuration
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("product_images");
                entity.HasKey(pi => pi.Id).HasName("product_images_pkey");
                entity.Property(pi => pi.Url).IsRequired();
            });


      
            // OrderItem Entity Configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");
                entity.HasKey(oi => oi.Id).HasName("order_items_pkey");
                entity.Property(oi => oi.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(oi => oi.UpdatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(oi => oi.Price).HasPrecision(18, 2);
                entity.HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(oi => oi.ProductSnapshot)
                      .HasConversion(new JsonValueConverter<ProductSnapshot>()!)
                      .HasColumnType("varchar(8000)");
            });

            // Review Entity Configuration
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");
                entity.HasKey(r => r.Id).HasName("reviews_pkey");
                entity.HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(r => r.Product).WithMany(p => p.Reviews).HasForeignKey(r => r.ProductId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(oi => oi.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(oi => oi.UpdatedAt).HasDefaultValueSql("GETDATE()");
            });

            #endregion

            // Enums registration
            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserRole)Enum.Parse(typeof(UserRole), v));

            modelBuilder.Entity<Order>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

            // Fetch seed data
            SeedData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var categories = SeedingData.GetCategories();
            modelBuilder.Entity<Category>().HasData(categories);

            var products = SeedingData.GetProducts();
            modelBuilder.Entity<Product>().HasData(products);

            var productImages = new List<ProductImage>();
            foreach (var product in products)
            {
                var imagesForProduct = SeedingData.GenerateProductImagesForProduct(product.Id);
                productImages.AddRange(imagesForProduct);
            }
            modelBuilder.Entity<ProductImage>().HasData(productImages);

            var users = SeedingData.GetUsers();
            foreach (var user in users)
            {
                user.Password = _passwordService.HashPassword(user, user.Password!);
            }
            modelBuilder.Entity<User>().HasData(users);



            var candidates = SeedingData.CreateCandidateProfiles(users.Skip(1).Take(3).ToArray());
            modelBuilder.Entity<CandidateProfile>().HasData(candidates);

            var recruiters = SeedingData.CreateRecruiterProfiles(users.Take(3).ToArray());
            modelBuilder.Entity<RecruiterProfile>().HasData(recruiters);

            var vacancies = SeedingData.CreateVacancies(users.Take(3).ToArray());
            modelBuilder.Entity<Vacancy>().HasData(vacancies);

            var addresses = SeedingData.GetAddresses();
            modelBuilder.Entity<Address>().HasData(addresses);

            var orders = SeedingData.GetOrders(users, addresses);
            modelBuilder.Entity<Order>().HasData(orders);

            var orderItems = SeedingData.GetOrderItems(orders, products, productImages);
            modelBuilder.Entity<OrderItem>().HasData(orderItems);

            var reviews = SeedingData.GetReviews(users, products);
            modelBuilder.Entity<Review>().HasData(reviews);
        }
    }
}