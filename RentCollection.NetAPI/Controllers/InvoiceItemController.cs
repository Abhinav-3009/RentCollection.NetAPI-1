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
using RentCollection.NetAPI.ServiceImplementation;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceItemController : ControllerBase
    {
        private IInvoiceItemRepository InvoiceItemRepository;
        private IInvoiceItemCategoryRepository InvoiceItemCategoryRepository;
        private IInvoiceRepository InvoiceRepository;
        private IAllocationRepository AllocationRepository;
        private int UserId;

        public InvoiceItemController(IHttpContextAccessor httpContextAccessor)
        {
            this.InvoiceItemRepository = new InvoiceItemRepository(new RentCollectionContext());
            this.InvoiceItemCategoryRepository = new InvoiceItemCategoryRepository(new RentCollectionContext());
            this.InvoiceRepository = new InvoiceRepository(new RentCollectionContext());
            this.AllocationRepository = new AllocationRepository(new RentCollectionContext());
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(InvoiceItem invoiceItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                // Check if Invoice is associated with the account

                this.InvoiceItemRepository.Add(invoiceItem);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while adding invoice item", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Invoice Item added successfully", invoiceItem = invoiceItem });
        }

        [HttpDelete]
        [Route("Delete/{invoiceItemId}")]
        public IActionResult Delete(int invoiceItemId)
        {
            try
            {
                // Check if Invoice Item is associated with account and Invoice is not deleted
                InvoiceItem invoiceItem = this.InvoiceItemRepository.Get(invoiceItemId);
                int invoiceId = invoiceItem.InvoiceId;
                Invoice invoice = this.InvoiceRepository.GetInvoice(invoiceId);

                Allocation allocation = this.AllocationRepository.Find(invoice.AllocationId);
                if (!RentalAccess.Check(allocation.RentalId, this.UserId) || !TenantAccess.Check(allocation.TenantId, this.UserId))
                    return Unauthorized("Rental or Tenant is not associated with your account");
                if (!allocation.IsActive)
                    return BadRequest(new { error = "Cannot perform delete action on inactive allocation" });
                if (invoice.IsDeleted)
                    return BadRequest(new { error = "Cannot delete items in deleted invoice" });
                this.InvoiceItemRepository.Delete(invoiceItemId);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while deleting invoice item", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Invoice Item deleted successfully"});
        }
    }
}