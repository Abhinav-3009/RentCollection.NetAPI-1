using System;
using System.Collections.Generic;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface ITenantRepository
    {
        void Add(Tenant tenant);
        void Delete(int tenantId);
        Tenant Get(int tenantId);
        void Update(Tenant tenant);
        List<Tenant> GetAll(int userId);
    }
}

