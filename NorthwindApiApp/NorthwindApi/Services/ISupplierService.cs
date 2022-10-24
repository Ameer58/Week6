using NorthwindApi.Models;

namespace NorthwindApi.Services
{
    public interface ISupplierService
    {
        public List<Supplier> GetSuppliers();
        public Task CreateSupplierAsync(Supplier s);
        public Task RemoveSupplierAsync(Supplier s);
        public Task<Supplier> GetSupplierByIdAsync(int id);
        public Task SaveSupplierChangesAsync();
        public bool SupplierExists(int id);
        public Task<List<Product>> GetProductsBySupplierIdAsync(int id);
        public Task CreateSuppliersAsync(IEnumerable<Supplier> s);
        public Task AddProductsAsync(IEnumerable<Product> p);

    }
}
