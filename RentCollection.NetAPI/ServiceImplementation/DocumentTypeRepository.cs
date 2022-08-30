using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class DocumentTypeRepository : IDocumentTypeRepository, IDisposable
    {
        private RentCollectionContext Context;
        public DocumentTypeRepository(RentCollectionContext context)
        {
            this.Context = context;
        }

        public void Add(DocumentType documentType)
        {
            this.Context.DocumentTypes.Add(documentType);
            this.Context.SaveChanges();
        }

        public void Delete(int documentTypeId)
        {
            DocumentType documentType = this.Context.DocumentTypes.Find(documentTypeId);
            this.Context.DocumentTypes.Remove(documentType);
            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Update(DocumentTypeUpdate documentTypeUpdate)
        {

            DocumentType oldDocumentType = this.Context.DocumentTypes.Find(documentTypeUpdate.DocumentTypeId);
            oldDocumentType.Code = documentTypeUpdate.Code;
            this.Context.DocumentTypes.Update(oldDocumentType);
            this.Context.SaveChanges();
        }
    }
}

