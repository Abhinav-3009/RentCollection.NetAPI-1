using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IAllocationRepository: IDisposable
    {
        void Allocate(Allocation allocation);
        void Reallocate(int allocationId);
        void Deallocate(int allocationId);
        void Delete(int allocationId);
        Allocation Find(int allocationId);
        Allocation GetAllocationByRentalId(int rentalId);
        Allocation GetAllocationByTenantId(int tenantId);
        Allocation Get(int allocationId);
    }
}

