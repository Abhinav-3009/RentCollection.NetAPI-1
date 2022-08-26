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

        [Required(ErrorMessage = "Full Name required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username required")]
        [UniqueUsernameAttribute(ErrorMessage = "Username is already taken")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Minimum length should be 6, Maximum 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Contact required")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Role required")]
        public string Role { get; set; }

        public virtual ICollection<DocumentType> DocumentTypes { get; set; }
        public virtual ICollection<InvoiceItemCategory> InvoiceItemCategories { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<Tenant> Tenants { get; set; }
    }
}
