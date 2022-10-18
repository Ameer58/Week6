using NUnit.Framework;
using NorthwindBusiness;
using NorthwindData;
using System.Linq;

namespace NorthwindTests
{
    public class CustomerTests
    {
        CustomerManager _customerManager;
        [SetUp]
        public void Setup()
        {
            _customerManager = new CustomerManager();
            // remove test entry in DB if present
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "Mand"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenANewCustomerIsAdded_TheNumberOfCustemersIncreasesBy1()
        {
            using(var db = new NorthwindContext())
            {
                var customersBefore = db.Customers.Count();
                _customerManager.Create("Mand", "N", "SpartaGlobal");
                var customersAfter = db.Customers.Count();
                Assert.That(customersBefore +1, Is.EqualTo(customersAfter));
            }

        }

        [Test]
        public void WhenANewCustomerIsAdded_TheirDetailsAreCorrect()
        {
            using(var db = new NorthwindContext())
            {
                _customerManager.Create("Mand", "N", "SpartaGlobal");
                var selectedCustomer = db.Customers.Find("Mand");
                Assert.That(selectedCustomer.ContactName, Is.EqualTo("N"));
                Assert.That(selectedCustomer.CompanyName, Is.EqualTo("SpartaGlobal"));
            }
        }

        [Test]
        public void WhenACustomersDetailsAreChanged_TheDatabaseIsUpdated()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("Mand", "N", "SpartaGlobal");
                _customerManager.Update("Mand", "N", "Holland", "Adam", "2036HD");
                var selectCustomer = db.Customers.Find("Mand");
                Assert.That(selectCustomer.Country, Is.EqualTo("Holland"));
            }
        }

        [Test]
        public void WhenACustomerIsUpdated_SelectedCustomerIsUpdated()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("Mand", "N", "SpartaGlobal");
                _customerManager.Update("Mand", "N", "Holland", "Adam", "2036HD");
                var updated = db.Customers.Find("Mand");

                Assert.That(updated.ToString(), Is.EqualTo(_customerManager.SelectedCustomer.ToString()));

            }

        }

        [Test]
        public void WhenACustomerIsNotInTheDatabase_Update_ReturnsFalse()
        {
          
                bool update = _customerManager.Update("asfafad", "asd", "wqe", "wef", "qwf");
                Assert.That(update, Is.False);
            

        }

        [Test]
        public void WhenACustomerIsRemoved_TheNumberOfCustomersDecreasesBy1()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("Mand", "N", "SpartaGlobal");
                var customersBefore = db.Customers.Count();
                _customerManager.Delete("Mand");
                var customersAfter = db.Customers.Count();
                Assert.That(customersBefore - 1, Is.EqualTo(customersAfter));
            }
        }

        [Test]
        public void WhenACustomerIsRemoved_TheyAreNoLongerInTheDatabase()
        {
            using(var db = new NorthwindContext())
            {
                _customerManager.Create("Mand", "N", "SpartaGlobal");
                _customerManager.Delete("Mand");
                var deleted = db.Customers.Find("Mand");
                Assert.That(deleted, Is.Null);
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MAND"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }
    }
}