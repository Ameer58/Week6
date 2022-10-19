using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NorthwindData;
using NorthwindData.Services;
using NUnit.Framework;

namespace NorthwindTests
{
    internal class CustomerServiceTests
    {
        private CustomerService _sut;
        private NorthwindContext _context;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "Ex")
                .Options;
            _context = new NorthwindContext(options);
            _sut = new CustomerService(_context);

            _sut.CreateCustomer(new Customer { CustomerId = "Phill", ContactName = "Philip", CompanyName = "Sparta Global", City = "Birmingham" });
            _sut.CreateCustomer(new Customer { CustomerId = "Ma", ContactName = "Mandal", CompanyName = "Sparta Global", City = "Birmingham" });
        }

        [Test]
        public void GivenValidId_CorrectCustomerReturned()
        {
            var result = _sut.GetCustomerById("Phill");
           Assert.That(result, Is.TypeOf<Customer>());
            Assert.That(result.ContactName, Is.EqualTo("Philip"));
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
        }

        [Test]
        public void GivenANewCustomer_CreateCustomerAddsItToTheDatabase()
        {
            var numOfCustomersBefore = _context.Customers.Count();
            var newCustomer = new Customer
            {
                CustomerId = "Odell",
                ContactName = "Max Odell",
                CompanyName = "Sparta Global",
                City = "Surrey"
            };

            _sut.CreateCustomer(newCustomer);
            var customersAfter = _context.Customers.Count();
            var result = _sut.GetCustomerById("Odel");

            Assert.That(numOfCustomersBefore + 1, Is.EqualTo(customersAfter));
            
        
        }

        [Test]
        public void GetCustomersReturnsListOfCustomers()
        {
            var result = _sut.GetCustomers();
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<List<Customer>>());
            
        }

        [Test]
        public void GivenRemoveCustomer_CustomerIsRemoved()
        {
            var numOfCustomersBefore = _context.Customers.Count();
            var cust = _sut.GetCustomerById("Ma");
            _sut.RemoveCustomer(cust);
            var customersAfter = _context.Customers.Count();
            Assert.That(numOfCustomersBefore -1, Is.EqualTo(customersAfter));
        }

    }
}
