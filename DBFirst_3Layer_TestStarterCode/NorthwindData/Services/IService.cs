using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindData.Services
{
    public interface IService
    {
        List<Customer> GetCustomers();
        Customer GetCustomerById(string  CustomerId);

        void CreateCustomer(Customer c);

        void SaveCustomerChanges();

        void RemoveCustomer(Customer c);
    }
}
