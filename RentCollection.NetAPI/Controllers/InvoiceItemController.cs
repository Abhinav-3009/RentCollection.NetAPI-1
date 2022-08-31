using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCollection.NetAPI.Models;
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

        public InvoiceItemController()
        {
            this.InvoiceItemRepository = new InvoiceItemRepository(new RentCollectionContext());
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