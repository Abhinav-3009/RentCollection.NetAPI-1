using System;
using System.ComponentModel.DataAnnotations;

namespace RentCollection.NetAPI.ViewModels
{
    public class UserCredentials
    {
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

    }
}

