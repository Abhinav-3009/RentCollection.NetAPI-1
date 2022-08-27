using System;
using System.Linq;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.RecordAccessibility
{
    public class TenantAccess
    {
        private static readonly RentCollectionContext db = new RentCollectionContext();

        public static bool Check(int tenantId, int userId)
        {
            var tenants = db.Tenants.ToList();
            var tenant = (from t in tenants where t.TenantId == tenantId && t.UserId == userId select t).FirstOrDefault();

            return tenant != null ? true : false;
        }
    }
}

