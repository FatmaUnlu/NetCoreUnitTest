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
            products = new List<Product>() { new Product { Id = 1, Name = "Kitap", Price=10, Stock=100, Color = "mavi"}, new Product { Id = 3, Name = "Bardak", Price = 16, Stock = 70, Color = "sar�" }, new Product { Id = 4, Name = "Usb", Price = 100, Stock = 100, Color = "mavi" } };  
        }

        [Fact]
        public async void GetProduct_ActionExecure_ReturnOkResultWithProduct()
        {
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(products);

            var result = await _productApiController.GetProduct();

            var okResult =Assert.IsType<OkObjectResult>(result); //resulttan ok result almam gerekiyor

            var returnProduct = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);//product nesnesi d�n�yor mu.

            //IAssignableFrom : Test s�recinde gelen de�erin hangi t�rden t�redi�ini kontrol eden metottur.Yani value IEnumarable ve product'tan mi t�remi� kontrol eder.           
            //3 tane mi veri geldi kontrol�
            Assert.Equal<int>(3,returnProduct.ToList().Count);
        }


        //GetProoduct metodunda �d invalid gelirse istenilen tepkiyi veriyor mu (not found) kontrol�
        [Theory]
        [InlineData(0)]
        public async void GetProduct_IdInvalid_ReturnNoFound(int id)
        {

            Product product = products.FirstOrDefault(x => x.Id == id);
           
            _mockRepo.Setup(x => x.GetById(id)).ReturnsAsync(product);                           

            var result = await _productApiController.GetProduct(id);

            Assert.IsType<NotFoundResult>(result);    //Api taraf�nda NotFound metodunda bir de�er olsayd� NotFoundObjectResult kullanacakt�k

        }

        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        public async void GetProduct_IdValid_ReturnOKResult(int id)
        {
            Product product = products.FirstOrDefault(x => x.Id == id);

            _mockRepo.Setup(x => x.GetById(id)).ReturnsAsync(product);

            var result = await _productApiController.GetProduct(id);
            var okresult = Assert.IsType<OkObjectResult>(result);//product nesnesi oldu�u i�in okobjectresult, ok result d�n�yor mu?
            var returnProduct = Assert.IsType<Product>(okresult.Value);//result�n i�indeki data product tipinde mi?

            Assert.Equal(id, returnProduct.Id); //result�n id si ile gelen id bbirbirine e�it mi?
            Assert.Equal(product.Name, returnProduct.Name); //product nesnesinin name de�eri ile result�n name i e�it mi
        }

        [Theory]
        [InlineData(1)]
        public void PutProduct_IdIsNullEqualProduct_ReturnBadRequest(int id)
        {
            //id si inline datadaki de�er olan [Product product] nesnesi elde etmek i�in
            var product = products.First(x => x.Id == id);

            var result = _productApiController.PutProduct(2, product); // id=2 ve id si inline datadaki de�er olan [Product product] nesnesi

            Assert.IsType<BadRequestResult>(result);

        }

        [Theory]
        [InlineData(1)]
        public void PutProduct_ActionExecutes_ReturnNoContent(int id)
        {
            //id si inline datadaki de�er olan [Product product] nesnesi elde etmek i�in
            var product = products.First(x => x.Id == id);

            _mockRepo.Setup(x => x.Update(product));

            var result = _productApiController.PutProduct(id, product); // id=inline datadaki de�er ve id si inline datadaki de�er olan [Product product] nesnesi

            _mockRepo.Verify(x => x.Update(product), Times.Once); //update metodu bir kere �al��t� m� do�rulamas�

            Assert.IsType<NoContentResult>(result);

        }
    }
}
