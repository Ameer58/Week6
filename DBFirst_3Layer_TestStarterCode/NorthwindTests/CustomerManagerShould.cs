using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using NorthwindBusiness;
using NorthwindData;
using NorthwindData.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindTests
{
    public class CustomerManagerShould
    {
        private CustomerManager _swt;

        [Ignore("Should fail")]
        [Test]
        public void BeAbleToConstructCustomerManager()
        {
            _swt = new CustomerManager(null);
            Assert.That(_swt, Is.InstanceOf<CustomerManager>());
        }

        [Test]
        public void BeAbleToConstructUsingMoq()
        {
            var mocObject = new Mock<IService>();
            _swt = new CustomerManager(mocObject.Object);
            Assert.That(_swt, Is.InstanceOf<CustomerManager>());
        }

        //stub
        [Category("Happy Path")]
        [Test]
        public void ReturnTrue_WhenUpdateIsCalledWithValidId()
        {
            var mockObject = new Mock<IService>();
            var originalCustomer = new Customer
            {
                CustomerId = "Manda"
            };
            mockObject.Setup(x => x.GetCustomerById("Manda")).Returns(originalCustomer);
            _swt = new CustomerManager(mockObject.Object);

            var result = _swt.Update("Manda", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(result, Is.True);
        }

        [Category("Sad Path")]
        [Test]
        public void RetursFalse_WhenUpdateIsCalled_WithInvalidId()
        {
            //Arrange
            var mockObject = new Mock<IService>();
            mockObject.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns((Customer)null);
            _swt = new CustomerManager(mockObject.Object);
            //Act
            var result = _swt.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            //Assert
            Assert.That(result, Is.False);
        }

        [Category("Sad Path")]
        [Test]
        public void NotChangeTheSelectedCustomer_WhenUpdateIsCalled_WithValidId()
        {
            var mockObject = new Mock<IService>();
            mockObject.Setup(cs => cs.GetCustomerById("MANDA")).Returns((Customer)null);
            var originalCustomer = new Customer
            {
                CustomerId = "MANDA",
                ContactName = "Nish Mandal",
                CompanyName = "Sparta Global",
                City = "Birmingham"
            };
            _swt = new CustomerManager(mockObject.Object);
            _swt.SetSelectedCustomer(originalCustomer);

            var result = _swt.Update("MANDA", "Nish Mandal", "UK", "London", null);
            Assert.That(_swt.SelectedCustomer.ContactName, Is.EqualTo("Nish Mandal"));
            Assert.That(_swt.SelectedCustomer.Country, Is.EqualTo(null));
            Assert.That(_swt.SelectedCustomer.City, Is.EqualTo("Birmingham"));
        }

        [Category("Happy Path")]
        [Test]
        public void ReturnTrueWhenDeleteWithValidId()
        {
            var mockObject = new Mock<IService>();
            var originalCustomer = new Customer
            {
                CustomerId = "Manda",
                ContactName = "Nish Mandal",
                CompanyName = "Sparta Global",
                City = "Birmingham"
            };
            mockObject.Setup(x => x.GetCustomerById("Manda")).Returns(originalCustomer);
            _swt = new CustomerManager(mockObject.Object);

            var result = _swt.Delete("Manda");
            Assert.That(result, Is.True);
        }

        [Category("Unhappy path")]
        [Test]
        public void RetursFalse_WhenDeleteIsCalled_WithInvalidId()
        {
            //Arrange
            var mockObject = new Mock<IService>();
            mockObject.Setup(x => x.GetCustomerById(It.IsAny<string>())).Returns((Customer)null);
            _swt = new CustomerManager(mockObject.Object);
            //Act
            var result = _swt.Delete(It.IsAny<String>());
            //Assert
            Assert.That(result, Is.False);
        }

        [Category("Sad path")]
        [Test]
        public void ReturnsFalse_WhenUpdateIsCalled_AndDatabaseThrowsException()
        {
            var mockObject = new Mock<IService>();
            mockObject.Setup(x => x.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
            mockObject.Setup(x => x.SaveCustomerChanges()).Throws<DbUpdateConcurrencyException>();
            _swt = new CustomerManager(mockObject.Object);
            //act
            var result = _swt.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(result, Is.False);

        }

        [Test]
        public void RetrieveAllReturnsListOfCustomers()
        {
            var mockObject = new Mock<IService>();
            mockObject.Setup(x=>x.GetCustomers()).Returns(new List<Customer>());
            _swt = new CustomerManager(mockObject.Object);
            //act
            var result = _swt.RetrieveAll();
            Assert.That(result, Is.TypeOf<List<Customer>>());
        }

        [Test]
        public void SetSelectedCustomer_SetsTheSelectedCustomer()
        {
            var mockObject = new Mock<IService>();
            var originalCustomer = new Customer
            {
                CustomerId = "Manda"
            };
           
            _swt = new CustomerManager(mockObject.Object);
            _swt.SetSelectedCustomer(originalCustomer);
            Assert.That(originalCustomer.CustomerId, Is.EqualTo(_swt.SelectedCustomer.CustomerId));
        }

       
        //upto now state base testing 

        //Behaviour based testing
        [Test]
        public void CallSaveCustomerChanges_WhenUpdateIsCalled_WithValidId()
        {
            var mockObject = new Mock<IService>();
            mockObject.Setup(x => x.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
            _swt = new CustomerManager(mockObject.Object);
            var result = _swt.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            mockObject.Verify(x => x.SaveCustomerChanges(), Times.Once);
            mockObject.Verify(x => x.SaveCustomerChanges(), Times.Exactly(1));
        }

        [Test]
        public void CallCreateCustomer_WhenCreateCustomerIsCalled()
        {
            var mockObject = new Mock<IService>();
            mockObject.Setup(x => x.CreateCustomer(It.IsAny<Customer>()));
            _swt = new CustomerManager(mockObject.Object);
            _swt.Create("MANDA", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            mockObject.Verify(x => x.CreateCustomer(It.IsAny<Customer>()), Times.Once);

        }

        [Test]
        public void CallRemoveCustomer_WhenDeleteIsCalled_WithValidId()
        {
            //Arrange
            var mockObject = new Mock<IService>();
            var myCustomer = new Customer();
            mockObject.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(myCustomer);
            _swt = new CustomerManager(mockObject.Object);
            _swt.Delete(It.IsAny<string>());

            //Assert
            mockObject.Verify(x => x.RemoveCustomer(myCustomer), Times.Once);
        }
    }
}
