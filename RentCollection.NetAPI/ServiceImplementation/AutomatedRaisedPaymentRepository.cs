using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class AutomatedRaisedPaymentRepository : IAutomatedRaisedPaymentRepository, IDisposable
    {
        private RentCollectionContext Context;

        public AutomatedRaisedPaymentRepository(RentCollectionContext context)
        {
            this.Context = context;
        }

        public void Add(AutomatedRaisedPayment automatedRaisedPayment)
        {
            InvoiceItemCategory invoiceItemCategory = this.Context.InvoiceItemCategories.Find(automatedRaisedPayment.InvoiceItemCategoryId);
            if (invoiceItemCategory.Code == "Waived Off")
                automatedRaisedPayment.Amount = -automatedRaisedPayment.Amount;
            this.Context.AutomatedRaisedPayments.Add(automatedRaisedPayment);
            this.Context.SaveChanges();
        }

        public void Delete(int automatedRaisedPaymentId)
        {
            AutomatedRaisedPayment automatedRaisedPayment = this.Context.AutomatedRaisedPayments.Find(automatedRaisedPaymentId);
            this.Context.AutomatedRaisedPayments.Remove(automatedRaisedPayment);
            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public AutomatedRaisedPayment Find(int automatedRaisedPaymentId)
        {
            AutomatedRaisedPayment automatedRaisedPayment = this.Context.AutomatedRaisedPayments.Find(automatedRaisedPaymentId);
            return automatedRaisedPayment;
        }

        public void Update(AutomatedRaisedPaymentUpdate automatedRaisedPaymentUpdate)
        {
            AutomatedRaisedPayment automatedRaisedPayment = this.Context.AutomatedRaisedPayments.Find(automatedRaisedPaymentUpdate.AutomatedRaisedPaymentId);

            InvoiceItemCategory invoiceItemCategory = this.Context.InvoiceItemCategories.Find(automatedRaisedPayment.InvoiceItemCategoryId);
            if (invoiceItemCategory.Code == "Waived Off")
                automatedRaisedPaymentUpdate.Amount = -automatedRaisedPaymentUpdate.Amount;

            automatedRaisedPayment.Description = automatedRaisedPaymentUpdate.Description;
            automatedRaisedPayment.Amount = automatedRaisedPaymentUpdate.Amount;
            this.Context.AutomatedRaisedPayments.Update(automatedRaisedPayment);
            this.Context.SaveChanges();
        }
    }
}

