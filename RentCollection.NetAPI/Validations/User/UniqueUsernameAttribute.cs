using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RentCollection.NetAPI.Models;

namespace RentCollection.NetAPI.Validations.User
{
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        private RentCollectionContext db = new RentCollectionContext();

        public override bool IsValid(object value)
        {
            string username = value.ToString();
            var users = db.Users.ToList();
            var user = (from u in users where u.Username == username select u).FirstOrDefault();

            return user == null ? true : false;
            return true;
        }
    }
}

