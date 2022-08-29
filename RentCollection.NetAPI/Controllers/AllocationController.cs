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

                if (!AllocationAccess.CheckIfAllocationEntityAreOccupied(allocation.TenantId, allocation.RentalId))
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
        public IActionResult allocationStateUpdate(AllocationStateUpdate allocationStateUpdate)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {

                var allocations = db.Allocations.ToList();
                Allocation allocation = (from a in allocations where a.RentalId == allocationStateUpdate.RentalId && a.TenantId == allocationStateUpdate.TenantId select a).FirstOrDefault();

                if (allocation == null)
                    return NotFound(new { error = "Allocation not found" });

                //Check for any outstanding invoice.

                //Deallocate the rental and tenant once all invoices have been settled.

               allocation.IsActive = false;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while deallocation process", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Deallocation done successfully", allocation = allocationStateUpdate });
        }

        [HttpPut]
        [Route("Reallocate")]
        public IActionResult Reallocate(AllocationStateUpdate allocationStateUpdate)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {

                if (!AllocationAccess.CheckIfAllocationExists(allocationStateUpdate.TenantId, allocationStateUpdate.RentalId))
                    return NotFound(new { error = "Allocation not found" });

                var allocations = db.Allocations.ToList();
                Allocation allocation = (from a in allocations where a.RentalId == allocationStateUpdate.RentalId && a.TenantId == allocationStateUpdate.TenantId select a).FirstOrDefault();

                if (allocation == null)
                    return NotFound(new { error = "Allocation not found" });

                allocation.IsActive = true;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while reallocation process", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Reallocation done successfully", allocation = allocationStateUpdate });
        }

        [HttpDelete]
        [Route("Delete/{allocationId}")]
        public IActionResult Delete(int allocationId)
        {
            try
            {
                Allocation allocation = db.Allocations.Find(allocationId);
                if (allocation == null)
                    return NotFound(new { error = "Allocation not found" });

                int rentalId = allocation.RentalId;
                int tenantId = allocation.TenantId;

                if (!RentalAccess.Check(rentalId, this.UserId) || !TenantAccess.Check(tenantId, this.UserId))
                    return Unauthorized("Allocation is not associated with your account");

                if (allocation.IsActive)
                    return Unauthorized("Active allocation cannot be deleted, First deallocate the allocation and then delete the allocation");

                allocation.IsDeleted = true;
                db.SaveChanges();

            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while deleting allocation", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Allocation deleted successfully"});

        }
    }
}