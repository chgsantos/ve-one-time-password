using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneTimePassword.Controllers;
using Smocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OneTimePassword.Controllers.Tests
{
    [TestClass()]
    public class PasswordControllerTests
    {
        private static Guid validUserId = Guid.Parse("a287509a-d517-43dd-972a-ff10798d014c");
        private static Guid invalidUserId = Guid.Empty;

        /// <summary>
        /// Checks the Initialize action
        /// </summary>
        [TestMethod()]
        public void InitializeTest()
        {
            // Arrange
            PasswordController controller = new PasswordController();

            // Act
            JsonResult result = controller.Initialize() as JsonResult;
            dynamic data = result.Data;

            // Assert
            Assert.AreEqual(data.status, true);
        }

        /// <summary>
        /// Checks the GeneratePassword action with a valid user
        /// </summary>
        [TestMethod()]
        public void GeneratePasswordTest_ValidUser()
        {
            // Arrange
            InitializeTest();
            PasswordController controller = new PasswordController();

            // Act
            JsonResult result = controller.GeneratePassword(validUserId) as JsonResult;
            dynamic data = result.Data;

            // Assert
            Assert.AreEqual(data.status, true);
            Assert.AreEqual(((String)data.data).Length, 6);
        }

        /// <summary>
        /// Checks the GeneratePassword action with an invalid user
        /// </summary>
        [TestMethod()]
        public void GeneratePasswordTest_InvalidUser()
        {
            // Arrange
            InitializeTest();
            PasswordController controller = new PasswordController();

            // Act
            JsonResult result = controller.GeneratePassword(invalidUserId) as JsonResult;
            dynamic data = result.Data;

            // Assert
            Assert.AreEqual(data.status, false);
        }

        /// <summary>
        /// Checks the ValidatePassword action using an invalid user
        /// </summary>
        [TestMethod()]
        public void ValidatePasswordTest_InvalidUser()
        {
            // Arrange
            InitializeTest();
            PasswordController controller = new PasswordController();

            // Act
            JsonResult result = controller.ValidatePassword(invalidUserId, "000000") as JsonResult;
            dynamic data = result.Data;

            // Assert
            Assert.AreEqual(data.status, false);
        }

        /// <summary>
        /// Checks the ValidatePassword action using a valid user and a valid password
        /// </summary>
        [TestMethod()]
        public void ValidatePasswordTest_ValidPassword()
        {
            // Arrange
            InitializeTest();
            PasswordController controller = new PasswordController();

            // Act

            // generates the password
            JsonResult passwordResult = controller.GeneratePassword(validUserId) as JsonResult;
            dynamic passwordData = passwordResult.Data;
            String password = passwordData.data;

            // uses the password
            JsonResult result = controller.ValidatePassword(validUserId, password) as JsonResult;
            dynamic data = result.Data;

            // Assert
            Assert.AreEqual(data.status, true);
        }

        /// <summary>
        /// Checks the ValidatePassword action using a valid user and a same valid password two times
        /// </summary>
        [TestMethod()]
        public void ValidatePasswordTest_ValidPasswordTwoTimes()
        {
            // Arrange
            InitializeTest();
            PasswordController controller = new PasswordController();

            // Act

            // generates the password
            JsonResult passwordResult = controller.GeneratePassword(validUserId) as JsonResult;
            dynamic passwordData = passwordResult.Data;
            String password = passwordData.data;

            // uses the password for the first time
            JsonResult firstTimeResult = controller.ValidatePassword(validUserId, password) as JsonResult;
            dynamic firstTimeData = firstTimeResult.Data;

            // uses the password for the second time
            JsonResult secondTimeResult = controller.ValidatePassword(validUserId, password) as JsonResult;
            dynamic secondTimeData = secondTimeResult.Data;

            // Assert
            Assert.AreEqual(firstTimeData.status, true);
            Assert.AreEqual(secondTimeData.status, false);
        }

        /// <summary>
        /// Checks the ValidatePassword action with a valid user and an invalid password
        /// </summary>
        [TestMethod()]
        public void ValidatePasswordTest_InvalidPassword()
        {
            // Arrange
            InitializeTest();
            PasswordController controller = new PasswordController();

            // Act
            JsonResult result = controller.ValidatePassword(validUserId, "000000") as JsonResult;
            dynamic data = result.Data;

            // Assert
            Assert.AreEqual(data.status, false);
        }

        /// <summary>
        /// Checks the ValidatePassword action with a valid user and an expired password
        /// </summary>
        [TestMethod()]
        public void ValidatePasswordTest_ExpiredPassword()
        {
            // Arrange
            InitializeTest();
            PasswordController generateController = new PasswordController();

            // Act

            // generates the password
            JsonResult passwordResult = generateController.GeneratePassword(validUserId) as JsonResult;
            dynamic passwordData = passwordResult.Data;
            String password = passwordData.data;

            // creates an UtcNow date with 31 seconds delay
            var a = DateTime.UtcNow;
            DateTime delayedDate = DateTime.UtcNow.AddSeconds(31);

            // uses a mock context that makes the DateTime.UtcNow return the delayed date
            Smock.Run(context =>
            {
                context.Setup(() => DateTime.UtcNow).Returns(delayedDate);

                InitializeTest();
                PasswordController validateController = new PasswordController();
                JsonResult result = validateController.ValidatePassword(validUserId, password) as JsonResult;
                dynamic data = result.Data;

                // Assert
                Assert.AreEqual(data.status, false);
            });
        }
    }
}