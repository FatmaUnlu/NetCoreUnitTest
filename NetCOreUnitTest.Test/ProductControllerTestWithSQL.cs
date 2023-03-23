using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreUnitTest.Controllers;
using NetCoreUnitTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetCOreUnitTest.Test
{
    public class ProductControllerTestWithSQL : ProductControllerTest
    {
        public ProductControllerTestWithSQL()
        {
            var sqlCon = @"Server=TRL22-51\FATMAUNLU;Database=UnitTestDb;Trusted_Connection=true; MultipleActiveResultSets=true";

            SetContextOptions(new DbContextOptionsBuilder<UnitTestDbContext>().UseSqlServer(sqlCon).Options);
        }

        [Fact]
        public async Task Create_ModelValidProduct_ReturnRedirectToActionWithSaveProduct()
        {
            var newProduct = new Product { Name = "Defter 30", Price = 200, Stock = 100, Color = "Mavi"};
            using (var context = new UnitTestDbContext(_contextOptions))
            {
                var category = context.Categories.First();

                newProduct.CategoryId = category.Id;

                //var repository = new repository<Product>(context);
                var controller = new ProductsController(context);

                var result = await controller.Create(newProduct);

                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);
            }

            using (var context = new UnitTestDbContext(_contextOptions))
            {
                var product = context.Products.FirstOrDefault(x => x.Name == newProduct.Name);

                Assert.Equal(newProduct.Name, product.Name);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteCategory_ExistCategoryId_DeletedAllProducts(int categoryId)
        {
            using (var context = new UnitTestDbContext(_contextOptions))

            {
                var category = await context.Categories.FindAsync(categoryId);

                context.Categories.Remove(category);

                context.SaveChanges();
            }

            using (var context = new UnitTestDbContext(_contextOptions))
            {
                var products = await context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();

                Assert.Empty(products);
            }
        }
    }
}
