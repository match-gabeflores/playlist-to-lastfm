using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LastfmPlaylist;
using LastfmPlaylist.Controllers;
using NUnit.Framework;
using Moq;
using System.IO;
using Tests;

namespace Tests
{
    

    [TestFixture]
    public class TestSomething
    {
        private ContextMocks _contextMocks;
        


        [Test]
        public void CanUploadTest()
        {
            // Arrange
            var file = new Mock<HttpPostedFileBase>();
            file.Setup(i => i.FileName).Returns("myfile.m3u");
            file.Setup(i => i.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes("")));


            var context = new Mock<ControllerContext>();
            context.Setup(i => i.HttpContext.Request.Files.Count).Returns(1);
            context.Setup(i => i.HttpContext.Request.Files[0]).Returns(file.Object);

            HomeController homeController = new HomeController();
            homeController.ControllerContext = context.Object;

            // Act
            //var actualResult = homeController.Upload(file.Object, true);


            // Assert

            //Assert.IsInstanceOf(typeof (ActionResult), actualResult);
        }
    }
}