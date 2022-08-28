using System;
using System.Collections.Generic;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
        }

        public int InvoiceId { get; set; }
        public int AllocationId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Allocation Allocation { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
