namespace API.Infrastructure
{
    public class CorporacionContextAccessor : ICorporacionContextAccessor
    {
        public string? CorporacionId { get; set; }
        public string? SistemaId { get; set; }
    }
}
