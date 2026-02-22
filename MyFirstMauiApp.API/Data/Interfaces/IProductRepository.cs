using MyFirstMauiApp.API.Data.Entities;

namespace MyFirstMauiApp.API.Data.Interfaces
{
    public interface IProductRepository
    {
        // Usamos Task<> porque todas las llamadas a BD deben ser asíncronas
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task<int> AddAsync(Product product);

        Task<Product?> UpdateAsync(Product product);

        Task<bool> DeleteAsync(int id);
    }
}