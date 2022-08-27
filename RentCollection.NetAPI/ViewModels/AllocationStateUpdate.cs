using System;
using System.ComponentModel.DataAnnotations;

namespace RentCollection.NetAPI.ViewModels
{
    public class AllocationStateUpdate
    {
        [Required(ErrorMessage = "Rental Id required")]
        public int RentalId { get; set; }

        [Required(ErrorMessage = "Tenant Id required")]
        public int TenantId { get; set; }
    }
}

