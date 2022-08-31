using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Invoice Id required")]
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Mode of Payment required")]
        public int ModeOfPaymentId { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Amount required")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public double Amount { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual ModeOfPayment ModeOfPayment { get; set; }
    }
}
