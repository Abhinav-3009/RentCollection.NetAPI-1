using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Document type name required")]
        public string Code { get; set; }

        public virtual User User { get; set; }
    }
}
