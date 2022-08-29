using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RentCollection.NetAPI.Validations.User;

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

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [UniqueUsernameAttribute(ErrorMessage = "Username is already taken")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        public virtual ICollection<DocumentType> DocumentTypes { get; set; }
        public virtual ICollection<InvoiceItemCategory> InvoiceItemCategories { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<Tenant> Tenants { get; set; }
    }
}
