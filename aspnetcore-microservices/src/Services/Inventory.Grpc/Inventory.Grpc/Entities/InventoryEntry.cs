using Contracts.Domains;
using Infrastructure.Extensions;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Enums.Inventory;

namespace Inventory.Grpc.Entities
{
    [BsonCollection("InventoryEntries")]
    public class InventoryEntry : MongoEntity
    {
        //[BsonElement("itemNo")]
        //public string ItemNo { get; set; }

        //[BsonElement("quantity")]
        //public int Quantity { get; set; }
        [BsonElement("documentType")]
        public EDocumentType DocumentType { get; set; }

        [BsonElement("documentNo")]
        public string DocumentNo { get; set; } = new Guid().ToString();

        [BsonElement("itemNo")]
        public string ItemNo { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("externalDocumentNo")]
        public string ExternalDocumentNo { get; set; }
    }
}
