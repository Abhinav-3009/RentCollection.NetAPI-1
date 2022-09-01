using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class AutomatedRaisedPayment
    {
        public int AutomatedRaisedPaymentId { get; set; }

        [Required(ErrorMessage = "Allocation Id required")]
        public int AllocationId { get; set; }

        [Required(ErrorMessage = "Invoice item category required")]
        public int InvoiceItemCategoryId { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Amount required")]
        public double Amount { get; set; }

        public virtual Allocation Allocation { get; set; }
    }
}
