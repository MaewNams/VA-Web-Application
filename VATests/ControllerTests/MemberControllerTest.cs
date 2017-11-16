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
using Microsoft.CSharp;

namespace VATests.ControllerTests
{
    [TestClass]
    public class MemberControllerTest
    {
        public MemberControllerTest()
        {
            // create some mock products to play with
            IEnumerable<Appointment> appointments = new List<Appointment> { };
            IEnumerable<Member> members = new List<Member> { };
            IEnumerable<Pet> pets = new List<Pet> { };
            IEnumerable<Specie> species = new List<Specie> { };
            IEnumerable<TimeSlot> timeslots = new List<TimeSlot> { };
            IEnumerable<AppointmentTimeSlot> apptimes = new List<AppointmentTimeSlot> { };
            IEnumerable<VAService> VAServices = new List<VAService> { };
            Clinic clinic = new Clinic();

            // Mock the Repository using Moq
            Mock<IAppointmentRepository> mockAppRepo = new Mock<IAppointmentRepository>();
            Mock<IMemberRepository> mockMemRepo = new Mock<IMemberRepository>();
            Mock<IPetRepository> mockPetRepo = new Mock<IPetRepository>();
            Mock<ISpecieRepository> mockSpecieRepo = new Mock<ISpecieRepository>();
            Mock<IVCRepository> mockVCRepo = new Mock<IVCRepository>();
            Mock<ITimeSlotRepository> mockTimeSlotRepo = new Mock<ITimeSlotRepository>();
            Mock<IAppTimeRepository> mockAppTimeRepo = new Mock<IAppTimeRepository>();
            Mock<IServiceRepository> mockServiceRepo = new Mock<IServiceRepository>();

            // Return the admin
            mockAppRepo.Setup(e => e.GetByPetID(1)).Returns(appointments.ToList());

            mockMemRepo.Setup(e => e.GetAll()).Returns(members.ToList());

            mockPetRepo.Setup(e => e.GetByMemberID(1)).Returns(pets.ToList());

            mockSpecieRepo.Setup(e => e.GetAll()).Returns(species.ToList());

            mockVCRepo.Setup(e => e.Get()).Returns(clinic);

            mockTimeSlotRepo.Setup(e => e.GetListByDate(1,2,2017)).Returns(timeslots.ToList());

            mockAppTimeRepo.Setup(e => e.GetByAppointmentID(1)).Returns(apptimes);

            mockServiceRepo.Setup(e => e.GetAll()).Returns(VAServices.ToList());

            // Complete the setup of our Mock 

            this.mockAppRepo = mockAppRepo;
            this.mockMemRepo = mockMemRepo;
            this.mockPetRepo = mockPetRepo;
            this.mockSpecieRepo = mockSpecieRepo;
            this.mockVCRepo = mockVCRepo;
            this.mockTimeSlotRepo = mockTimeSlotRepo;
            this.mockAppTimeRepo = mockAppTimeRepo;
            this.mockServiceRepo = mockServiceRepo;
        }

        public readonly Mock<IAppointmentRepository> mockAppRepo;
        public readonly Mock<IMemberRepository> mockMemRepo;
        public readonly Mock<IPetRepository> mockPetRepo;
        public readonly Mock<ISpecieRepository> mockSpecieRepo;
        public readonly Mock<IVCRepository> mockVCRepo;
        public readonly Mock<ITimeSlotRepository> mockTimeSlotRepo;
        public readonly Mock<IAppTimeRepository> mockAppTimeRepo;
        public readonly Mock<IServiceRepository> mockServiceRepo;

        [TestMethod]
        public void TestMemberIndex()
        {// Arrange
            var member = new Member { id = 1, name = "one", surname = "someone",
                address="t1 address",email = "one@mail.com", phonenumber="011111111" };


            mockMemRepo.Setup(e => e.GetByID(1)).Returns(member);

            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);

