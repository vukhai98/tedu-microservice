using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Products
{
    public class CreateProductDto : CreateOrUpdateDto
    {
        [Required]
        public string No { get; set; }
    }
}
