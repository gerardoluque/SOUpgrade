using MediatR;
using SOUpgrade.Application.Common.DTOs;
using SOUpgrade.Domain.Enums;

namespace SOUpgrade.Application.Features.ServiceOrders.Commands.UpdateServiceOrderStatus;

public record UpdateServiceOrderStatusCommand(Guid Id, ServiceOrderStatus Status) : IRequest<ServiceOrderDto?>;
