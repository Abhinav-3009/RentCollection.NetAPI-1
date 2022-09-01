using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    public class ElectricityMeterReadingController : ControllerBase
    {
        private IElectricityMeterReadingRepository ElectricityMeterReadingRepository;
        private IInvoiceRepository InvoiceRepository;
        private IInvoiceItemCategoryRepository InvoiceItemCategoryRepository;
        private IInvoiceItemRepository InvoiceItemRepository;
        private IAllocationRepository AllocationRepository;

        private int UserId;

        public ElectricityMeterReadingController(IHttpContextAccessor httpContextAccessor)
        {
            this.ElectricityMeterReadingRepository = new ElectricityMeterReadingRepository(new RentCollectionContext());
            this.InvoiceRepository = new InvoiceRepository(new RentCollectionContext());
            this.AllocationRepository = new AllocationRepository(new RentCollectionContext());
            this.InvoiceItemCategoryRepository = new InvoiceItemCategoryRepository(new RentCollectionContext());
            this.InvoiceItemRepository = new InvoiceItemRepository(new RentCollectionContext());
            this.UserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(ElectricityMeterReading electricityMeterReading)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Model is invalid" });

            try
            {
                // Fetch Allocation
                Allocation allocation = this.AllocationRepository.GetAllocationByRentalId(electricityMeterReading.RentalId);

                if (!electricityMeterReading.GenerateBill)
                {
                    // Add new meter reading
                    this.ElectricityMeterReadingRepository.Add(electricityMeterReading);
                    return Ok(new { success = "Electricity meter reading added successfully" });
                }

                // Generate Bill

                // Fetch latest reading
                ElectricityMeterReading latestReading = this.ElectricityMeterReadingRepository.FetchLatestReading(electricityMeterReading.RentalId);

                // Add new meter reading
                this.ElectricityMeterReadingRepository.Add(electricityMeterReading);



                if (latestReading == null)
                    return Ok(new { success = "Electricity meter reading added successfully" });

                if (latestReading.TakenOn > electricityMeterReading.TakenOn)
                    return BadRequest(new { error = "Invalid Date, Last reading taken on: " + latestReading.TakenOn.ToShortDateString() + " New reading date must be greater than last reading date" });

                if (latestReading.Units >= electricityMeterReading.Units)
                    return BadRequest(new { error = "Units must be greater than last reading units, Last reading units: " + latestReading.Units });

                int units = electricityMeterReading.Units - latestReading.Units;
                int amount = electricityMeterReading.Charges * units;

                // Create invoice
                Invoice newInvoice = new Invoice();
                newInvoice.AllocationId = allocation.AllocationId;
                newInvoice.InvoiceDate = DateTime.Now;
                Invoice invoice = this.InvoiceRepository.Create(newInvoice);

                // Create invoice item for electricity bill
                int invoiceItemCategoryId = this.InvoiceItemCategoryRepository.GetInvoiceItemCategoryIdByCode("Electricity Bill", this.UserId);

                InvoiceItem invoiceItem = new InvoiceItem();
                invoiceItem.InvoiceId = invoice.InvoiceId;
                invoiceItem.InvoiceItemCategoryId = invoiceItemCategoryId;
                invoiceItem.Amount = amount;
                invoiceItem.Date = DateTime.Now;
                invoiceItem.Description = "From " +
                                          latestReading.TakenOn.ToShortDateString() +
                                          ": " +
                                          latestReading.Units +
                                          ", " +
                                          "To " +
                                          electricityMeterReading.TakenOn.ToShortDateString() +
                                          ": " +
                                          electricityMeterReading.Units +
                                          ", " +
                                          "Net Consumption: " +
                                          units +
                                          (units == 1 ? " Unit" : " Units") +
                                          ", " +
                                          "Charges: " +
                                          electricityMeterReading.Charges +
                                          " Rs/Unit" +
                                          ", " +
                                          "Bill Amount: Rs " +
                                          amount +
                                          ", " +
                                          "MeterReadingId: " +
                                          electricityMeterReading.MeterReadingId;

                this.InvoiceItemRepository.Add(invoiceItem);
                return Ok(new { success = "₹" + amount + " raised for electricity bill" });

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace); 
                return BadRequest(new { error = "Something went wrong while adding electricity meter reading", exceptionMessage = e.Message });
            }

        }

        [HttpDelete]
        [Route("Delete/{electricityMeterReadingId}")]
        public IActionResult Delete(int electricityMeterReadingId)
        {

            try
            {
                this.ElectricityMeterReadingRepository.Delete(electricityMeterReadingId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while deleting electricity meter reading", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Electricity meter reading deleted successfully" });
        }

        [HttpGet]
        [Route("Get/{electricityMeterReadingId}")]
        public IActionResult Get(int electricityMeterReadingId)
        {
            ElectricityMeterReading electricityMeterReading = new ElectricityMeterReading();
            try
            {
                electricityMeterReading = this.ElectricityMeterReadingRepository.Get(electricityMeterReadingId);

                if (!RentalAccess.Check(electricityMeterReading.RentalId, this.UserId))
                    return BadRequest(new { error = "Rental not associated with your account" });

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while fetching electricity meter reading", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Electricity meter reading fetched successfully", electricityMeterReading = electricityMeterReading });
        }

        [HttpGet]
        [Route("Get/All/{rentalId}")]
        public IActionResult GetAllReadings(int rentalId)
        {
            List<ElectricityMeterReading> readings = new List<ElectricityMeterReading>();
            try
            {
                if (!RentalAccess.Check(rentalId, this.UserId))
                    return BadRequest(new { error = "Rental not associated with your account" });

                readings = this.ElectricityMeterReadingRepository.GetAllReadings(rentalId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest(new { error = "Something went wrong while fetching electricity meter readings", exceptionMessage = e.Message });
            }

            return Ok(new { success = "Electricity meter readings fetched successfully", readings = readings });
        }
    }
}