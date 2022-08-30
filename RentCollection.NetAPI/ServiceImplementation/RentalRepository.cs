using System;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class RentalRepository : IRentalRepository, IDisposable
    {
        private RentCollectionContext context;

        public RentalRepository(RentCollectionContext context)
        {
            this.context = context;
        }

        public void Add(Rental rental)
        {
            this.context.Rentals.Add(rental);
            this.context.SaveChanges();
        }

        public void Delete(int rentalId)
        {
            Rental rental = this.context.Rentals.Find(rentalId);
            rental.IsDeleted = true;
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Rental Get(int rentalId)
        {
            Rental rental = this.context.Rentals.Find(rentalId);
            return rental;
        }

        public void Update(Rental rental)
        {
            rental.IsDeleted = false;
            this.context.Rentals.Update(rental);
            this.context.SaveChanges();
        }
    }
}

