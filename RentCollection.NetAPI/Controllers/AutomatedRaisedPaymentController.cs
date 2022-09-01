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
    public class AutomatedRaisedPaymentController : ControllerBase
    {

        private IAllocationRepository AllocationRepository;
        private IAutomatedRaisedPaymentRepository AutomatedRaisedPaymentRepository;
        private IInvoiceItemCategoryRepository InvoiceItemCategoryRepository;
        private IRentalRepository RentalRepository;
        private int UserId;

        public AutomatedRaisedPaymentController(IHttpContextAccessor httpContextAccessor)
        {
            this.AllocationRepository = new AllocationRepository(new RentCollectionContext());
            this.AutomatedRaisedPaymentRepository = new AutomatedRaisedPaymentRepository(new RentCollectionContext());
            this.InvoiceItemCategoryRepository = new InvoiceItemCategoryRepository(new RentCollectionContext());
            this.RentalRepository = new RentalRepository(new RentCollectionContext());
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(AutomatedRaisedPayment automatedRaisedPayment)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                Allocation allocation = this.AllocationRepository.Find(automatedRaisedPayment.AllocationId);

                if (!RentalAccess.Check(allocation.RentalId, this.UserId) || !TenantAccess.Check(allocation.TenantId, this.UserId))
                    return Unauthorized("Rental or Tenant is not associated with your account");

                if (!allocation.IsActive)
                    return BadRequest(new { error = "Allocation is not active" });

                this.AutomatedRaisedPaymentRepository.Add(automatedRaisedPayment);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while adding automated raised payment", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Automated raised payment added successfully", automatedRaisedPayment = automatedRaisedPayment });
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(AutomatedRaisedPaymentUpdate automatedRaisedPaymentUpdate)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                AutomatedRaisedPayment automatedRaisedPayment = this.AutomatedRaisedPaymentRepository.Find(automatedRaisedPaymentUpdate.AutomatedRaisedPaymentId);

                Allocation allocation = this.AllocationRepository.Find(automatedRaisedPayment.AllocationId);

                if (!RentalAccess.Check(allocation.RentalId, this.UserId) || !TenantAccess.Check(allocation.TenantId, this.UserId))
                    return Unauthorized("Rental or Tenant is not associated with your account");

                if (!allocation.IsActive)
                    return BadRequest(new { error = "Allocation is not active" });

                this.AutomatedRaisedPaymentRepository.Update(automatedRaisedPaymentUpdate);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while updating automated raised payment", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Automated raised payment updated successfully", automatedRaisedPaymentUpdate = automatedRaisedPaymentUpdate });
        }

        [HttpDelete]
        [Route("Delete/{automatedRaisedPaymentId}")]
        public IActionResult Delete(int automatedRaisedPaymentId)
        {
            try
            {
                AutomatedRaisedPayment automatedRaisedPayment = this.AutomatedRaisedPaymentRepository.Find(automatedRaisedPaymentId);

                Allocation allocation = this.AllocationRepository.Find(automatedRaisedPayment.AllocationId);

                if (!RentalAccess.Check(allocation.RentalId, this.UserId) || !TenantAccess.Check(allocation.TenantId, this.UserId))
                    return Unauthorized("Rental or Tenant is not associated with your account");

                if (!allocation.IsActive)
                    return BadRequest(new { error = "Allocation is not active" });

                int invoiceItemCategoryId = this.InvoiceItemCategoryRepository.GetInvoiceItemCategoryIdByCode("Rent", this.UserId);

                if (automatedRaisedPayment.InvoiceItemCategoryId == invoiceItemCategoryId)
                    return BadRequest(new { error = "Rent automated payment can not be deleted" });

                this.AutomatedRaisedPaymentRepository.Delete(automatedRaisedPaymentId);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Something went wrong while deleting automated raised payment", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Automated raised payment deleted successfully" });
        }
    }
}