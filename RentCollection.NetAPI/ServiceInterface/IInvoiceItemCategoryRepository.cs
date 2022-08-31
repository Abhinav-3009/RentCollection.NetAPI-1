using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IInvoiceItemCategoryRepository
    {
        void Add(InvoiceItemCategory invoiceItemCategory);

        void Update(InvoiceItemCategoryUpdate invoiceItemCategoryUpdate);

        void Delete(int invoiceItemCategoryId);

        int GetInvoiceItemCategoryIdByCode(string code, int userId);
    }
}

