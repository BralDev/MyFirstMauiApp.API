using Dapper;
using MyFirstMauiApp.API.Data.Connection;
using MyFirstMauiApp.API.Data.Entities;
using MyFirstMauiApp.API.Data.Interfaces;
using System.Data;

namespace MyFirstMauiApp.API.Data.Repositories
{
    // Heredamos de la interfaz IProductRepository
    public class ProductRepository : IProductRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        // Inyección de dependencias: Pedimos el factory que registramos en Program.cs
        public ProductRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // Abrimos la conexión, using la cerrará automaticamente al terminar
            using var connection = _connectionFactory.CreateConnection();
        
            // Dapper ejecuta el procedimiento y mapea cada fila de SQL Server a un objeto Product de C#           
            return await connection.QueryAsync<Product>(
                "sp_Product_GetAll",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            // QueryFirstOrDefaultAsync devuelve el primer resultado, o null si no encuentra nada.
            // Le pasamos un objeto anónimo { Id = id } para que Dapper reemplace el @Id en el SQL.            
            return await connection.QueryFirstOrDefaultAsync<Product>(
                "sp_Product_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddAsync(Product product)
        {
            using var connection = _connectionFactory.CreateConnection();
            // En el procedimiento usamos SCOPE_IDENTITY() para que obtener el ID autoincremental que acaba de generar.
            return await connection.ExecuteScalarAsync<int>(
                "sp_Product_Create",
                new
                {
                    product.Name,
                    product.Description,
                    product.Price,
                    product.Stock
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            using var connection = _connectionFactory.CreateConnection();
            // Ejecutamos y mediante el OUTPUT INSERTED en el SP, recibimos el objeto Product completo mapeado por Dapper.
            return await connection.QueryFirstOrDefaultAsync<Product>(
                "sp_Product_Update",
                new
                {
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.Stock
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            // ExecuteAsync devuelve el número de filas afectadas por el SP en la base de datos.
            var rowsAffected = await connection.ExecuteAsync(
                "sp_Product_Delete",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            // Si rowsAffected es mayor a 0, significa que la actualización fue exitosa.
            return rowsAffected > 0;
        }
    }
}