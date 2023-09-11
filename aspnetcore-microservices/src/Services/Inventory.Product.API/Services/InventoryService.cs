using AutoMapper;
using Infrastructure.Extensions;
using Inventory.Product.API.Entities;
using Inventory.Product.API.Repositories;
using Inventory.Product.API.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Configurations;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventory.Product.API.Services
{
    public class InventoryService : MongoDbRepository<InventoryEntry>, IInventoryService
    {
        private readonly IMapper _mapper;
        public InventoryService(IMongoClient client, MongoDBSettings settings, IMapper mapper) : base(client, settings)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await FindAll().Find(x => x.ItemNo.Equals(itemNo)).ToListAsync();

            //if (entities.Count == 0) return null;
            
            var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(entities);

            return result;
        }

        public async Task<PageList<InventoryEntryDto>> GetAllByItemNoPaggingAsync(GetInventoryPagingQuery query)
        {
            var filterSearchContent = Builders<InventoryEntry>.Filter.Empty;

            var filterItemNo = Builders<InventoryEntry>.Filter.Eq(x => x.ItemNo, query.ItemNo());

            if (!string.IsNullOrEmpty(query.SearchContent))
            {
                filterSearchContent = Builders<InventoryEntry>.Filter.Eq(x => x.DocumentNo, query.SearchContent);
            }

            var andFilter = filterItemNo & filterSearchContent;

            var pagedList = await Collection.PaginatedListAsync(andFilter, query._pageNumber, query._pageSize);

            var items= _mapper.Map<IEnumerable<InventoryEntryDto>>(pagedList);

            var result = new PageList<InventoryEntryDto>(items, pagedList.GetMetaData().TotalItems,query._pageNumber,query._pageNumber);

            return result;
        }

        public async Task<InventoryEntryDto> GetById(string id)
        {
            var filter = Builders<InventoryEntry>.Filter.Eq(x => x.Id, id);

            var entity = await FindAll().Find(filter).FirstOrDefaultAsync();

            var result = _mapper.Map<InventoryEntryDto>(entity);

            return result;
        }

        public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto request)
        {
            var entity = new InventoryEntry(ObjectId.GenerateNewId().ToString())
            {
                Quantity = request.Quantity,
                ItemNo = itemNo,
                DocumentType = request.DocumentType
            };

            await CreatAsync(entity);

            return _mapper.Map<InventoryEntryDto>(entity);
        }
    }
}
