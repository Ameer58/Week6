using Microsoft.Extensions.Logging;
using NorthwindAPI.Controllers;
using NorthwindApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;
using NorthwindApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Http.Features;

namespace NorthwindAPITests
{
    public class SupplierControllerShould
    {
        private SuppliersController? _sut;

        [Test]
        public void BeAbleToBeConstructed()
        {
            var mockService = new Mock<ISupplierService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            _sut = new SuppliersController(mockLogger.Object, mockService.Object);
            Assert.That(_sut, Is.InstanceOf<SuppliersController>());
        }
        [Category("Happy Path")]
        [Test]
        public void ReturnNoContentStatusCode_WhenPutIsCalledWithValidId()
        {
            var mockObject = new Mock<ISupplierService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            var originalCustomer = new Supplier
            {
                SupplierId = 1
            };
            var dto = new SupplierDTO
            {
                SupplierId = 1,
                CompanyName = "Sparta"
            };
            mockObject.Setup(x => x.GetSupplierByIdAsync(1)).ReturnsAsync(originalCustomer);
            _sut = new SuppliersController(mockLogger.Object, mockObject.Object);
            
            var result = _sut.PutSupplier(1, dto).Result as StatusCodeResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        }
        [Category("UnhappyPath")]
        [Test]
        public void ReturnBadRequestStatusCode_WhenPutIsCalledWithInValidId()
        {
            var mockObject = new Mock<ISupplierService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            var originalCustomer = new Supplier
            {
                SupplierId = 1
            };
            var dto = new SupplierDTO
            {
                SupplierId = 2,
                CompanyName = "Sparta"
            };
            mockObject.Setup(x => x.GetSupplierByIdAsync(1)).ReturnsAsync(originalCustomer);
            _sut = new SuppliersController(mockLogger.Object, mockObject.Object);
            var result = _sut.PutSupplier(1, dto).Result as StatusCodeResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Category("Happy Path")]
        [Test]
        public void ReturnNoContentStatusCode_WhenDeleteIsCalledWithValidId()
        {
            var mockObject = new Mock<ISupplierService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            var originalCustomer = new Supplier
            {
                SupplierId = 1
            };
            mockObject.Setup(x => x.GetSupplierByIdAsync(1)).ReturnsAsync(originalCustomer);
            _sut = new SuppliersController(mockLogger.Object, mockObject.Object);

            var result = _sut.DeleteSupplier(1).Result as StatusCodeResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        }

        [Category("Unhappy path")]
        [Test]
        public void ReturnNotFoundStatusCode_WhenDeleteIsCalledWithInValidId()
        {
            var mockObject = new Mock<ISupplierService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            var originalCustomer = new Supplier
            {
                SupplierId = 1
            };
            mockObject.Setup(x => x.GetSupplierByIdAsync(1)).ReturnsAsync(originalCustomer);
            _sut = new SuppliersController(mockLogger.Object, mockObject.Object);

            var result = _sut.DeleteSupplier(0).Result as StatusCodeResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }



    }

    
}
