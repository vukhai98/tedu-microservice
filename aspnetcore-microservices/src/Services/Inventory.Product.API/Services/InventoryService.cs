using AutoMapper;
using Inventory.Product.API.Entities;
using Inventory.Product.API.Repositories;
using Inventory.Product.API.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Configurations;
using Shared.DTOs.Inventory;

namespace Inventory.Product.API.Services
{
    public class InventoryService : MongoDbRepository<InventoryEntry>, IInventoryService
    {
        private readonly IMapper _mapper;
        public InventoryService(IMongoClient client, MongoDBSettings settings) : base(client, settings)
        {
        }

        public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await FindAll().Find(x => x.ItemNo.Equals(itemNo)).ToListAsync();

            var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(entities);

            return result;
        }

        public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoPaggingAsync(GetInventoryPagingQuery query)
        {
            var filterSearchContent = Builders<InventoryEntry>.Filter.Empty;

            var filterItemNo = Builders<InventoryEntry>.Filter.Eq(x => x.ItemNo, query.ItemNo());

            if (!string.IsNullOrEmpty(query.SearchContent))
            {
                filterSearchContent = Builders<InventoryEntry>.Filter.Eq(x => x.DocumentNo, query.SearchContent);
            }

            var andFilter = filterItemNo & filterSearchContent;

            var pagedList = await Collection.Find(andFilter).Skip((query._pageNumber - 1) * query._pageSize)
                                                             .Limit(query._pageSize)
                                                             .ToListAsync();

            var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(pagedList);

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
