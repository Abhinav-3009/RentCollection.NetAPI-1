using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class InvoiceRepository: IInvoiceRepository, IDisposable
    {
        private RentCollectionContext context;

        public InvoiceRepository(RentCollectionContext context)
        {
            this.context = context;
        }

        public Invoice Create(Invoice invoice)
        {
            this.context.Invoices.Add(invoice);
            this.context.SaveChanges();
            return invoice;
        }

        public void Delete(int invoiceId)
        {
            Invoice invoice = this.context.Invoices.Find(invoiceId);
            this.context.Invoices.Remove(invoice);
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<Invoice> GetAllInvoices(int allocationId)
        {
            throw new NotImplementedException();
        }

        public Invoice GetInvoice(int invoiceId)
        {
            Invoice invoice = this.context.Invoices.Include("InvoiceItems").Where(i => i.InvoiceId == invoiceId).FirstOrDefault();
            return invoice;
        }
    }
}

