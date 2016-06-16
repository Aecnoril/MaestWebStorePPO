using MaestWebStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace MaestWebStore.Tests
{
    [TestClass]
    class LoginTest
    {
        [TestMethod]
        public void LoginViewTest()
        {
            //Arrange
            var user = new Controllers.UserController();
            var userM = new Models.User();
            userM.Password = "test";
            userM.Username = "Koen";
            //Act and assert
            var result = (RedirectToRouteResult) user.Login(userM); //???
            //Assert.AreEqual("Index", result.RouteValues["Action"])
        }

    }
}
