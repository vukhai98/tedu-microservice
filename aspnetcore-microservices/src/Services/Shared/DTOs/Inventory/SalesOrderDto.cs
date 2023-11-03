using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Inventory
{
    public class SalesOrderDto
    {
        public string OrderDocumentNo { get; set; }

        public List<SaleItemDto> SaleItems { get; set; }
    }
}
