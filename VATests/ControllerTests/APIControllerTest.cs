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
    public class APIControllerTest
    {
        public APIControllerTest()
        {
            // create some mock products to play with
            IEnumerable<Administrator> admins = new List<Administrator> { };
            IEnumerable<Appointment> appointments = new List<Appointment> { };
            IEnumerable<Member> members = new List<Member> { };
            IEnumerable<Pet> pets = new List<Pet> { };
            IEnumerable<Specie> species = new List<Specie> { };
            Clinic clinic = new Clinic();

            // Mock the Repository using Moq
            Mock<IAdministratorRepository> mockAdminRepo = new Mock<IAdministratorRepository>();
            Mock<IAppointmentRepository> mockAppRepo = new Mock<IAppointmentRepository>();
            Mock<IMemberRepository> mockMemRepo = new Mock<IMemberRepository>();
            Mock<IPetRepository> mockPetRepo = new Mock<IPetRepository>();
            Mock<ISpecieRepository> mockSpecieRepo = new Mock<ISpecieRepository>();
            Mock<IVCRepository> mockVCRepo = new Mock<IVCRepository>();

            // Return the admin
            mockAdminRepo
                .Setup(mr => mr.GetByUsernamrAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string u, string p) => admins.Where(x => x.username == u && x.password == p)
                .FirstOrDefault());

            mockAppRepo.Setup(e => e.GetByPetID(1)).Returns(appointments.ToList());

            mockMemRepo.Setup(e => e.GetAll()).Returns(members.ToList());

            mockPetRepo.Setup(e => e.GetByMemberID(1)).Returns(pets.ToList());

            mockSpecieRepo.Setup(e => e.GetAll()).Returns(species.ToList());

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
        public readonly Mock<ISpecieRepository> mockSpecieRepo;
        public readonly Mock<IVCRepository> mockVCRepo;
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
