using System;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IElectricityMeterReadingRepository : IDisposable
    {
        void Add(ElectricityMeterReading electricityMeterReading);
        void Delete(int electricityMeterReadingId);

        ElectricityMeterReading FetchLatestReading(int rentalId);

        ElectricityMeterReading Get(int electricityMeterReadingId);
    }
}

