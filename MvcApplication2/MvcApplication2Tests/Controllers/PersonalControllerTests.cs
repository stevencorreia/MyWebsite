using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcApplication2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers.Tests
{
    [TestClass()]
    public class PersonalControllerTests
    {
        [TestMethod()]
        public void SaveFilesTest()
        {
            JsonResult a =  new PersonalController().SaveFiles("test");
            Assert.AreEqual(1,1);
        }
    }
}