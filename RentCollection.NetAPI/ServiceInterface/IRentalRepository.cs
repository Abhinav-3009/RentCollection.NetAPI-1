using System;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IRentalRepository : IDisposable
    {
        void Add(Rental rental);
        void Delete(int rentalId);
        Rental Get(int rentalId);
        void Update(Rental rental);

    }
}

