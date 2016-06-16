using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaestWebStore.Tests
{
    [TestClass]
    public class DetailsUnitTest
    {
        [TestMethod]
        public void TestDetails()
        {
            //Arrange
            Util.DatabaseConnection.Initialize("dbi319888", "Knotwilg117", "192.168.15.50:1521/fhictora");
            var game = new Controllers.GameController();
            //Ac
            var result = game.Details(new Models.Game(), 3);
            //Assert
            Assert.AreEqual("Details", result);

        }
    }
}
