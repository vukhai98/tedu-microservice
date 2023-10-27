using AutoMapper;
using EventBus.Messages.IntegrationEvents.Events;
using Infrastructure.Mappings;
using Ordering.Application.Features.V1.Orders.Commands.Create;
using Ordering.Application.Features.V1.Orders.Commands.Update;
using Ordering.Application.Features.V1.Orders.Common;
using Ordering.Domain.Entities;
using Shared.DTOs.Orders;

namespace Ordering.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<CreateOrderDto, CreateOrderCommand>().ReverseMap();

            CreateMap<CreateOrderCommand, Order>().ReverseMap();

            CreateMap<BasketCheckoutEvent, CreateOrderCommand>().ReverseMap();

            CreateMap<CreateOrUpdateCommand, Order>();

            CreateMap<UpdateOrderCommand, Order>()
                       .ForMember(dest => dest.Status, opts => opts.Ignore()).IgnoreAllNonExisting();

        }
    }
}
