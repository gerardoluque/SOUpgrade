using AutoMapper;
using MediatR;
using SOUpgrade.Application.Common.DTOs;
using SOUpgrade.Domain.Enums;
using SOUpgrade.Domain.Interfaces;

namespace SOUpgrade.Application.Features.ServiceOrders.Commands.UpdateServiceOrderStatus;

public class UpdateServiceOrderStatusHandler : IRequestHandler<UpdateServiceOrderStatusCommand, ServiceOrderDto?>
{
    private readonly IServiceOrderRepository _repository;
    private readonly IMapper _mapper;

    public UpdateServiceOrderStatusHandler(IServiceOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceOrderDto?> Handle(UpdateServiceOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing is null) return null;

        existing.Status = request.Status;
        existing.UpdatedAt = DateTime.UtcNow;

        if (request.Status == ServiceOrderStatus.Completed)
            existing.CompletedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(existing);
        return _mapper.Map<ServiceOrderDto>(updated);
    }
}
