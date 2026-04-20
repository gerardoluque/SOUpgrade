using MediatR;
using Microsoft.AspNetCore.Mvc;
using SOUpgrade.Application.Common.DTOs;
using SOUpgrade.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using SOUpgrade.Application.Features.ServiceOrders.Commands.DeleteServiceOrder;
using SOUpgrade.Application.Features.ServiceOrders.Commands.UpdateServiceOrder;
using SOUpgrade.Application.Features.ServiceOrders.Commands.UpdateServiceOrderStatus;
using SOUpgrade.Application.Features.ServiceOrders.Queries.GetAllServiceOrders;
using SOUpgrade.Application.Features.ServiceOrders.Queries.GetServiceOrderById;
using SOUpgrade.Domain.Enums;

namespace SOUpgrade.API.Controllers;

[ApiController]
[Route("api/service-orders")]
[Produces("application/json")]
public class ServiceOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all service orders</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServiceOrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllServiceOrdersQuery());
        return Ok(result);
    }

    /// <summary>Get a service order by ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ServiceOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetServiceOrderByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new service order</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ServiceOrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateServiceOrderDto dto)
    {
        var command = new CreateServiceOrderCommand(
            dto.Title,
            dto.Description,
            dto.Priority,
            dto.ClientName,
            dto.ClientEmail,
            dto.ClientPhone,
            dto.AssignedTo,
            dto.EstimatedCompletionDate,
            dto.Notes,
            dto.Cost
        );
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an existing service order</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ServiceOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateServiceOrderDto dto)
    {
        var command = new UpdateServiceOrderCommand(
            id,
            dto.Title,
            dto.Description,
            dto.Priority,
            dto.ClientName,
            dto.ClientEmail,
            dto.ClientPhone,
            dto.AssignedTo,
            dto.EstimatedCompletionDate,
            dto.Notes,
            dto.Cost
        );
        var result = await _mediator.Send(command);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Delete a service order</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteServiceOrderCommand(id));
        return result ? NoContent() : NotFound();
    }

    /// <summary>Update the status of a service order</summary>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(ServiceOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        var result = await _mediator.Send(new UpdateServiceOrderStatusCommand(id, request.Status));
        return result is null ? NotFound() : Ok(result);
    }
}

public record UpdateStatusRequest(ServiceOrderStatus Status);
