using System;
using System.Collections.Generic;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IInvoiceRepository : IDisposable
    {
        Invoice Create(Invoice invoice);

        Invoice GetInvoice(int invoiceId);

        List<Invoice> GetAllInvoices(int allocationId);

        void Delete(int invoiceId);
    }
}