            // Act 
            var result = controllerUnderTest.Index(1,null,null) as ViewResult;
            var model = result.Model as MemberAppointmentViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.member.id);
            Assert.AreEqual("one", model.member.name);
            Assert.AreEqual("someone", model.member.surname);
            Assert.AreEqual("one@mail.com", model.member.email);
            Assert.AreEqual("t1 address", model.member.address);
            Assert.AreEqual("011111111", model.member.phonenumber);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
        }

        [TestMethod]
        public void TestMemberPet()
        {// Arrange
            var member = new Member
            {
                id = 1,
                name = "one",
                surname = "someone",
                address = "t1 address",
                email = "one@mail.com",
                phonenumber = "011111111"
            };
            var species = new List<Specie> {
                 new Specie {id = 1 , name = "cat"},
                  new Specie {id = 2 , name = "dog"},
                   new Specie {id = 3 , name = "rabbit"},
                    new Specie {id = 4 , name = "fish"},
            };
            var pets = new List<Pet> {
                 new Pet {id = 1 , name = "oneCat"},
                  new Pet {id = 2 , name = "oneDog"},
            };


            mockMemRepo.Setup(e => e.GetByID(1)).Returns(member);
            mockPetRepo.Setup(e => e.GetByMemberID(1)).Returns(pets);
            mockSpecieRepo.Setup(e => e.GetAll()).Returns(species);


            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);

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
            var result = controllerUnderTest.MemberPet(1) as ViewResult;
            var model = result.Model as MemberPetViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.member.id);
            Assert.AreEqual(2, model.pets.Count());
            Assert.AreEqual(4, model.species.Count());
        //    Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void TestCreateMemberCase1_success()
        {// Arrange
            var member = new Member
            {
                id = 1,
                name = "one",
                surname = "someone",
                address = "t1 address",
                email = "one@mail.com",
                phonenumber = "011111111"
            };
            mockMemRepo.Setup(e => e.GetByNameAndSurname("one", "someone")).Returns(member);
            mockMemRepo.Setup(e => e.GetByExactlyEmail("one@mail.com")).Returns(member);


            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.CreateMember("chonticha","chang","earth","ruttikar_kawaii@gmail.com","0956786809") as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Success, newMemberId = 0 }", result);
        }

        [TestMethod]
        public void TestCreateMemberCase2_invalid_email()
        {// Arrange
            var member = new Member
            {
                id = 1,
                name = "one",
                surname = "someone",
                address = "t1 address",
                email = "one@mail.com",
                phonenumber = "011111111"
            };
            mockMemRepo.Setup(e => e.GetByNameAndSurname("one", "someone")).Returns(member);
            mockMemRepo.Setup(e => e.GetByExactlyEmail("one@mail.com")).Returns(member);


            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.CreateMember("chonticha", "chang", "earth", "555", "0956786809") as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Fail, email address is in invalid format, the valid format of email is xxx@xxx.xx }", result);
        }


        [TestMethod]
        public void TestEditMemberCase1_success()
        {// Arrange
            var member = new Member
            {
                id = 1,
                name = "one",
                surname = "someone",
                address = "t1 address",
                email = "one@mail.com",
                phonenumber = "011111111"
            };
            mockMemRepo.Setup(e => e.GetByNameAndSurname("one", "someone")).Returns(member);
            mockMemRepo.Setup(e => e.GetByID(2)).Returns(member);
            mockMemRepo.Setup(e => e.GetByExactlyEmail("one@mail.com")).Returns(member);

            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.Edit(2,"Thicha", "Chang", "ruttikar_kawaii@gmail.com", "Jupiter", "0956786809") as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Success }", result);
        }

        [TestMethod]
        public void TestEditMemberCase2_exits_email()
        {// Arrange
            var member = new Member
            {
                id = 1,
                name = "one",
                surname = "someone",
                address = "t1 address",
                email = "one@mail.com",
                phonenumber = "011111111"
            };
            mockMemRepo.Setup(e => e.GetByNameAndSurname("one", "someone")).Returns(member);
            mockMemRepo.Setup(e => e.GetByExactlyEmail("one@mail.com")).Returns(member);
            mockMemRepo.Setup(e => e.GetByID(2)).Returns(member);
            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.Edit(2, "Thicha", "Chang", "one@mail.com", "Jupiter", "0956786809") as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Fail, the email already exits in the system }", result);
        }

        [TestMethod]
        public void TestResetPassword_success()
        {// Arrange
            var member = new Member
            {
                id = 1,
                name = "one",
                surname = "someone",
                address = "t1 address",
                email = "one@mail.com",
                phonenumber = "011111111"
            };
        
            mockMemRepo.Setup(e => e.GetByID(1)).Returns(member);
 

            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.ResetPassword(1) as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Success }", result);
        }

        [TestMethod]
        public void TestDeleteMember_success()
        {// Arrange
            var member = new Member
            {
                id = 1,
                name = "one",
                surname = "someone",
                address = "t1 address",
                email = "one@mail.com",
                phonenumber = "011111111"
            };

            mockMemRepo.Setup(e => e.GetByID(1)).Returns(member);


            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.Delete(1) as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Success }", result);
        }

        [TestMethod]
        public void TestCreatePetCase1_success()
        {// Arrange
            var pet = new Pet
            {
                id = 2,
                memberId = 1,
                name = "oneDog",
                specieId = 1,
            };

         //   mockPetRepo.Setup(e => e.GetById(1)).Returns(pet);
            mockPetRepo.Setup(e => e.GetByMemberIDAndNameAndSpecie(1, "oneDog", 1)).Returns(pet);

            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.CreatePet(1,1,"Lukpar") as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Success }", result);
        }

        [TestMethod]
        public void TestCreatePetCase2_Fail_exits()
        {// Arrange
            var pet = new Pet
            {
                id = 2,
                memberId = 1,
                name = "oneDog",
                specieId = 1,
            };

            var specie = new Specie { id =1, name = "dog" };

            //   mockPetRepo.Setup(e => e.GetById(1)).Returns(pet);
            mockPetRepo.Setup(e => e.GetByMemberIDAndNameAndSpecie(1, "oneDog", 1)).Returns(pet);
            mockSpecieRepo.Setup(e => e.GetById(1)).Returns(specie);
            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.CreatePet(1, 1, "oneDog") as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Fail, The member already have dog name oneDog }", result);
        }


        [TestMethod]
        public void TestEditPetCase1_success()
        {// Arrange
            var pet = new Pet
            {
                id = 2,
                memberId = 1,
                name = "oneDog",
                specieId = 1,
            };

            mockPetRepo.Setup(e => e.GetById(1)).Returns(pet);
            mockPetRepo.Setup(e => e.GetByMemberIDAndNameAndSpecie(1, "oneDog", 1)).Returns(pet);

            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.EditPet(1, 3, "Jellopy") as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Success }", result);
        }

        [TestMethod]
        public void TestEditPetCase2_fail_noName()
        {// Arrange
            var pet = new Pet
            {
                id = 2,
                memberId = 1,
                name = "oneDog",
                specieId = 1,
            };

            mockPetRepo.Setup(e => e.GetById(1)).Returns(pet);
            mockPetRepo.Setup(e => e.GetByMemberIDAndNameAndSpecie(1, "oneDog", 1)).Returns(pet);

            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.EditPet(1, 3, "") as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Fail, name is required }", result);
        }

        [TestMethod]
        public void TestDeletePet_success()
        {// Arrange
            var pet = new Pet
            {
                id = 1,
                name = "oneCat"
            };

            mockPetRepo.Setup(e => e.GetById(1)).Returns(pet);


            var controllerUnderTest = new MemberController(mockAppRepo.Object, mockMemRepo.Object, mockPetRepo.Object, mockSpecieRepo.Object, mockVCRepo.Object, mockTimeSlotRepo.Object, mockAppTimeRepo.Object, mockServiceRepo.Object);
            // Act 
            var message = controllerUnderTest.DeletePet(1) as JsonResult;
            var result = message.Data.ToString();
            // Assert
            Assert.AreEqual("{ Result = Success }", result);
        }
    }
}
