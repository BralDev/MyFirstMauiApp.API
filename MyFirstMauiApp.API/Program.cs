using Dapper;
using MyFirstMauiApp.API.Data.Connection;

namespace MyFirstMauiApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Registrar la fábrica de conexiones como Singleton (solo necesitamos una instancia en toda la app)
            builder.Services.AddSingleton<DbConnectionFactory>();

            var app = builder.Build();

            // Verificar la conexión a la base de datos al iniciar la aplicación
            using (var scope = app.Services.CreateScope())
            {
                var factory = scope.ServiceProvider.GetRequiredService<DbConnectionFactory>();
                try
                {
                    using var connection = factory.CreateConnection();
                    connection.Open();

                    // Consulta básica a SQL Server para traer su versión
                    var dbVersion = connection.ExecuteScalar<string>("SELECT @@VERSION");

                    Console.WriteLine("\n=============================================");
                    Console.WriteLine("¡CONEXIÓN EXITOSA A LA BASE DE DATOS!");
                    Console.WriteLine($"Versión:\n{dbVersion}");
                    Console.WriteLine("=============================================\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n=============================================");
                    Console.WriteLine("ERROR AL CONECTAR A LA BASE DE DATOS:");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("=============================================\n");
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
