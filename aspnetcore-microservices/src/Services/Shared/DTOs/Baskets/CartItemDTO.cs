using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Baskets
{
    public class CartItemDTO
    {
        [Required]
        [Range(1, double.PositiveInfinity, ErrorMessage = "The field {0} must be >= {1}.")]
        public int Quantity { get; set; }
        [Required]
        [Range(0.1, double.PositiveInfinity, ErrorMessage = "The field {0} must be >= {1}.")]
        public decimal ItemPrice { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }

        public int AvaliableQuantity { get; set; }

        public void SetAvaliableItemPrice(int stockQuantity)
        {
            AvaliableQuantity = stockQuantity;
        }
    }
}
