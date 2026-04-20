using AutoMapper;
using MediatR;
using SOUpgrade.Application.Common.DTOs;
using SOUpgrade.Domain.Interfaces;

namespace SOUpgrade.Application.Features.ServiceOrders.Queries.GetServiceOrderById;

public class GetServiceOrderByIdHandler : IRequestHandler<GetServiceOrderByIdQuery, ServiceOrderDto?>
{
    private readonly IServiceOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetServiceOrderByIdHandler(IServiceOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceOrderDto?> Handle(GetServiceOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.Id);
        return order is null ? null : _mapper.Map<ServiceOrderDto>(order);
    }
}
