using AutoMapper;
using MediatR;
using SOUpgrade.Application.Common.DTOs;
using SOUpgrade.Domain.Interfaces;

namespace SOUpgrade.Application.Features.ServiceOrders.Commands.UpdateServiceOrder;

public class UpdateServiceOrderHandler : IRequestHandler<UpdateServiceOrderCommand, ServiceOrderDto?>
{
    private readonly IServiceOrderRepository _repository;
    private readonly IMapper _mapper;

    public UpdateServiceOrderHandler(IServiceOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceOrderDto?> Handle(UpdateServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing is null) return null;

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.Priority = request.Priority;
        existing.ClientName = request.ClientName;
        existing.ClientEmail = request.ClientEmail;
        existing.ClientPhone = request.ClientPhone;
        existing.AssignedTo = request.AssignedTo;
        existing.EstimatedCompletionDate = request.EstimatedCompletionDate;
        existing.Notes = request.Notes;
        existing.Cost = request.Cost;
        existing.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(existing);
        return _mapper.Map<ServiceOrderDto>(updated);
    }
}
