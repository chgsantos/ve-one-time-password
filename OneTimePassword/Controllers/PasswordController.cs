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
        private static List<User> _users = new List<User>();
        private int _passwordTimeoutSeconds = 30;

        /// <summary>
        /// Constructor that initializes _users list with some users
        /// </summary>
        public PasswordController()
        {
            if (_users.Count == 0)
            {
                _users.Add(
                    new User
                    {
                        Id = Guid.Parse("837b062c-18c5-46cb-aa3d-43e6b43a3363")
                    }
                );

                _users.Add(
                    new User
                    {
                        Id = Guid.Parse("a287509a-d517-43dd-972a-ff10798d014c")
                    }
                );

                _users.Add(
                    new User
                    {
                        Id = Guid.Parse("407e6bce-19c5-4a73-b327-fcb94b25adb6")
                    }
                );
            }
        }

        /// <summary>
        /// Generates a password for a given User ID
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>The generated password for the given User ID</returns>
        public JsonResult GeneratePassword(Guid userId)
        {
            // Search the User by ID
            var user = (
                from i 
                in _users
                where i.Id.Equals(userId)
                select i
            ).FirstOrDefault();

            if (user == null)
            {
                // If no user was found
                return JSendResult(false, null, "User not found");
            }
            else
            {
                // If user was found
                if (user.PasswordGeneratedDate != null && user.PasswordGeneratedDate.Value.AddSeconds(_passwordTimeoutSeconds) > DateTime.UtcNow)
                {
                    // If password is valid yet, returns the same password
                    return JSendResult(true, user.Password);
                }
                else
                {
                    // If password is invalid or there is no password, generates a new password

                    // gets the current passwords and verify if the generated password is unique
                    Random random = new Random();
                    String newPassword = "";
                    List<string> currentPasswords = (
                        from i
                        in _users
                        where i.PasswordGeneratedDate != null && i.PasswordGeneratedDate.Value.AddSeconds(_passwordTimeoutSeconds) > DateTime.UtcNow
                        select i.Password
                    ).ToList();
                    do
                    {
                        newPassword = random.Next(100000, 999999).ToString();
                    } while (currentPasswords.Contains(newPassword));

                    // sets the new password to the user
                    user.Password = newPassword;
                    user.PasswordGeneratedDate = DateTime.UtcNow;

                    // returns
                    return JSendResult(true, user.Password);
                }
            }
        }

        /// <summary>
        /// Checks the validity of a given User ID and password
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="password">Password</param>
        /// <returns>Validity of the password</returns>
        public JsonResult ValidatePassword(Guid userId, String password)
        {
            // Search the User by ID
            var user = (
                from i
                in _users
                where i.Id.Equals(userId)
                select i
            ).FirstOrDefault();

            if (user == null)
            {
                // If no user was found
                return JSendResult(false, null, "User not found");
            }
            else
            {
                // If user was found
                if (user.PasswordGeneratedDate != null && user.PasswordGeneratedDate.Value.AddSeconds(_passwordTimeoutSeconds) > DateTime.UtcNow)
                {
                    // If password is valid yet, clears the password (one time use only) and returns true
                    user.Password = null;
                    user.PasswordGeneratedDate = null;

                    return JSendResult(true, user.Password);
                }
                else
                {
                    // If password is invalid or there is no password, returns false
                    return JSendResult(false, user.Password);
                }
            }
        }

        /// <summary>
        /// JsonResult based on JSend specification
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="data">Data to the client</param>
        /// <param name="message">Error message</param>
        /// <param name="code">Error code</param>
        /// <returns>JsonResult based on JSend specification</returns>
        private JsonResult JSendResult(bool status, object data = null, string message = null, int? code = null)
        {
            return Json(
                new
                {
                    status = status,
                    data = data,
                    message = message,
                    code = code
                }, 
                JsonRequestBehavior.AllowGet
            );
        }
    }
}