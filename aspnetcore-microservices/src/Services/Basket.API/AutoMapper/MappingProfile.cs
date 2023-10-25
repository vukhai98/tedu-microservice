using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.IntegrationEvents.Events;
using Shared.DTOs.Baskets;

namespace Basket.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
            CreateMap<CartDTO, Cart>().ReverseMap();
            CreateMap<CartItemDTO, CartItem>().ReverseMap();
        }
    }
}
