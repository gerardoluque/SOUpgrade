using MediatR;
using SOUpgrade.Application.Common.DTOs;

namespace SOUpgrade.Application.Features.ServiceOrders.Queries.GetServiceOrderById;

public record GetServiceOrderByIdQuery(Guid Id) : IRequest<ServiceOrderDto?>;
