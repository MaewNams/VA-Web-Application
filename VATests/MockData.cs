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
    class MockData
    {
        public MockData()
        {
            // create some mock products to play with
            IEnumerable<Administrator> admins = new List<Administrator> { };
            IEnumerable<Appointment> appointments = new List<Appointment> { };
            IEnumerable<Member> members = new List<Member> { };
            IEnumerable<Pet> pets = new List<Pet> { };
            IEnumerable<PetType> types = new List<PetType> { };
            Clinic clinic = new Clinic();

            // Mock the Products Repository using Moq
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

            mockAppRepo.Setup(e => e.GetAll())
              .Returns(appointments
                 .ToList());

            mockMemRepo.Setup(e => e.GetAll())
             .Returns(members
             .ToList());

            mockPetRepo.Setup(e => e.GetAll())
               .Returns(pets
               .ToList());

            mockSpecieRepo.Setup(e => e.GetAll())
               .Returns(types
               .ToList());

            mockVCRepo.Setup(e => e.Get())
                .Returns(clinic);

            // Complete the setup of our Mock Product Repository



        }
        public readonly IAdministratorRepository MockAdminRepo;

        /*     public readonly mockAdminRepo;
             public readonly mockAppRepo;
             public readonly  mockMemRepo;
             public readonly mockPetRepo;
             public readonly  mockSpecieRepo;
             public readonly mockVCRepo;*/
    }
}
