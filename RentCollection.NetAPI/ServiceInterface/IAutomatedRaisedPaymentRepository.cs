using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IAutomatedRaisedPaymentRepository : IDisposable
    {
        void Add(AutomatedRaisedPayment automatedRaisedPayment);
        void Delete(int automatedRaisedPaymentId);
        void Update(AutomatedRaisedPaymentUpdate automatedRaisedPaymentUpdate);
        AutomatedRaisedPayment Find(int automatedRaisedPaymentId);
    }
}

