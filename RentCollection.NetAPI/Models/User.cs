using System;
using System.Collections.Generic;

#nullable disable

namespace RentCollection.NetAPI.Models
{
    public partial class User
    {
        public User()
        {
            DocumentTypes = new HashSet<DocumentType>();
            InvoiceItemCategories = new HashSet<InvoiceItemCategory>();
            Rentals = new HashSet<Rental>();
            Tenants = new HashSet<Tenant>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string Role { get; set; }

        public virtual ICollection<DocumentType> DocumentTypes { get; set; }
        public virtual ICollection<InvoiceItemCategory> InvoiceItemCategories { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<Tenant> Tenants { get; set; }
    }
}
