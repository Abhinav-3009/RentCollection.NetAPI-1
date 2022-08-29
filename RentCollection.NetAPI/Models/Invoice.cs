using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RentCollection.NetAPI.Validations.Allocation;

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

        [AllocationAssociatedWithAccountAttribute(ErrorMessage = "Allocation not associated with your account or it is inactive")]
        public int AllocationId { get; set; }

        [Required(ErrorMessage = "Invoice Date is required")]
        public DateTime InvoiceDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Allocation Allocation { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
