using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.Security;
using RentCollection.NetAPI.ServiceImplementation;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private IPaymentRepository PaymentRepository;

        private int UserId;

        public PaymentController(IHttpContextAccessor httpContextAccessor)
        {
            this.PaymentRepository = new PaymentRepository(new RentCollectionContext());
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                payment.Amount = -payment.Amount;
                this.PaymentRepository.Add(payment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while adding payment", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Payment added successfully", payment = payment });
        }

        [HttpDelete]
        [Route("Delete/{paymentId}")]
        public IActionResult Delete(int paymentId)
        {

            try
            {
                this.PaymentRepository.Delete(paymentId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while deleting payment", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Payment deleted successfully" });
        }
    }
}