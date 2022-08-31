using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class InvoiceItemCategory
    {
        public int InvoiceItemCategoryId { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Invoice item category name required")]
        public string Code { get; set; }

        public virtual User User { get; set; }
    }
}
