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
        public HomeControllerTest()
        {
            // create some mock products to play with
            IEnumerable<Administrator> admins = new List<Administrator> { };
            IEnumerable<Appointment> appointments = new List<Appointment> { };
            IEnumerable<Member> members = new List<Member> { };
            IEnumerable<Pet> pets = new List<Pet> { };
            IEnumerable<PetType> types = new List<PetType> { };
            Clinic clinic = new Clinic();

            // Mock the Repository using Moq
            Mock<IAdministratorRepository> mockAdminRepo = new Mock<IAdministratorRepository>();
            Mock<IAppointmentRepository> mockAppRepo = new Mock<IAppointmentRepository>();
            Mock<IMemberRepository> mockMemRepo = new Mock<IMemberRepository>();
            Mock<IPetRepository> mockPetRepo = new Mock<IPetRepository>();
            Mock<IPetSpecieRepository> mockSpecieRepo = new Mock<IPetSpecieRepository>();
            Mock<IVCRepository> mockVCRepo = new Mock<IVCRepository>();

            // Return the admin
            mockAdminRepo
                .Setup(mr => mr.GetByUsernamrAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string u, string p) => admins.Where(x => x.username == u && x.password == p)
                .FirstOrDefault());

            mockAppRepo.Setup(e => e.GetByPetID(1)).Returns(appointments.ToList());

            mockMemRepo.Setup(e => e.GetAll()).Returns(members.ToList());

            mockPetRepo.Setup(e => e.GetByMemberID(1)).Returns(pets.ToList());

            mockSpecieRepo.Setup(e => e.GetAll()).Returns(types.ToList());

            mockVCRepo.Setup(e => e.Get()).Returns(clinic);

            // Complete the setup of our Mock 
            this.mockAdminRepo = mockAdminRepo;
            this.mockAppRepo = mockAppRepo;
            this.mockMemRepo = mockMemRepo;
            this.mockPetRepo = mockPetRepo;
            this.mockSpecieRepo = mockSpecieRepo;
            this.mockVCRepo = mockVCRepo;
        }

        public readonly Mock<IAdministratorRepository> mockAdminRepo;
        public readonly Mock<IAppointmentRepository> mockAppRepo;
        public readonly Mock<IMemberRepository> mockMemRepo;
        public readonly Mock<IPetRepository> mockPetRepo;
        public readonly Mock<IPetSpecieRepository> mockSpecieRepo;
        public readonly Mock<IVCRepository> mockVCRepo;

        [TestMethod]
        public void TestIndexWithAppointments()
        {
            //Arrange
            DateTime startTime = new DateTime(2017, 5, 12);


            var waitingAppointments = new List<Appointment>
            {
                new Appointment {id = 1, memberId = 1, petId = 1, serviceId =1, detail= "test detail01",
                    startTime = startTime,  status = "Waiting"},
                new Appointment {id = 3, memberId = 2, petId = 2, serviceId =1, detail= "test detail03",
                    startTime = startTime, status = "Waiting"},
            };

            var completeAppointments = new List<Appointment>
            {
               new Appointment {id = 2, memberId = 1, petId = 1, serviceId =2, detail= "test detail02",
                    startTime = startTime, status = "Complete"}
            };

            DateTime today = new DateTime(2017, 5, 12);


            mockAppRepo
                .Setup(e => e.GetByDayAndMonthAndYearAndStatus(today.Day, today.Month, today.Year, "Waiting"))
                .Returns(waitingAppointments
                .ToList());
            mockAppRepo
                .Setup(e => e.GetByDayAndMonthAndYearAndStatus(today.Day, today.Month, today.Year, "Complete"))
                .Returns(completeAppointments
                .ToList());

            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);

            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            context.SetupGet(x => x.Session["Authen"]).Returns(true);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);

            // Act 

            var result = controllerUnderTest.Index(today.Day, today.Month, today.Year) as ViewResult;

            var model = result.Model as AllAppointmentViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(today, model.date);
            Assert.IsInstanceOfType(model.AppCom, typeof(IEnumerable<Appointment>));
            Assert.IsInstanceOfType(model.AppWait, typeof(IEnumerable<Appointment>));
            Assert.AreEqual(1, model.AppCom.Count());
            Assert.AreEqual(2, model.AppWait.Count());
        }

        [TestMethod]
        public void TestIndexWithNoAppointments()
        {
            //Arrange

            var waitingAppointments = new List<Appointment> { };

            var completeAppointments = new List<Appointment> { };

            DateTime today = new DateTime(2017, 5, 12);

            mockAppRepo
                .Setup(e => e.GetByDayAndMonthAndYearAndStatus(today.Day, today.Month, today.Year, "Waiting"))
                .Returns(waitingAppointments
                .ToList());
            mockAppRepo
                .Setup(e => e.GetByDayAndMonthAndYearAndStatus(today.Day, today.Month, today.Year, "Complete"))
                .Returns(completeAppointments
                .ToList());

            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);

            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            context.SetupGet(x => x.Session["Authen"]).Returns(true);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);

            // Act 

            var result = controllerUnderTest.Index(today.Day, today.Month, today.Year) as ViewResult;

            var model = result.Model as AllAppointmentViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(today, model.date);
            Assert.AreEqual(0, model.AppCom.Count());
            Assert.AreEqual(0, model.AppWait.Count());
        }

        [TestMethod]
        public void TestVaSettingPass()
        {// Arrange
            Clinic MC = new Clinic { maximumCase = 1 };
            //  var mockVCRepository = new Mock<IVCRepository>();
            mockVCRepo
                .Setup(e => e.Get())
                .Returns(MC);
            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            context.SetupGet(x => x.Session["Authen"]).Returns(true);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);

            //Act
            var result = controllerUnderTest.VASetting() as ViewResult;
            var model = result.Model as Clinic;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestVaSettingFail()
        {
            //Arrange
            //   var mockAppointmentRepository = new Mock<IAppointmentRepository>();
            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            context.SetupGet(x => x.Session["Authen"]).Returns(true);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);

            //Act
            var vafalse = controllerUnderTest.VASetting("#@!$!@#!@#") as JsonResult;
            var result = vafalse.Data.ToString();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("{ Result = Fail, maximum case can only numeric character }", result);

        }

        [TestMethod]
        public void TestGetAllmember()
        {// Arrange
            var members = new List<Member> {
                 new Member {id = 1 , name = "one"},
                  new Member {id = 2 , name = "two"},
                   new Member {id = 3 , name = "three"}
            };

            mockMemRepo.Setup(e => e.GetAll()).Returns(members.ToList());

            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);

            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            context.SetupGet(x => x.Session["Authen"]).Returns(true);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);


            // Act 

            var result = controllerUnderTest.Member(null, "a") as ViewResult;

            var model = result.Model as IEnumerable<Member>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, model.Count());
        }


        [TestMethod]
        public void TestSearchMemberByEmail()
        {// Arrange
            var membersEmail = new List<Member>
            {
                new Member {id = 1 , name = "one",email ="mail1"},
                  new Member {id = 2 , name = "two", email ="mail2"}
            };


            mockMemRepo.Setup(e => e.GetByEmail("mail")).Returns(membersEmail.ToList());

            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);

            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            context.SetupGet(x => x.Session["Authen"]).Returns(true);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);

            // Act 
            var result = controllerUnderTest.Member("email", "mail") as ViewResult;
            var model = result.Model as IEnumerable<Member>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public void TestSearchMemberByName()
        {// Arrange
            var membersName = new List<Member>
            {
                new Member {id = 1 , name = "one",email ="mail1"},
                  new Member {id = 2 , name = "two", email ="mail2"}
            };

            mockMemRepo.Setup(e => e.GetByName("testname")).Returns(membersName.ToList());

            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);

            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            context.SetupGet(x => x.Session["Authen"]).Returns(true);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);

            // Act 
            var result = controllerUnderTest.Member("name", "testname") as ViewResult;
            var model = result.Model as IEnumerable<Member>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Count());
        }


        [TestMethod]

        public void TestEditPetSpecieNameEmpty()
        {
            //Arrange
            //   var mockPetRepository = new Mock<IPetSpecieRepository>();
            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            context.SetupGet(x => x.Session["Authen"]).Returns(true);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);

            //Act
            var vafalse = controllerUnderTest.EditSpecie("1", null) as JsonResult;
            var result = vafalse.Data.ToString();

            // Assert
            Assert.AreEqual(true, (Boolean)(controllerUnderTest.Session["Authen"]));
            Assert.IsNotNull(result);
            Assert.AreEqual("{ Result = Fail, specie name is required }", result);

        }

        [TestMethod]
        public void AddSpecie_Without_Name()
        {
            // Arrange
            HomeController controlerUndertest = new HomeController();

            //Act
            var vafail = controlerUndertest.AddSpecie(null) as JsonResult;
            var result = vafail.Data.ToString();

            //Assert
            Assert.AreEqual("{ Result = Fail, specie name is required }", result);
        }

        [TestMethod]
        public void AddSpecie_With_Name_Arealdy_Exits()
        {
            var species = new PetType { id = 1, name = "dog" };

            // Arrange
            mockSpecieRepo
               .Setup(e => e.GetByName("dog"))
               .Returns(species);

            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);

            //Act
            var vafail = controllerUnderTest.AddSpecie("dog") as JsonResult;
            var result = vafail.Data.ToString();

            //Assert
            Assert.AreEqual("{ Result = Fail, this specie is already exits in the system }", result);
        }


        [TestMethod]
        public void TestLoginPass()
        {

            // Arrange
            var admin = new Administrator() { id = 1, username = "testusername", password = "testpassword" };
            mockAdminRepo.Setup(e => e.GetByUsernamrAndPassword("testusername", "testpassword")).Returns(admin);
            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);

            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();


            Boolean sessionValue = false;
            context.SetupSet(s => s.Session["Authen"] = It.IsAny<Boolean>())
                .Callback((string name, object val) => sessionValue = (Boolean)val); ;
            context.SetupGet(s => s.Session["Authen"]).Returns(() => sessionValue);
            context.Setup(x => x.Session).Returns(session.Object);

            string usernameValue = "somename";
            context.SetupSet(s => s.Session["username"] = It.IsAny<string>())
                .Callback((string name, object val) => usernameValue = (string)val); ;
            context.SetupGet(s => s.Session["username"]).Returns(() => usernameValue);


            var requestContext = new RequestContext(context.Object, new RouteData());
            controllerUnderTest.ControllerContext = new ControllerContext(requestContext, controllerUnderTest);


            // Act 
            var testAdmin = new Administrator() { username = "testusername", password = "testpassword" };
            var result = controllerUnderTest.Login(testAdmin) as RedirectToRouteResult;

            // Assert
            //    Assert.AreEqual(true,(controllerUnderTest.Session["Authen"]));
            Assert.IsNotNull(result);
            Assert.IsTrue((Boolean)controllerUnderTest.Session["Authen"]);
            Assert.AreEqual("testusername", controllerUnderTest.Session["username"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestLoginFail()
        {

            // Arrange
            var admin = new Administrator() { id = 1, username = "testusername", password = "testpassword" };
            mockAdminRepo.Setup(e => e.GetByUsernamrAndPassword("testusername", "testpassword")).Returns(admin);
            var controllerUnderTest = new HomeController(mockAppRepo.Object, mockAdminRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object);

            // Act 
            var testAdmin = new Administrator() { username = "ds", password = "sd" };
            var result = controllerUnderTest.Login(testAdmin) as RedirectToRouteResult;

            // Assert
            Assert.IsNull(result);
        }



        /*       [TestMethod]
               public void LoginWithExitsUsernameAndPassword()
               {
                   // Arrange
                   var mockAdministratorRepository = new Mock<IAdministratorRepository>();
                   var admin = new Administrator { username = "a", password = "b" };
                   mockAdministratorRepository
                    .Setup(e => e.GetByUsernamrAndPassword("a", "b"))
                    .Returns(admin);

                   var controllerUnderTest = new HomeController(mockAdministratorRepository.Object);

                   // Act
                   var result = controllerUnderTest.Login("a","b") as ViewResult;

                   var model = result.Model as AllAppointmentViewModel;

                   // Assert

               }
               */
        [TestMethod]
        public void name()
        {// Arrange
            // Act
            // Assert
        }


    }
}
