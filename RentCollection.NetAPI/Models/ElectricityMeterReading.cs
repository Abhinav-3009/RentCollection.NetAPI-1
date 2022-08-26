using System;
using System.Collections.Generic;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class ElectricityMeterReading
    {
        public int MeterReadingId { get; set; }
        public int RentalId { get; set; }
        public int Units { get; set; }
        public DateTime TakenOn { get; set; }

        public virtual Rental Rental { get; set; }
    }
}
