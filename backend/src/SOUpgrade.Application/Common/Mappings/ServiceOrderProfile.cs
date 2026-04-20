using AutoMapper;
using SOUpgrade.Application.Common.DTOs;
using SOUpgrade.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using SOUpgrade.Application.Features.ServiceOrders.Commands.UpdateServiceOrder;
using SOUpgrade.Domain.Entities;

namespace SOUpgrade.Application.Common.Mappings;

public class ServiceOrderProfile : Profile
{
    public ServiceOrderProfile()
    {
        CreateMap<ServiceOrder, ServiceOrderDto>();
        CreateMap<CreateServiceOrderCommand, ServiceOrder>();
        CreateMap<UpdateServiceOrderCommand, ServiceOrder>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());
    }
}
