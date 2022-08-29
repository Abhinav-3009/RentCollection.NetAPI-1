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
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private IInvoiceRepository InvoiceRepository;
        private readonly RentCollectionContext db = new RentCollectionContext();

        private int UserId;

        public InvoiceController(IHttpContextAccessor httpContextAccessor)
        {
            this.InvoiceRepository = new InvoiceRepository(new RentCollectionContext());
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(Invoice invoice)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while creating invoice", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Invoice created successfully", invoice = invoice });
        }

        [HttpGet]
        [Route("Get/{invoiceId}")]
        public IActionResult GetInvoice(int invoiceId)
        {
            Invoice invoice = new Invoice();
            try
            {
                invoice = this.InvoiceRepository.GetInvoice(invoiceId);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while fetching invoice", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Invoice fetched successfully", invoice = invoice });
        }
    }
}