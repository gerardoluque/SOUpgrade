using AutoMapper;
using MediatR;
using SOUpgrade.Application.Common.DTOs;
using SOUpgrade.Domain.Entities;
using SOUpgrade.Domain.Enums;
using SOUpgrade.Domain.Interfaces;

namespace SOUpgrade.Application.Features.ServiceOrders.Commands.CreateServiceOrder;

public class CreateServiceOrderHandler : IRequestHandler<CreateServiceOrderCommand, ServiceOrderDto>
{
    private readonly IServiceOrderRepository _repository;
    private readonly IMapper _mapper;

    public CreateServiceOrderHandler(IServiceOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceOrderDto> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var orderNumber = $"SO-{now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

        var serviceOrder = new ServiceOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = orderNumber,
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Status = ServiceOrderStatus.Pending,
            ClientName = request.ClientName,
            ClientEmail = request.ClientEmail,
            ClientPhone = request.ClientPhone,
            AssignedTo = request.AssignedTo,
            EstimatedCompletionDate = request.EstimatedCompletionDate,
            Notes = request.Notes,
            Cost = request.Cost,
            CreatedAt = now,
            UpdatedAt = now
        };

        var created = await _repository.CreateAsync(serviceOrder);
        return _mapper.Map<ServiceOrderDto>(created);
    }
}
