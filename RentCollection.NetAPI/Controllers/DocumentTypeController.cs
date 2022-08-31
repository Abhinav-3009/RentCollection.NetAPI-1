using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
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
    public class DocumentTypeController : ControllerBase
    {
        
        private IDocumentTypeRepository DocumentTypeRepository;

        private int UserId;

        public DocumentTypeController(IHttpContextAccessor httpContextAccessor)
        {
            this.DocumentTypeRepository = new DocumentTypeRepository(new RentCollectionContext());
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(DocumentType documentType)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });
            try
            {
                this.DocumentTypeRepository.Add(documentType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while adding document type", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Document type added successfully", documentType = documentType });
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(DocumentTypeUpdate documentTypeUpdate)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });
            try
            {
                this.DocumentTypeRepository.Update(documentTypeUpdate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while updating document type", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Document type updated successfully", documentTypeUpdate = documentTypeUpdate });
        }

        [HttpDelete]
        [Route("Delete/{documentTypeId}")]
        public IActionResult Delete(int documentTypeId)
        {
            try
            {
                if (!this.DocumentTypeRepository.Used(documentTypeId))
                    this.DocumentTypeRepository.Delete(documentTypeId);
                else return BadRequest(new { error = "Cannot Delete: Documents utilise this document type" });
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while deleting document type", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Document type deleted successfully" });
        }
    }
}