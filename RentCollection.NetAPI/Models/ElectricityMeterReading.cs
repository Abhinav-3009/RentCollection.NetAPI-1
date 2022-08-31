using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RentCollection.NetAPI.Validations.Rental;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class ElectricityMeterReading
    {
        public int MeterReadingId { get; set; }

        [Required(ErrorMessage = "Rental id required")]
        [RentalAssociatedWithAccountAttribute(ErrorMessage = "Rental record is not associated with your account")]
        public int RentalId { get; set; }

        [Required(ErrorMessage = "Units required")]
        public int Units { get; set; }
        [Required(ErrorMessage = "Taken on date required")]
        public DateTime TakenOn { get; set; }

        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Generate bill state required")]
        public bool GenerateBill { get; set; }

        [Required(ErrorMessage = "Charges per unit required")]
        public int Charges { get; set; }

        public virtual Rental Rental { get; set; }
    }
}
