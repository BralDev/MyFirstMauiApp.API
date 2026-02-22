using MyFirstMauiApp.API.Business.DTOs;

namespace MyFirstMauiApp.API.Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto);
        Task<ProductResponseDto?> UpdateProductAsync(ProductUpdateDto dto);
        Task<bool> DeleteProductAsync(int id);
    }
}