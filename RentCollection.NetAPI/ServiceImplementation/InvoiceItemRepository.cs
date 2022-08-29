using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class InvoiceItemRepository: IInvoiceItemRepository, IDisposable
    {
        private RentCollectionContext context;

        public InvoiceItemRepository(RentCollectionContext context)
        {
            this.context = context;
        }

        public void Add(InvoiceItem invoiceItem)
        {
            this.context.InvoiceItems.Add(invoiceItem);
            this.context.SaveChanges();
        }

        public void Delete(int invoiceItemId)
        {
            InvoiceItem invoiceItem = this.context.InvoiceItems.Find(invoiceItemId);
            this.context.InvoiceItems.Remove(invoiceItem);
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

