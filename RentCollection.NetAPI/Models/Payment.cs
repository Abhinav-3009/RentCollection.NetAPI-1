using System;
using System.Collections.Generic;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int InvoiceId { get; set; }
        public int ModeOfPaymentId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual ModeOfPayment ModeOfPayment { get; set; }
    }
}
