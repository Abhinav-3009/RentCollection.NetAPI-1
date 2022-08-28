using System;
using System.Linq;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.RecordAccessibility
{
    public class RentalAccess
    {
        private static readonly RentCollectionContext db = new RentCollectionContext();

        public static bool Check(int rentalId, int userId)
        {
            var rentals = db.Rentals.ToList();
            var rental = (from r in rentals where r.RentalId == rentalId && r.UserId == userId && r.IsDeleted == false select r).FirstOrDefault();

            return rental != null ? true : false;
        }
    }
}

