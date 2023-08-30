using AutoMapper;
using Inventory.Product.API.Entities;
using Shared.DTOs.Inventory;

namespace Inventory.Product.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InventoryEntry, InventoryEntryDto>().ReverseMap();
            CreateMap<PurchaseProductDto, InventoryEntryDto>();
        }
    }
}
