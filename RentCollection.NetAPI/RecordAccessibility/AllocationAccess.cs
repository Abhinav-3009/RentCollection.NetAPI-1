using System;
using System.Linq;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.RecordAccessibility
{
    public class AllocationAccess
    {
        private static readonly RentCollectionContext db = new RentCollectionContext();

        public static bool CheckIfAllocationEntityAreOccupied(int tenantId, int rentalId)
        {
            var allocations = db.Allocations.ToList();
            var allocation = (from a in allocations where (a.TenantId == tenantId || a.RentalId == rentalId) && a.IsActive == true select a).FirstOrDefault();

            return allocation == null ? true : false;
        }

        public static bool CheckIfAllocationExists(int tenantId, int rentalId)
        {
            var allocations = db.Allocations.ToList();
            var allocation = (from a in allocations where (a.TenantId == tenantId && a.RentalId == rentalId) && a.IsActive == false && a.IsDeleted == false select a).FirstOrDefault();

            return allocation != null ? true : false;
        }
    }
}

