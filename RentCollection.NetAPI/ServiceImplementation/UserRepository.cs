using System;
using Microsoft.EntityFrameworkCore;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.ServiceInterface;

namespace RentCollection.NetAPI.ServiceImplementation
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private RentCollectionContext context;

        public UserRepository(RentCollectionContext context)
        {
            this.context = context;
        }

        public void Add(User user)
        {
            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}