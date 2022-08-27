using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.RecordAccessibility;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AllocationController : ControllerBase
    {
        private readonly RentCollectionContext db = new RentCollectionContext();

        private int UserId;

        public AllocationController(IHttpContextAccessor httpContextAccessor)
        {
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Allocate")]
        public IActionResult Allocate(Allocation allocation)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                if (!RentalAccess.Check(allocation.RentalId, this.UserId))
                    return BadRequest(new { error = "Rental record is not associated with your account" });

                if (!TenantAccess.Check(allocation.TenantId, this.UserId))
                    return BadRequest(new { error = "Tenant record is not associated with your account" });

                if(!AllocationAccess.Check(allocation.TenantId, allocation.RentalId))
                    return BadRequest(new { error = "Tenant or Rental is already allocated" });

                allocation.IsActive = true;
                db.Allocations.Add(allocation);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while allocation process", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Allocation done successfully", allocation = allocation });
        }

        [HttpPut]
        [Route("Deallocate")]
        public IActionResult Deallocate(Deallocate deallocate)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {

                if (!RentalAccess.Check(deallocate.RentalId, this.UserId))
                    return BadRequest(new { error = "Rental record is not associated with your account" });

                if (!TenantAccess.Check(deallocate.TenantId, this.UserId))
                    return BadRequest(new { error = "Tenant record is not associated with your account" });


                var allocations = db.Allocations.ToList();
                Allocation allocation = (from a in allocations where a.RentalId == deallocate.RentalId && a.TenantId == deallocate.TenantId select a).FirstOrDefault();

                if (allocation == null)
                    return NotFound(new { error = "Allocation not found" });

                // Check for any outstanding invoice.
                // Deallocate the rental and tenant once all invoices have been settled.

                allocation.IsActive = false;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while deallocation process", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Deallocation done successfully", allocation = deallocate });
        }
    }
}