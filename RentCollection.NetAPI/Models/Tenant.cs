using System;
using System.Collections.Generic;

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
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
