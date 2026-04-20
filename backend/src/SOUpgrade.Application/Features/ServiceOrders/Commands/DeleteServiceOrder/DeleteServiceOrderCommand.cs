using MediatR;

namespace SOUpgrade.Application.Features.ServiceOrders.Commands.DeleteServiceOrder;

public record DeleteServiceOrderCommand(Guid Id) : IRequest<bool>;
