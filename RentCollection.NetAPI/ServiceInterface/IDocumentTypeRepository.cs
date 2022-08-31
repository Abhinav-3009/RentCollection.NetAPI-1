using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IDocumentTypeRepository
    {
        void Add(DocumentType documentType);

        void Update(DocumentTypeUpdate documentTypeUpdate);

        void Delete(int documentTypeId);

        bool Used(int documentTypeId);
    }
}

