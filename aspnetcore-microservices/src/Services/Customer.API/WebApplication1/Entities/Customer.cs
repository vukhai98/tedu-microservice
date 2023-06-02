﻿using Contracts.Domains;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Customer.API.Entities
{
    public class Customer : EntityBase<int>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
