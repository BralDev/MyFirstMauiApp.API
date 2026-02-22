namespace MyFirstMauiApp.API.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        // Inicializamos para evitar advertencias de nulos en .NET 8
        public string Name { get; set; } = string.Empty; 
        
        // (?) significa que puede ser nulo en la BD
        public string? Description { get; set; } 
        
        public decimal Price { get; set; }
        
        public int Stock { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}