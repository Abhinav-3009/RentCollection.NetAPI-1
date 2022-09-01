using System;
using System.Collections.Generic;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IRentalRepository : IDisposable
    {
        void Add(Rental rental);
        void Delete(int rentalId);
        Rental Get(int rentalId);
        void Update(Rental rental);
        List<Rental> GetAll(int userId);
    }
}

