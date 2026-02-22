using Dapper;
using MyFirstMauiApp.API.Data.Connection;

namespace MyFirstMauiApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Declaracion e inicialización del builder donde configuro servicios y la inyección de dependencias.
            var builder = WebApplication.CreateBuilder(args);
       
            // Registro de servicios sobre contenedor de inyección de dependencias para habilitar la capa de presentación (Controllers).
            builder.Services.AddControllers();

            // Configuración de Swagger para autogenerar la documentación del API.            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            // Registro de fábrica de conexiones como Singleton (solo necesitamos una instancia en toda la app).
            builder.Services.AddSingleton<DbConnectionFactory>();

            // Construcción de la aplicación, lo que nos da acceso a los servicios registrados y al pipeline de middleware.
            var app = builder.Build();
                      
            // Configuracion de middlewares para manejar las peticiones HTTP. El orden es importante.
            // Si estamos en entorno de desarrollo, mostramos la interfaz gráfica de Swagger.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Redirige automáticamente peticiones HTTP a HTTPS.
            app.UseHttpsRedirection();

            // Middleware para validar permisos/tokens.
            app.UseAuthorization();       

            // Escanea los controladores y mapea sus rutas automáticamente.
            app.MapControllers();

            // Inicia el servidor Kestrel interno y empieza a escuchar peticiones.           
            app.Run();
        }
    }
}
