using Dapper;
using MyFirstMauiApp.API.Data.Connection;
using MyFirstMauiApp.API.Data.Entities;
using MyFirstMauiApp.API.Data.Interfaces;

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

            // Escribimos consulta SQL requerida
            var sql = @"
                SELECT 
                    Id, 
                    Name, 
                    Description, 
                    Price, 
                    Stock, 
                    CreatedAt 
                FROM Products";

            // Dapper ejecuta la consulta y mapea cada fila de SQL Server a un objeto Product de C#
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            // Uso de @Id como parámetro para evitar Inyección SQL
            var sql = @"
                SELECT 
                    Id, 
                    Name, 
                    Description, 
                    Price, 
                    Stock, 
                    CreatedAt 
                FROM Products 
                WHERE Id = @Id";

            // QueryFirstOrDefaultAsync devuelve el primer resultado, o null si no encuentra nada.
            // Le pasamos un objeto anónimo { Id = id } para que Dapper reemplace el @Id en el SQL.
            return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Product product)
        {
            using var connection = _connectionFactory.CreateConnection();

            // Insertamos y al final usamos SCOPE_IDENTITY() (propio de SQL Server) 
            // para que nos devuelva el ID autoincremental que acaba de generar.
            var sql = @"
                INSERT INTO Products (Name, Description, Price, Stock, CreatedAt) 
                VALUES (@Name, @Description, @Price, @Stock, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            // ExecuteScalarAsync ejecuta la consulta y devuelve la primera columna de la primera fila (el ID nuevo).            
            return await connection.ExecuteScalarAsync<int>(sql, product);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                UPDATE Products 
                SET Name = @Name, 
                    Description = @Description, 
                    Price = @Price, 
                    Stock = @Stock 
                WHERE Id = @Id";

            // ExecuteAsync devuelve el número de filas afectadas en la base de datos.
            var rowsAffected = await connection.ExecuteAsync(sql, product);

            // Si rowsAffected es mayor a 0, significa que la actualización fue exitosa.
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = "DELETE FROM Products WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}