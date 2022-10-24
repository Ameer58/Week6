using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;

namespace NorthwindApi.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly NorthwindContext _context;

        public SupplierService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task AddProductsAsync(IEnumerable<Product> p)
        {
            await _context.Products.AddRangeAsync();
            await _context.SaveChangesAsync();
        }

        public async Task CreateSupplierAsync(Supplier s)
        {
            _context.Suppliers.Add(s);
            await _context.SaveChangesAsync();
        }

        public async Task CreateSuppliersAsync(IEnumerable<Supplier> s)
        {
            _context.Suppliers.AddRange(s);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsBySupplierIdAsync(int id)
        {
            return await _context.Products.Where(p => p.SupplierId == id).ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers.Include(s => 
            s.Products).Where(x => x.SupplierId == id)
            .FirstOrDefaultAsync();
        }

        public List<Supplier> GetSuppliers()
        {
            return _context.Suppliers.Include(s => s.Products).ToList();
        }

        public async Task RemoveSupplierAsync(Supplier s)
        {
            _context.Suppliers.Remove(s);
            await _context.SaveChangesAsync();
        }

        public async Task SaveSupplierChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool SupplierExists(int id)
        {
            return (bool)(_context.Suppliers?.Any(e => e.SupplierId == id));
        }
    }
}
