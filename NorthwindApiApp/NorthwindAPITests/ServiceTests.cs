using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindAPITests
{
    public class Tests
    {
        private NorthwindContext _context;
        private ISupplierService _sut;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "NorthwindDB").Options;
            _context = new NorthwindContext(options);
            _sut = new SupplierService(_context);
            _sut.CreateSupplierAsync(new Supplier { SupplierId = 1, CompanyName = "Sparta Global", City = "Birmingham", Country = "UK", ContactName = "Nish Mandal", ContactTitle = "Manager" }).Wait();
            _sut.CreateSupplierAsync(new Supplier { SupplierId = 2, CompanyName = "Nintendo", City = "Tokyo", Country = "Japan", ContactName = "Shigeru Miyamoto", ContactTitle = "CEO" }).Wait();
        }

        [Test]
        public void GivenValidID_GetSupplierById_ReturnsCorrectSupplier()
        {
            var result = _sut.GetSupplierByIdAsync(1).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Supplier>());
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
        }

        [Test]
        public void GivenANewSupplier_CreateSupplierAddsItToTheDatabase()
        {
            var numOfCustomersBefore = _context.Suppliers.Count();
            var newCustomer = new Supplier
            {
                SupplierId = 3,
                ContactName = "Max Odell",
                CompanyName = "Sparta Global",
                City = "Surrey"
            };

            _sut.CreateSupplierAsync(newCustomer);
            var customersAfter = _context.Suppliers.Count();

            Assert.That(numOfCustomersBefore + 1, Is.EqualTo(customersAfter));
        }

        [Test]
        public void GetSuppliersReturnsListOfSuppliers()
        {
            var result = _sut.GetSuppliers();
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<List<Supplier>>());

        }


        [Test]
        public void GivenRemoveSupplier_SupplierIsRemoved()
        {
            
            var numOfCustomersBefore = _context.Suppliers.Count();
            var s = _sut.GetSupplierByIdAsync(2).Result;
            _sut.RemoveSupplierAsync(s);
            var customersAfter = _context.Suppliers.Count();
            Assert.That(numOfCustomersBefore - 1, Is.EqualTo(customersAfter));
        }

        [Test]
        public void GetSuppliersProducts_ReturnsListOfProducts()
        {
            var result = _sut.GetProductsBySupplierIdAsync(1).Result;
          
            Assert.That(result, Is.TypeOf<List<Product>>());
        }

        //[Test]
        //public void  AddProducts_AddsListOfProducts()
        //{
        //    var productsBefore = _context.Products.Count();
        //    var products = new List<Product>() {
        //        new Product{ SupplierId = 1, CategoryId = 2, Discontinued = false, ProductId = 4, ProductName="sdad", UnitPrice=1 }};
        //    _sut.AddProductsAsync(products);
        //    var productsAfter = _context.Products.Count();
        //    Assert.That(productsBefore+1, Is.EqualTo(productsAfter));
        //}


    }
}