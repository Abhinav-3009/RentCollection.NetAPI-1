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
using RentCollection.NetAPI.Security;
using RentCollection.NetAPI.ServiceImplementation;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TenantController : ControllerBase
    {

        private ITenantRepository TenantRepository;

        private int UserId;

        public TenantController(IHttpContextAccessor httpContextAccessor)
        {
            this.TenantRepository = new TenantRepository(new RentCollectionContext());
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(Tenant tenant)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                tenant.UserId = this.UserId;
                tenant.Password = Encryption.Encrypt(tenant.Password);
                this.TenantRepository.Add(tenant);
                tenant.Password = "Password is secured with encryption";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while adding tenant", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Tenant added successfully", tenant = tenant });
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(Tenant tenant)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                if(!TenantAccess.Check(tenant.TenantId, this.UserId))
                    return Unauthorized("Tenant record is not associated with your account");

                tenant.UserId = this.UserId;
                tenant.Password = Encryption.Encrypt(tenant.Password);
                this.TenantRepository.Add(tenant);
                tenant.Password = "Password is secured with encryption";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while updating tenant", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Tenant updated successfully", tenant = tenant });
        }

        [HttpDelete]
        [Route("Delete/{tenantId}")]
        public IActionResult Delete(int tenantId)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                if (!TenantAccess.Check(tenantId, this.UserId))
                    return Unauthorized("Tenant record is not associated with your account");

                if (!AllocationAccess.CheckIfAllocationEntityAreOccupied(tenantId, 0))
                    return Unauthorized("Allocated tenant can not be deleted");

                this.TenantRepository.Delete(tenantId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while deleting tenant", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Tenant deleted successfully" });
        }

        [HttpGet]
        [Route("Get/{tenantId}")]
        public IActionResult Get(int tenantId)
        {
            Tenant tenant = new Tenant();
            try
            {
                if (!TenantAccess.Check(tenantId, this.UserId))
                    return Unauthorized("Tenant record is not associated with your account");
                tenant = this.TenantRepository.Get(tenantId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while fetching tenant", exceptionMessage = e.Message });
            }
            return Ok(new { success = "Tenant fetched successfully", tenant = tenant });
        }


        [HttpGet]
        [Route("Get")]
        public IActionResult GetAll()
        {
            List<Tenant> tenants = new List<Tenant>();
            try
            {
                tenants = this.TenantRepository.GetAll(this.UserId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while fetching tenants", exceptionMessage = e.Message });
            }
            return Ok(new { success = "Tenants fetched successfully", tenants = tenants });
        }
    }
}