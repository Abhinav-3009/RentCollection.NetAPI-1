using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.RecordAccessibility;
using System.Security.Claims;
using RentCollection.NetAPI.ServiceInterface;
using RentCollection.NetAPI.ServiceImplementation;

namespace RentCollection.NetAPI.Validations.Invoice
{
    public class InvoiceAssociatedWithAccountAttribute : ValidationAttribute
    {

        private IInvoiceRepository InvoiceRepository;

        public InvoiceAssociatedWithAccountAttribute()
        {
            this.InvoiceRepository = new InvoiceRepository(new RentCollectionContext());
        }

        private int UserId;

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.GetService(typeof(IHttpContextAccessor));
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());

            //RentCollection.NetAPI.Models.Allocation allocation = db.Allocations.Find(allocationId);
            //if (allocation == null)
            //    return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });

            //int rentalId = allocation.RentalId;
            //int tenantId = allocation.TenantId;

            //if (!RentalAccess.Check(rentalId, this.UserId) || !TenantAccess.Check(tenantId, this.UserId) || !allocation.IsActive || allocation.IsDeleted)
            //    return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });

            return ValidationResult.Success;
        }
    }
}

