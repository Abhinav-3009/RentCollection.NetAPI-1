using System;
using System.Linq;
using RentCollection.NetAPI.Models;
using RentCollection.NetAPI.Security;

namespace RentCollection.NetAPI.Authentication
{
    public class UserAuthentication
    {
        private static RentCollectionContext db = new RentCollectionContext();

        public static User GetUser(string username, string password)
        {
            var users = db.Users.ToList();
            var user = (from u in users where u.Username == username && Encryption.Decrypt(u.Password) == password select u).FirstOrDefault();
            return user;
        }
    }
}

