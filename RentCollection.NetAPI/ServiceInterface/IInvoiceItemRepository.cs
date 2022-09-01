using System;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IInvoiceItemRepository : IDisposable
    {
        void Add(InvoiceItem invoiceItem);
        void Delete(int invoiceItemId);
        public InvoiceItem Get(int invoiceItemId);
    }
}

