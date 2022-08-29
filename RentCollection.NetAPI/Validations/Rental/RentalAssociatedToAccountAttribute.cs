using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using RentCollection.NetAPI.Models;
using System.Security.Claims;
using System.Linq;

namespace RentCollection.NetAPI.Validations.Rental
{
    public class RentalAssociatedToAccountAttribute : ValidationAttribute
    {
        private RentCollectionContext db = new RentCollectionContext();

        private int UserId;


        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.GetService(typeof(IHttpContextAccessor));
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());


            int rentalId = Convert.ToInt32(value.ToString());
            var rentals = db.Rentals.ToList();
            var rental = (from r in rentals where r.RentalId == rentalId && r.UserId == this.UserId && r.IsDeleted == false select r).FirstOrDefault();

            if (rental != null)
                return ValidationResult.Success;

            return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });

        }
    }
}

