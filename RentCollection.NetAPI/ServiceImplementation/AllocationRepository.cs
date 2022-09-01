using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class AllocationRepository : IAllocationRepository, IDisposable
    {

        private RentCollectionContext Context;
        public AllocationRepository(RentCollectionContext context)
        {
            this.Context = context;
        }

        public void Allocate(Allocation allocation)
        {
            this.Context.Allocations.Add(allocation);
            this.Context.SaveChanges();
        }

        public void Deallocate(int allocationId)
        {
            Allocation allocation = this.Context.Allocations.Find(allocationId);
            allocation.IsActive = false;
            this.Context.Allocations.Update(allocation);
            this.Context.SaveChanges();
        }

        public void Delete(int allocationId)
        {
            Allocation allocation = this.Context.Allocations.Find(allocationId);
            allocation.IsDeleted = true;
            this.Context.Allocations.Update(allocation);
            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Allocation Find(int allocationId)
        {
            Allocation allocation = this.Context.Allocations.Find(allocationId);
            return allocation;
        }

        public Allocation Get(int allocationId)
        {
            Allocation allocation =  this.Context.Allocations.Include("Rental").Include("Tenant").Where(a => a.AllocationId == allocationId).FirstOrDefault();
            return allocation;
        }

        public Allocation GetAllocationByRentalId(int rentalId)
        {
            Allocation allocation = this.Context.Allocations.Where(a => a.RentalId == rentalId).FirstOrDefault();
            return allocation;
        }

        public Allocation GetAllocationByTenantId(int tenantId)
        {
            throw new NotImplementedException();
        }

        public void Reallocate(int allocationId)
        {
            Allocation allocation = this.Context.Allocations.Find(allocationId);
            allocation.IsActive = true;
            this.Context.Allocations.Update(allocation);
            this.Context.SaveChanges();
        }
    }
}

