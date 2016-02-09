using OneTimePassword.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneTimePassword.Controllers
{
    public class PasswordController : Controller
    {
        // Declare the list of Users
        private static List<User> _users;

        /// <summary>
        /// Initializes the app with some users
        /// </summary>
        /// <returns></returns>
        public JsonResult Initialize()
        {
            return Json(new {
                status = true,
                data = ""
            });
        }

        /// <summary>
        /// Generates a password for a given User ID
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>The generated password for the given User ID</returns>
        public JsonResult GeneratePassword(Guid userId)
        {
            return Json(new
            {
                status = true,
                data = 123456
            });
        }

        /// <summary>
        /// Checks the validity of a given User ID and password
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="password">Password</param>
        /// <returns>Validity of the password</returns>
        public JsonResult ValidatePassword(Guid userId, String password)
        {
            return Json(new {
                status = true,
                data = true
            });
        }
    }
}