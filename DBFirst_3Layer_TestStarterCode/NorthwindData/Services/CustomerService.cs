using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindData.Services
{
    public class CustomerService : IService
    {
        private readonly NorthwindContext _context;
        
        public CustomerService()
        {
            _context = new NorthwindContext();
        }

        public CustomerService(NorthwindContext context)
        {
            _context = context;    
        }
        public void CreateCustomer(Customer c)
        {
            _context.Customers.Add(c);
            _context.SaveChanges();
        }

        public Customer GetCustomerById(string CustomerId)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        public void RemoveCustomer(Customer c)
        {
            throw new NotImplementedException();
        }

        public void SaveCustomerChanges()
        {
            throw new NotImplementedException();
        }
    }
}
