using System;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.ServiceInterface
{
    public interface IUserRepository
    {
        void Add(User user);
    }
}

