using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Inventory
{
    public class CreatedSaleOrderSuccesDTO
    {
        public string DocumentNo { get; }

        public CreatedSaleOrderSuccesDTO(string documentNo)
        {
            DocumentNo = documentNo;
        }
    }
}
