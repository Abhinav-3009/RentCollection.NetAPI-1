using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class Tenant
    {
        public Tenant()
        {
            Documents = new HashSet<Document>();
        }

        public int TenantId { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        public string Contact { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 characters")]
        public string Password { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
