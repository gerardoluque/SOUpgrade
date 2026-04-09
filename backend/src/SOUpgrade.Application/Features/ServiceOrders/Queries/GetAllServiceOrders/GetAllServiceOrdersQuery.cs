using MediatR;
using SOUpgrade.Application.Common.DTOs;

namespace SOUpgrade.Application.Features.ServiceOrders.Queries.GetAllServiceOrders;

public record GetAllServiceOrdersQuery : IRequest<IEnumerable<ServiceOrderDto>>;
