using SOUpgrade.Domain.Entities;
using SOUpgrade.Domain.Enums;

namespace SOUpgrade.Domain.Interfaces;

public interface IServiceOrderRepository
{
    Task<ServiceOrder?> GetByIdAsync(Guid id);
    Task<IEnumerable<ServiceOrder>> GetAllAsync();
    Task<IEnumerable<ServiceOrder>> GetByStatusAsync(ServiceOrderStatus status);
    Task<ServiceOrder> CreateAsync(ServiceOrder serviceOrder);
    Task<ServiceOrder> UpdateAsync(ServiceOrder serviceOrder);
    Task DeleteAsync(Guid id);
    Task<ServiceOrder?> GetByOrderNumberAsync(string orderNumber);
}
