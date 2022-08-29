using System;
using Microsoft.AspNetCore.Http;
using RentCollection.NetAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Linq;
using RentCollection.NetAPI.RecordAccessibility;

namespace RentCollection.NetAPI.Validations.Allocation
{
    public class AllocationAssociatedWithAccountAttribute : ValidationAttribute
    {

        private RentCollectionContext db = new RentCollectionContext();

        private int UserId;


        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.GetService(typeof(IHttpContextAccessor));
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());

            int allocationId = Convert.ToInt32(value.ToString());

            RentCollection.NetAPI.Models.Allocation allocation = db.Allocations.Find(allocationId);
            if (allocation == null)
                return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });

            int rentalId = allocation.RentalId;
            int tenantId = allocation.TenantId;

            if (!RentalAccess.Check(rentalId, this.UserId) || !TenantAccess.Check(tenantId, this.UserId) || !allocation.IsActive || allocation.IsDeleted)
                return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });

            return ValidationResult.Success;
        }
    }
}

