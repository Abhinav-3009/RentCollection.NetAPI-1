using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCollection.NetAPI.Authentication;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.Security;
using RentCollection.NetAPI.ServiceImplementation;
using RentCollection.NetAPI.ServiceInterface;
using RentCollection.NetAPI.ViewModels;

namespace RentCollection.NetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IJwtAuthenticationManager JwtAuthenticationManager;

        private IUserRepository UserRepository;
        private IInvoiceItemCategoryRepository InvoiceItemCategoryRepository;
        private IDocumentTypeRepository DocumentTypeRepository;

        private readonly string[] InvoiceItemCategoryList = { "Wifi", "Mess", "Rent", "Electricity Bill", "Waived Off" };

        private readonly string[] DocumentTypeList = { "Aadhaar", "Pancard", "Driving License" };

        public UserController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.UserRepository = new UserRepository(new RentCollectionContext());
            this.InvoiceItemCategoryRepository = new InvoiceItemCategoryRepository(new RentCollectionContext());
            this.DocumentTypeRepository = new DocumentTypeRepository(new RentCollectionContext());
            this.JwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost]
        [Route("Authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate(UserCredentials user)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            string token = this.JwtAuthenticationManager.Authenticate(user.Username, user.Password);
            if (token == null)
                return Unauthorized();

            return Ok(new { success = "Authentication successful", token = token });
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register(User user)
        {
            if(!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                // Check if Contact is verified using OTP

                // encrypt password
                user.Password = Encryption.Encrypt(user.Password);

                // add user to database
                this.UserRepository.Add(user);

                // add default document types to user account
                for(int i=0; i<DocumentTypeList.Length; i++)
                {
                    DocumentType dt = new DocumentType();
                    dt.UserId = user.UserId;
                    dt.Code = DocumentTypeList[i];
                    this.DocumentTypeRepository.Add(dt);
                }

                // add default invoice item category to user account
                for (int i = 0; i < InvoiceItemCategoryList.Length; i++)
                {
                    InvoiceItemCategory itc = new InvoiceItemCategory();
                    itc.UserId = user.UserId;
                    itc.Code = InvoiceItemCategoryList[i];
                    this.InvoiceItemCategoryRepository.Add(itc);
                }

            }
            catch(Exception e)
            {
                return BadRequest(new { error = "Something went wrong while registering user", exceptionMessage = e.Message });
            }

            user.Password = "Your password is saved with encryption";
            return Created("User registration successful", user);
        }

        [HttpGet]
        [Route("Authentication/Testing")]
        public IActionResult AuthenticationTesting()
        {
            return Ok("Authentication Working");
        }

    }
}