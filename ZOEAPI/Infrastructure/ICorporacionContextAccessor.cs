namespace API.Infrastructure
{
    public interface ICorporacionContextAccessor
    {
        string? CorporacionId { get; set; }
        string? SistemaId { get; set; } 
    }
}
