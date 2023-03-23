using Microsoft.EntityFrameworkCore;
using NetCoreUnitTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCOreUnitTest.Test
{
    public class ProductControllerTest
    {
       protected DbContextOptions<UnitTestDbContext> _contextOptions { get; private set; }

        public void SetContextOptions(DbContextOptions<UnitTestDbContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        public void Seed()
        {
            using (UnitTestDbContext dbContext = new UnitTestDbContext(_contextOptions))
            {
                dbContext.Database.EnsureDeleted(); //dbnin silindiğinden emin olmak için
                dbContext.Database.EnsureCreated();

                dbContext.Categories.Add(new Category() { Name = "kalemler"});
                dbContext.Categories.Add(new Category() { Name = "defterler" });
                dbContext.SaveChanges();

                dbContext.Products.Add(new Product() { CategoryId = 1, Name = "kalem 10", Price = 100, Stock = 100, Color = "Kırmızı" });
                dbContext.Products.Add(new Product() { CategoryId = 1, Name = "kalem 20", Price = 100, Stock = 100, Color = "Mavi" });

                dbContext.SaveChanges();
            }
        }
    }
}
