using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class TenantRepository : ITenantRepository, IDisposable
    {
        private RentCollectionContext context;

        public TenantRepository(RentCollectionContext context)
        {
            this.context = context;
        }

        public void Add(Tenant tenant)
        {
            this.context.Tenants.Add(tenant);
            this.context.SaveChanges();
        }

        public void Delete(int tenantId)
        {
            Tenant tenant = this.context.Tenants.Find(tenantId);
            tenant.IsDeleted = true;
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Tenant Get(int tenantId)
        {
            Tenant tenant = this.context.Tenants.Find(tenantId);
            return tenant;
        }

        public void Update(Tenant tenant)
        {
            tenant.IsDeleted = false;
            this.context.Tenants.Update(tenant);
            this.context.SaveChanges();
        }
    }
}

