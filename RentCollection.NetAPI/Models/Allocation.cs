using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RentCollection.NetAPI.Validations.Rental;
using RentCollection.NetAPI.Validations.Tenant;

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

        [RentalAssociatedWithAccountAttribute(ErrorMessage = "Rental record is not associated with your account")]
        public int RentalId { get; set; }

        [TenantAssociatedWithAccountAttribute(ErrorMessage = "Tenant record is not associated with your account")]
        public int TenantId { get; set; }

        [Required(ErrorMessage = "Allocation Date is required")]
        public DateTime? AllocatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Rental Rental { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<AutomatedRaisedPayment> AutomatedRaisedPayments { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
