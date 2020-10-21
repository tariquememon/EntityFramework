using InventoryModels;
using InventoryModels.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace InventoryDatabaseCore
{
    public class InventoryDbContext : DbContext
    {
        private IConfigurationRoot _configuration;

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryColor> CategoryColors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GetItemsForListingDto> ItemsForListing { get; set; }
        public DbSet<AllItemsPipeDelimitedStringDto> AllItemsOutput { get; set; }
        public DbSet<GetItemsTotalValueDto> GetItemsTotalValues { get; set; }

        public InventoryDbContext() : base() {}

        public InventoryDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemGenre>()
                .HasIndex(ig => new { ig.ItemId, ig.GenreId })
                .IsUnique()
                .IsClustered(false);

            modelBuilder.Entity<GetItemsForListingDto>()
                .HasNoKey()
                .ToView("ItemsForListing");

            modelBuilder.Entity<AllItemsPipeDelimitedStringDto>()
                .HasNoKey()
                .ToView("AllItemsOutput");

            modelBuilder.Entity<GetItemsTotalValueDto>()
                .HasNoKey()
                .ToView("GetItemsTotalValues");

            modelBuilder.Entity<Genre>(x => {
                x.HasData(
                    new Genre() { Id = 1, CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false, Name = "Fantasy" },
                    new Genre() { Id = 2, CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false, Name = "Sci/Fi" },
                    new Genre() { Id = 3, CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false, Name = "Horror" },
                    new Genre() { Id = 4, CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false, Name = "Comedy" },
                    new Genre() { Id = 5, CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false, Name = "Drama" }
                );
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _configuration = builder.Build();

                var cnstr = _configuration.GetConnectionString("InventoryManager");
                optionsBuilder.UseSqlServer(cnstr);
            }
        }

        public override int SaveChanges()
        {
            var tracker = ChangeTracker;

            foreach(var entry in tracker.Entries())
            {
                if(entry.Entity is FullAuditModel)
                {
                    var referenceEntity = entry.Entity as FullAuditModel;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            referenceEntity.CreatedDate = System.DateTime.Now;
                            break;
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            referenceEntity.LastModifiedDate = System.DateTime.Now;
                            break;
                        default:
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
