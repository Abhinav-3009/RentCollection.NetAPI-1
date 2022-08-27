using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RentCollection.NetAPI.Validations.Rental;
using RentCollection.NetAPI.Validations.User;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class Rental
    {
        public Rental()
        {
            Allocations = new HashSet<Allocation>();
            ElectricityMeterReadings = new HashSet<ElectricityMeterReading>();
        }

        public int RentalId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Title required")]
        [UniqueRentalTitleAttribute(ErrorMessage = "Title is already taken")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Amount required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid amount")]
        public double Amount { get; set; }

        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Allocation> Allocations { get; set; }
        public virtual ICollection<ElectricityMeterReading> ElectricityMeterReadings { get; set; }
    }
}
