using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class Document
    {
        public int DocumentId { get; set; }

        [Required(ErrorMessage = "Tenant required")]
        public int TenantId { get; set; }

        [Required(ErrorMessage = "Document type required")]
        public int DocumentTypeId { get; set; }

        [Required(ErrorMessage = "Document name required")]
        public string DocumentName { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
