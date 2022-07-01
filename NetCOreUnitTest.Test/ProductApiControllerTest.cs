using Microsoft.AspNetCore.Mvc;
using Moq;
using NetCoreUnitTest.Controllers;
using NetCoreUnitTest.Models;
using NetCoreUnitTest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetCOreUnitTest.Test
{
    public class ProductApiControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductApiController _productApiController;
        private List<Product> products;
        public ProductApiControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _productApiController = new ProductApiController(_mockRepo.Object);
            products = new List<Product>() { new Product { Id = 1, Name = "Kitap", Price=10, Stock=100, Color = "mavi"}, new Product { Id = 3, Name = "Bardak", Price = 16, Stock = 70, Color = "sarý" }, new Product { Id = 4, Name = "Usb", Price = 100, Stock = 100, Color = "mavi" } };  
        }

        [Fact]
        public async void GetProduct_ActionExecure_ReturnOkResultWithProduct()
        {
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(products);

            var result = await _productApiController.GetProduct();

            var okResult =Assert.IsType<OkObjectResult>(result); //resulttan ok result almam gerekiyor

            var returnProduct = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);//product nesnesi dönüyor mu.

            //IAssignableFrom : Test sürecinde gelen deðerin hangi türden türediðini kontrol eden metottur.Yani value IEnumarable ve product'tan mi türemiþ kontrol eder.           
            //3 tane mi veri geldi kontrolü
            Assert.Equal<int>(3,returnProduct.ToList().Count);
        }


        //GetProoduct metodunda ýd invalid gelirse istenilen tepkiyi veriyor mu (not found) kontrolü
        [Theory]
        [InlineData(0)]
        public async void GetProduct_IdInvalid_ReturnNoFound(int id)
        {

            Product product = products.FirstOrDefault(x => x.Id == id);
           
            _mockRepo.Setup(x => x.GetById(id)).ReturnsAsync(product);                           

            var result = await _productApiController.GetProduct(id);

            Assert.IsType<NotFoundResult>(result);    //Api tarafýnda NotFound metodunda bir deðer olsaydý NotFoundObjectResult kullanacaktýk

        }

        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        public async void GetProduct_IdValid_ReturnOKResult(int id)
        {
            Product product = products.FirstOrDefault(x => x.Id == id);

            _mockRepo.Setup(x => x.GetById(id)).ReturnsAsync(product);

            var result = await _productApiController.GetProduct(id);
            var okresult = Assert.IsType<OkObjectResult>(result);//product nesnesi olduðu için okobjectresult, ok result dönüyor mu?
            var returnProduct = Assert.IsType<Product>(okresult.Value);//resultýn içindeki data product tipinde mi?

            Assert.Equal(id, returnProduct.Id); //resultýn id si ile gelen id bbirbirine eþit mi?
            Assert.Equal(product.Name, returnProduct.Name); //product nesnesinin name deðeri ile resultýn name i eþit mi
        }

        [Theory]
        [InlineData(1)]
        public void PutProduct_IdIsNullEqualProduct_ReturnBadRequest(int id)
        {
            //id si inline datadaki deðer olan [Product product] nesnesi elde etmek için
            var product = products.First(x => x.Id == id);

            var result = _productApiController.PutProduct(2, product); // id=2 ve id si inline datadaki deðer olan [Product product] nesnesi

            Assert.IsType<BadRequestResult>(result);

        }

        [Theory]
        [InlineData(1)]
        public void PutProduct_ActionExecutes_ReturnNoContent(int id)
        {
            //id si inline datadaki deðer olan [Product product] nesnesi elde etmek için
            var product = products.First(x => x.Id == id);

            _mockRepo.Setup(x => x.Update(product));

            var result = _productApiController.PutProduct(id, product); // id=inline datadaki deðer ve id si inline datadaki deðer olan [Product product] nesnesi

            _mockRepo.Verify(x => x.Update(product), Times.Once); //update metodu bir kere çalýþtý mý doðrulamasý

            Assert.IsType<NoContentResult>(result);

        }
    }
}
