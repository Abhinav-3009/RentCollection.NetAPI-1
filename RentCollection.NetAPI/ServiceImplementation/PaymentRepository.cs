using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class PaymentRepository : IPaymentRepository, IDisposable
    {
        private RentCollectionContext Context;

        public PaymentRepository(RentCollectionContext context)
        {
            this.Context = context;
        }

        public void Add(Payment payment)
        {
            this.Context.Payments.Add(payment);
            this.Context.SaveChanges();
        }

        public void Delete(int paymentId)
        {
            Payment payment = this.Context.Payments.Find(paymentId);
            this.Context.Payments.Remove(payment);
            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

