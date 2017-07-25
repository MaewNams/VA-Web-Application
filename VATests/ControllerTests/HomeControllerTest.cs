using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VA;
using Moq;
using VA.Models;
using VA.Repositories;
using VA.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VA.ViewModel;

namespace VATests
{
    [TestClass]
    public class HomeControllerTest
    {
        // HomeController controllerUnderTest = new HomeController();


        [TestMethod]
        public void TestIndexOfCurrentDate()
        {  ///--------------------- Current date is 12/05/2017


            //appointment 1
            TimeSpan start1 = new TimeSpan(09, 30, 0);
            TimeSpan end1 = new TimeSpan(11, 30, 0);
            DateTime startTime1 = new DateTime(2017, 5, 12) + start1;
            DateTime endTime1 = new DateTime(2017, 5, 12) + end1;

            //appointment 2 the same day as appointment 1
            TimeSpan start2 = new TimeSpan(10, 30, 0);
            TimeSpan end2 = new TimeSpan(11, 30, 0);
            DateTime startTime2 = new DateTime(2017, 5, 12) + start2;
            DateTime endTime2 = new DateTime(2017, 5, 12) + end2;

            //appointment 3 the same month
            TimeSpan start3 = new TimeSpan(10, 30, 0);
            TimeSpan end3 = new TimeSpan(11, 30, 0);
            DateTime startTime3 = new DateTime(2017, 10, 5) + start3;
            DateTime endTime3 = new DateTime(2017, 10, 5) + end3;

            // Abobe data did not use in test because

            var waitingAppointments = new List<Appointment>
            {
                new Appointment {id = 1, memberId = 1, petId = 1, serviceId =1, detail= "test detail01",
                    startTime = startTime1, endTime = endTime1, status = "Waiting"},
                new Appointment {id = 3, memberId = 2, petId = 2, serviceId =1, detail= "test detail03",
                    startTime = startTime3, endTime = endTime3, status = "Waiting"},
            };

            var completeAppointments = new List<Appointment>
            {
               new Appointment {id = 2, memberId = 1, petId = 1, serviceId =2, detail= "test detail02",
                    startTime = startTime2, endTime = endTime2, status = "Complete"}
            };

            //          var controllerUnderTest2 = new HomeController(mockAppointmentRepository);

            //Arrange

            // Arrange
            ///--------------------- Current date is 12/05/2017
            ///

            DateTime today = new DateTime(2017, 5, 12);

            var mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockAppointmentRepository
                .Setup(e => e.GetByDayAndMonthAndYear(today.Day, today.Month, today.Year, "Waiting"))
                .Returns(waitingAppointments
                .ToList());
            mockAppointmentRepository
                .Setup(e => e.GetByDayAndMonthAndYear(today.Day, today.Month, today.Year, "Complete"))
                .Returns(completeAppointments
                .ToList());

            var controllerUnderTest = new HomeController(mockAppointmentRepository.Object);

            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            context.SetupGet(x => x.Session["Authen"]).Returns("true");
            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);

            // Act 

            var result = controllerUnderTest.Index(today.Day, today.Month, today.Year) as ViewResult;


            var vafalse = controllerUnderTest.VASetting("#@!$!@#!@#") as JsonResult;
           // var vatrue = controllerUnderTest.VASetting("1234");

            var b = "";

           

            var model = result.Model as AllAppointmentViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(today, model.date);
            Assert.AreEqual(1, model.AppCom.Count());
            Assert.AreEqual(2, model.AppWait.Count());

        }

        [TestMethod]
        public void TestSomething()
        {
            // Arrange
            /*  var repoMock = new Mock<IAppointmentRepository>();
              repoMock.Setup(r => r.GetSomething()).Returns(TestData.SomeThings);
              var controller = new SomethingController(repoMock.Object);
  */
            // Act
            /* controller.DoStuff();*/

            // Assert
        }
    }
}
