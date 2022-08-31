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
using RentCollection.NetAPI.ServiceImplementation;
using RentCollection.NetAPI.ServiceInterface;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AllocationController : ControllerBase
    {
        private IAllocationRepository AllocationRepository;

        private int UserId;

        public AllocationController(IHttpContextAccessor httpContextAccessor)
        {
            this.AllocationRepository = new AllocationRepository(new RentCollectionContext());
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
                this.AllocationRepository.Allocate(allocation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while allocation process", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Allocation done successfully", allocation = allocation });
        }

        [HttpPut]
        [Route("Deallocate/{allocationId}")]
        public IActionResult allocationStateUpdate(int allocationId)
        {
            try
            {
                // Check if allocation is associated with the account

                // Check for any outstanding invoice.

                // Deallocate the rental and tenant once all invoices have been settled.

                this.AllocationRepository.Deallocate(allocationId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while deallocation process", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Deallocation done successfully" });
        }

        [HttpPut]
        [Route("Reallocate/{allocationId}")]
        public IActionResult Reallocate(int allocationId)
        {
            try
            {
                // Check if Tenant or Rental of this allocation is not occupied in other allocation whichi is active
                this.AllocationRepository.Reallocate(allocationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while reallocation process", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Reallocation done successfully" });
        }

        [HttpDelete]
        [Route("Delete/{allocationId}")]
        public IActionResult Delete(int allocationId)
        {
            try
            {
                Allocation allocation = this.AllocationRepository.Find(allocationId);
                if (allocation == null)
                    return NotFound(new { error = "Allocation not found" });

                int rentalId = allocation.RentalId;
                int tenantId = allocation.TenantId;

                if (!RentalAccess.Check(rentalId, this.UserId) || !TenantAccess.Check(tenantId, this.UserId))
                    return Unauthorized("Allocation is not associated with your account");

                if (allocation.IsActive)
                    return Unauthorized("Active allocation cannot be deleted, First deallocate the allocation and then delete the allocation");

                this.AllocationRepository.Delete(allocationId);

            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while deleting allocation", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Allocation deleted successfully"});

        }
    }
}