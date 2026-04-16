namespace API.Persistence
{
    public interface ICorporacionDbContextFactory
    {
        Task<CorporacionDbContext> CreateAsync();
    }
}
