using InventoryDatabaseCore;
using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Activity0302_EFCoreNewDb
{
    class Program
    {
        static IConfigurationRoot _configuration;
        static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

        static void Main(string[] args)
        {
            BuildOptions();
            DeleteAllItems();
            InsertItems();
            UpdateItems();
            ListInventory();
        }

        static void BuildOptions()
        {
            _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
        }

        static void InsertItems()
        {
            var items = new List<Item>() 
            {
                new Item() { Name = "Top Gun" , IsActive=true, Description="Top Gun Description"},
                new Item() { Name = "Batman Begins", IsActive=true, Description="Batman Begins Description"},
                new Item() { Name = "Inception", IsActive=true, Description="Inception Description" },
                new Item() { Name = "Star Wars: The Empire Strikes Back", IsActive=true, Description="Star Wars Description"},
                new Item() { Name = "Remember the Titans", IsActive=true, Description="Remember the Titans Description"}
            };

            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                foreach (var item in items) 
                { 
                    item.CreatedByUserId = 1; 
                }

                db.AddRange(items);
                db.SaveChanges();
            }
        }

        static void DeleteAllItems()
        {
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var items = db.Items.ToList();

                foreach (var item in items) 
                { 
                    item.LastModifiedUserId = 1; 
                }

                db.Items.RemoveRange(items);
                db.SaveChanges();
            }
        }

        static void UpdateItems()
        {
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var items = db.Items.ToList();
                foreach(var item in items)
                {
                    item.LastModifiedUserId = 1;
                    item.CurrentOrFinalPrice = 9.99M;
                }
                db.Items.UpdateRange(items);
                db.SaveChanges();
            }
        }

        static void ListInventory()
        {
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                var items = db.Items.Take(5).OrderBy(x => x.Name).ToList();
                items.ForEach(x => Console.WriteLine($"New Item: {x.Name}"));
            }
        }
    }
}
