using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class InvoiceItemCategoryRepository: IInvoiceItemCategoryRepository, IDisposable
    {
        private RentCollectionContext Context;
        public InvoiceItemCategoryRepository(RentCollectionContext context)
        {
            this.Context = context;
        }

        public void Add(InvoiceItemCategory invoiceItemCategory)
        {
            this.Context.InvoiceItemCategories.Add(invoiceItemCategory);
            this.Context.SaveChanges();
        }

        public void Delete(int invoiceItemCategoryId)
        {
            InvoiceItemCategory invoiceItemCategory = this.Context.InvoiceItemCategories.Find(invoiceItemCategoryId);
            this.Context.InvoiceItemCategories.Remove(invoiceItemCategory);
            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Update(InvoiceItemCategoryUpdate invoiceItemCategoryUpdate)
        {
            InvoiceItemCategory oldInvoiceItemCategory = this.Context.InvoiceItemCategories.Find(invoiceItemCategoryUpdate.InvoiceItemCategoryId);

            oldInvoiceItemCategory.Code = invoiceItemCategoryUpdate.Code;
            this.Context.InvoiceItemCategories.Update(oldInvoiceItemCategory);
            this.Context.SaveChanges();
        }

    }
}

