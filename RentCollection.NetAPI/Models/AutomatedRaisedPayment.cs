using System;
using System.Collections.Generic;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class AutomatedRaisedPayment
    {
        public int AutomatedRaisedPaymentId { get; set; }
        public int AllocationId { get; set; }
        public int InvoiceItemCategoryId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public virtual Allocation Allocation { get; set; }
    }
}
