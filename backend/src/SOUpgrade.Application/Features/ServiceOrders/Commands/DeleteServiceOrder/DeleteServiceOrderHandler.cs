using MediatR;
using SOUpgrade.Domain.Interfaces;

namespace SOUpgrade.Application.Features.ServiceOrders.Commands.DeleteServiceOrder;

public class DeleteServiceOrderHandler : IRequestHandler<DeleteServiceOrderCommand, bool>
{
    private readonly IServiceOrderRepository _repository;

    public DeleteServiceOrderHandler(IServiceOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing is null) return false;

        await _repository.DeleteAsync(request.Id);
        return true;
    }
}
