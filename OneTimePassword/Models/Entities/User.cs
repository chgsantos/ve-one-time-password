using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneTimePassword.Models.Entities
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        // Unique Identifier
        public Guid Id { get; set; }

        // One time password
        public String Password { get; set; }

        // Password generated date
        public DateTime? PasswordDate { get; set; }
    }
}