using AutoMapper;
using Basket.API.Entities;
using Shared.DTOs.Baskets;
using Shared.DTOs.Orders;

namespace Saga.Orchestrator.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOrderDto, BasketCheckoutDto>().ReverseMap();
        }
    }
}
