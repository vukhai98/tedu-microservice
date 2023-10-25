using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Baskets
{
    public class CartDTO
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();

        public CartDTO()
        {
        }

        public CartDTO(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice => Items.Sum(x => x.ItemPrice * x.Quantity);

    }
}
