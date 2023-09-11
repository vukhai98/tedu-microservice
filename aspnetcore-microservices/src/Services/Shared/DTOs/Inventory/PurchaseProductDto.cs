using Shared.Enums.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs.Inventory
{
    public class PurchaseProductDto
    {
        public EDocumentType DocumentType => EDocumentType.Purchase;

        [JsonIgnore]
        public string ItemNo { get; set; }

        public string DocumentNo { get; set; }

        public int Quantity { get; set; }
    }
}
