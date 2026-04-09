using Microsoft.EntityFrameworkCore;
using SOUpgrade.Domain.Entities;
using SOUpgrade.Domain.Enums;
using SOUpgrade.Domain.Interfaces;
using SOUpgrade.Infrastructure.Persistence;

namespace SOUpgrade.Infrastructure.Repositories;

public class ServiceOrderRepository : IServiceOrderRepository
{
    private readonly AppDbContext _context;

    public ServiceOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceOrder?> GetByIdAsync(Guid id)
        => await _context.ServiceOrders.FindAsync(id);

    public async Task<IEnumerable<ServiceOrder>> GetAllAsync()
        => await _context.ServiceOrders.OrderByDescending(x => x.CreatedAt).ToListAsync();

    public async Task<IEnumerable<ServiceOrder>> GetByStatusAsync(ServiceOrderStatus status)
        => await _context.ServiceOrders
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public async Task<ServiceOrder> CreateAsync(ServiceOrder serviceOrder)
    {
        _context.ServiceOrders.Add(serviceOrder);
        await _context.SaveChangesAsync();
        return serviceOrder;
    }

    public async Task<ServiceOrder> UpdateAsync(ServiceOrder serviceOrder)
    {
        _context.ServiceOrders.Update(serviceOrder);
        await _context.SaveChangesAsync();
        return serviceOrder;
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _context.ServiceOrders.FindAsync(id);
        if (order is not null)
        {
            _context.ServiceOrders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ServiceOrder?> GetByOrderNumberAsync(string orderNumber)
        => await _context.ServiceOrders
            .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);
}
