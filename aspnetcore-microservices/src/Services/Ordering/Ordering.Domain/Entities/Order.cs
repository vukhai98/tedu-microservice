﻿using Contracts.Common.Events;
using Contracts.Domains;
using Ordering.Domain.OrderAggregate.Events;
using Shared.Enums.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class Order : AuditableEventEntity<long>
    {
        [Required]
        public string UserName { get; set; }

        public Guid DocumentNo { get; set; } = new Guid();

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string ShippingAddress { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string InvoiceAddress { get; set; }

        public EOrderStatus Status { get; set; }

        public Order AddedOrder()
        {
            AddDomainEvent(new OrderCreatedEvent(Id, UserName, DocumentNo.ToString(), TotalPrice, EmailAddress, ShippingAddress, InvoiceAddress));
            return this;
        }

        public Order DeletedOrder()
        {
            AddDomainEvent(new OrderDeletedEvent(Id));
            return this;
        }
    }
}
