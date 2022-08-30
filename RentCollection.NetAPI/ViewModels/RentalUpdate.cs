using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.Validations.Rental;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentCollection.NetAPI.ViewModels
{
    public class RentalUpdate
    {
        public int RentalId { get; set; }

        [Required(ErrorMessage = "Title required")]
        [UniqueRentalTitleAttribute(ErrorMessage = "Title is already taken")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Amount required")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public double Amount { get; set; }

    }
}

