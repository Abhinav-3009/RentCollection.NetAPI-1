using System;
using System.Collections.Generic;

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
        public string Title { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Allocation> Allocations { get; set; }
        public virtual ICollection<ElectricityMeterReading> ElectricityMeterReadings { get; set; }
    }
}
