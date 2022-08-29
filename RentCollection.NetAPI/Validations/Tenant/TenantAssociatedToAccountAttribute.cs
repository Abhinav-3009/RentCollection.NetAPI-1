using System;
using Microsoft.AspNetCore.Http;
using RentCollection.NetAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Linq;

namespace RentCollection.NetAPI.Validations.Tenant
{
    public class TenantAssociatedToAccountAttribute : ValidationAttribute
    {
        private RentCollectionContext db = new RentCollectionContext();

        private int UserId;


        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.GetService(typeof(IHttpContextAccessor));
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());

            int tenantId = Convert.ToInt32(value.ToString());
            var tenants = db.Tenants.ToList();
            var tenant = (from t in tenants where t.TenantId == tenantId && t.UserId == this.UserId && t.IsDeleted == false select t).FirstOrDefault();

            if (tenant != null)
                return ValidationResult.Success;

            return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });

        }
    }
}

