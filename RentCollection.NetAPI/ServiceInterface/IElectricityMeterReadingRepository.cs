using System;
using System.Collections.Generic;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IElectricityMeterReadingRepository : IDisposable
    {
        void Add(ElectricityMeterReading electricityMeterReading);
        void Delete(int electricityMeterReadingId);

        ElectricityMeterReading FetchLatestReading(int rentalId);

        ElectricityMeterReading Get(int electricityMeterReadingId);

        List<ElectricityMeterReading> GetAllReadings(int rentalId);
    }
}

