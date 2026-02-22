using MyFirstMauiApp.API.Business.DTOs;
using MyFirstMauiApp.API.Business.Interfaces;
using MyFirstMauiApp.API.Data.Entities;
using MyFirstMauiApp.API.Data.Interfaces;

namespace MyFirstMauiApp.API.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        // Inyectamos el repositorio
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var entities = await _repository.GetAllAsync();

            // Mapeamos la lista de Entidades a una lista de DTOs usando LINQ
            return entities.Select(e => new ProductResponseDto(
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Price = e.Price,
                Stock = e.Stock,
                CreatedAt = e.CreatedAt
            ));
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new ProductResponseDto(
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Stock = entity.Stock,
                CreatedAt = entity.CreatedAt
            );
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto)
        {
            // Mapear DTO a Entidad
            var entity = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CreatedAt = DateTime.Now // Asignamos la fecha de creación
            };

            // Guardar en BD, por contrato el repositorio nos devuelve solo el ID nuevo
            var newId = await _repository.AddAsync(entity);

            // Armar la respuesta final usando el ID que nos dio la BD
            return new ProductResponseDto(
                Id = newId,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Stock = entity.Stock,
                CreatedAt = entity.CreatedAt
            );
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(ProductUpdateDto dto)
        {
            // Mapeamos los datos para actualizar
            var entity = new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock
            };

            // Intentamos actualizar en la base de datos
            var success = await _repository.UpdateAsync(entity);

            // Si devolvió null, significa que el ID no existía
            if (success == null) return null;
                        
            return new ProductResponseDto(
                Id = success.Id,
                Name = success.Name,
                Description = success.Description,
                Price = success.Price,
                Stock = success.Stock,
                CreatedAt = success.CreatedAt
            );
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}