using System;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IPaymentRepository : IDisposable
    {
        void Add(Payment payment);
        void Delete(int paymentId);
    }
}

