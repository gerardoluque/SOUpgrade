using AutoMapper;
using MediatR;
using SOUpgrade.Application.Common.DTOs;
using SOUpgrade.Domain.Interfaces;

namespace SOUpgrade.Application.Features.ServiceOrders.Queries.GetAllServiceOrders;

public class GetAllServiceOrdersHandler : IRequestHandler<GetAllServiceOrdersQuery, IEnumerable<ServiceOrderDto>>
{
    private readonly IServiceOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetAllServiceOrdersHandler(IServiceOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceOrderDto>> Handle(GetAllServiceOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ServiceOrderDto>>(orders);
    }
}
