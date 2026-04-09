using MediatR;
using SOUpgrade.Application.Common.DTOs;
using SOUpgrade.Domain.Enums;

namespace SOUpgrade.Application.Features.ServiceOrders.Commands.CreateServiceOrder;

public record CreateServiceOrderCommand(
    string Title,
    string Description,
    Priority Priority,
    string ClientName,
    string ClientEmail,
    string ClientPhone,
    string AssignedTo,
    DateTime? EstimatedCompletionDate,
    string Notes,
    decimal Cost
) : IRequest<ServiceOrderDto>;
