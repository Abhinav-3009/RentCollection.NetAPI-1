using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly RentCollectionContext db = new RentCollectionContext();

        private int UserId;

        public RentalController(IHttpContextAccessor httpContextAccessor)
        {
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(Rental rental)
        {
           //this.UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                rental.UserId = this.UserId;
                db.Rentals.Add(rental);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while adding rental", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Rental added successfully", rental = rental });
        }
    }
}