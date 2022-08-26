using System;
using System.Collections.Generic;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class Document
    {
        public int DocumentId { get; set; }
        public int TenantId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
