using System;
using System.ComponentModel.DataAnnotations;
using RentCollection.NetAPI.Validations.Rental;
using RentCollection.NetAPI.Validations.Tenant;

namespace RentCollection.NetAPI.ViewModels
{
    public class AllocationStateUpdate
    {

        [Required(ErrorMessage = "Rental Id required")]
        [RentalAssociatedToAccountAttribute(ErrorMessage = "Rental record is not associated with your account")]
        public int RentalId { get; set; }

        [Required(ErrorMessage = "Tenant Id required")]
        [TenantAssociatedToAccountAttribute(ErrorMessage = "Tenant record is not associated with your account")]
        public int TenantId { get; set; }

    }
}

