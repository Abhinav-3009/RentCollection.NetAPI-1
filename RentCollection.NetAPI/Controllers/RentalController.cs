using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.RecordAccessibility;
using RentCollection.NetAPI.Security;
using RentCollection.NetAPI.ServiceImplementation;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private IRentalRepository RentalRepository;

        private int UserId;

        public RentalController(IHttpContextAccessor httpContextAccessor)
        {
            this.RentalRepository = new RentalRepository(new RentCollectionContext());
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(Rental rental)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                rental.UserId = this.UserId;
                this.RentalRepository.Add(rental);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while adding rental", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Rental added successfully", rental = rental });
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(Rental rental)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                if (!RentalAccess.Check(rental.RentalId, this.UserId))
                    return Unauthorized("Rental record is not associated with your account");

                rental.UserId = this.UserId;
                this.RentalRepository.Add(rental);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while updating rental", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Rental updated successfully", rental = rental });
        }

        [HttpDelete]
        [Route("Delete/{rentalId}")]
        public IActionResult Delete(int rentalId)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                if (!RentalAccess.Check(rentalId, this.UserId))
                    return Unauthorized("Rental record is not associated with your account");

                if (!AllocationAccess.CheckIfAllocationEntityAreOccupied(0, rentalId))
                    return Unauthorized("Allocated rental can not be deleted");

                this.RentalRepository.Delete(rentalId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while deleting rental", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Rental deleted successfully" });
        }
        [HttpGet]
        [Route("Get/{rentalId}")]
        public IActionResult Get(int rentalId)
        {
            Rental rental = new Rental();
            try
            {
                if (!RentalAccess.Check(rentalId, this.UserId))
                    return Unauthorized("Rental record is not associated with your account");

                rental = this.RentalRepository.Get(rentalId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while fetching rental", exceptionMessage = e.Message });
            }
            return Ok(new { success = "Rental fetched successfully", rental = rental });
        }


        [HttpGet]
        [Route("Get")]
        public IActionResult GetAll()
        {
            List<Rental> rentals = new List<Rental>();
            try
            {
                rentals = this.RentalRepository.GetAll(this.UserId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while fetching rentals", exceptionMessage = e.Message });
            }
            return Ok(new { success = "Rentals fetched successfully", rentals = rentals });
        }
    }
}