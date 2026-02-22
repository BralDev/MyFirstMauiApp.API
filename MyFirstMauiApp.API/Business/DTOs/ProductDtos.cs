namespace MyFirstMauiApp.API.Business.DTOs
{    
    public record ProductCreateDto(string Name, string? Description, decimal Price, int Stock);
 
    public record ProductUpdateDto(int Id, string Name, string? Description, decimal Price, int Stock);
   
    public record ProductResponseDto(int Id, string Name, string? Description, decimal Price, int Stock, DateTime CreatedAt);
}