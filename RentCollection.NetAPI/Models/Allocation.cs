using System;
using System.Collections.Generic;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class Allocation
    {
        public Allocation()
        {
            AutomatedRaisedPayments = new HashSet<AutomatedRaisedPayment>();
            Invoices = new HashSet<Invoice>();
        }

        public int AllocationId { get; set; }
        public int RentalId { get; set; }
        public int TenantId { get; set; }
        public DateTime? AllocatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Rental Rental { get; set; }
        public virtual ICollection<AutomatedRaisedPayment> AutomatedRaisedPayments { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
