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
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly RentCollectionContext db = new RentCollectionContext();

        private int UserId;

        public InvoiceController(IHttpContextAccessor httpContextAccessor)
        {
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(Invoice invoice)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                Allocation allocation = db.Allocations.Find(invoice.AllocationId);
                if (allocation == null)
                    return NotFound(new { error = "Allocation not found" });

                int rentalId = allocation.RentalId;
                int tenantId = allocation.TenantId;

                if (!RentalAccess.Check(rentalId, this.UserId) || !TenantAccess.Check(tenantId, this.UserId))
                    return Unauthorized("Allocation is not associated with your account");

                if (!allocation.IsActive)
                    return BadRequest("Allocation is inactive");

                db.Invoices.Add(invoice);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while creating invoice", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Invoice created successfully", invoice = invoice });
        }
    }
}