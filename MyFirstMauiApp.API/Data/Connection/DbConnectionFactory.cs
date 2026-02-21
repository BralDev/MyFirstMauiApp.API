using System.Data;
using Microsoft.Data.SqlClient;

namespace MyFirstMauiApp.API.Data.Connection
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        // Inyectamos IConfiguration para leer el appsettings.json
        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("La cadena de conexión no fue encontrada.");
        }

        // Método que devolverá un instancia de conexión SQL lista para abrirse
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }
}
