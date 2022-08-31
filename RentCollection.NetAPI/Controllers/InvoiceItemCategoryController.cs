using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceImplementation;
using RentCollection.NetAPI.ServiceInterface;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceItemCategoryController : ControllerBase
    {
        private IInvoiceItemCategoryRepository InvoiceItemCategoryRepository;

        private int UserId;

        public InvoiceItemCategoryController(IHttpContextAccessor httpContextAccessor)
        {
            this.InvoiceItemCategoryRepository = new InvoiceItemCategoryRepository(new RentCollectionContext());
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(InvoiceItemCategory invoiceItemCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });
            try
            {
                this.InvoiceItemCategoryRepository.Add(invoiceItemCategory);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while adding invoice item category", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Invoice item category added successfully", invoiceItemCategory = invoiceItemCategory });
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(InvoiceItemCategoryUpdate invoiceItemCategoryUpdate)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });
            try
            {
                this.InvoiceItemCategoryRepository.Update(invoiceItemCategoryUpdate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while updating invoice item category", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Invoice item category updated successfully", invoiceItemCategoryUpdate = invoiceItemCategoryUpdate });
        }

        [HttpDelete]
        [Route("Delete/{invoiceItemCategoryId}")]
        public IActionResult Delete(int invoiceItemCategoryId)
        {
            try
            {
                // Check if any invoice item is bases on this category
                if (!this.InvoiceItemCategoryRepository.Used(invoiceItemCategoryId))
                    this.InvoiceItemCategoryRepository.Delete(invoiceItemCategoryId);
                else return BadRequest(new { error = "Cannot Delete: Invoice items utilise this invoice item category" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while deleting invoice item category", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Invoice item category deleted successfully" });
        }
    }
}