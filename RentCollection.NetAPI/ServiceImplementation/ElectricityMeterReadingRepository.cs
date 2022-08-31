using System;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class ElectricityMeterReadingRepository : IElectricityMeterReadingRepository, IDisposable
    {

        private RentCollectionContext Context;
        private SqlConnection Conn;
        public ElectricityMeterReadingRepository(RentCollectionContext context)
        {
            this.Context = context;
        }

        public void Add(ElectricityMeterReading electricityMeterReading)
        {
            this.Context.ElectricityMeterReadings.Add(electricityMeterReading);
            this.Context.SaveChanges();
        }

        public void Delete(int electricityMeterReadingId)
        {

            // Delete associated invoice item using procedure
            this.Context.Database.ExecuteSqlInterpolated($"EXEC DeleteInvoiceItemAssociatedWithElectricityBill({electricityMeterReadingId})");
            ElectricityMeterReading electricityMeterReading = this.Context.ElectricityMeterReadings.Find(electricityMeterReadingId);
            electricityMeterReading.IsDeleted = true;
            this.Context.ElectricityMeterReadings.Update(electricityMeterReading);
            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ElectricityMeterReading FetchLatestReading(int rentalId)
        {
            ElectricityMeterReading electricityMeterReading = this.Context.ElectricityMeterReadings.Where(e => e.RentalId == rentalId && e.IsDeleted == false).OrderByDescending(e => e.MeterReadingId).FirstOrDefault();

            return electricityMeterReading;
        }

        public ElectricityMeterReading Get(int electricityMeterReadingId)
        {
            ElectricityMeterReading electricityMeterReading = this.Context.ElectricityMeterReadings.Find(electricityMeterReadingId);
            return electricityMeterReading;
        }

        
    }
}

