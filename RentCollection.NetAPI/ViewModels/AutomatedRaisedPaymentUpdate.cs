using System;
using RentCollection.NetAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace RentCollection.NetAPI.ViewModels
{
    public class AutomatedRaisedPaymentUpdate
    {
        [Required(ErrorMessage = "Automated raised payment id required")]
        public int AutomatedRaisedPaymentId { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Amount required")]
        public double Amount { get; set; }

        public virtual Allocation Allocation { get; set; }
    }
}

