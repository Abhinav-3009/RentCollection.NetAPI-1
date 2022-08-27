using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.Validations.Rental
{
    public class UniqueRentalTitleAttribute : ValidationAttribute
    {
        private RentCollectionContext db = new RentCollectionContext();

        private int UserId;


        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.GetService(typeof(IHttpContextAccessor));
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            string title = value.ToString();
            var rentals = db.Rentals.ToList();
            var rental = (from r in rentals where r.Title == title && r.UserId == this.UserId select r).FirstOrDefault();

            if (rental == null)
                return ValidationResult.Success;

            return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });

        }
    }
}

