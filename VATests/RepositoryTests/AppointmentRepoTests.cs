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


namespace VATests.RepositoryTests
{
    [TestClass]
    public class AppointmentRepoTests
    { 
        public AppointmentRepoTests() { 
        // create some mock products to play with
        IEnumerable<Appointment> appointments = new List<Appointment>
                {
                    new Appointment { id=1, memberId = 1, petId =1, serviceId =1, detail ="This is appointment 1", suggestion = "none", startTime =new DateTime(1900, 1, 1),endTime =new DateTime(1900, 1, 1), status  = "Waiting" },
                       new Appointment { id=2, memberId = 1, petId =1, serviceId =2, detail ="Thi is appointment 2", suggestion = "none", startTime =new DateTime(1900, 1, 1),endTime =new DateTime(1900, 1, 1), status  = "Waiting" }
                };

        // Mock the Products Repository using Moq
        Mock<IAppointmentRepository> mockAppointmentRepository = new Mock<IAppointmentRepository>();

            // Return the appointment
            mockAppointmentRepository.Setup(mr => mr.GetById(It.IsAny<int>()))
                .Returns((int x) => appointments.Where(a => a.id == x).Single());

            // Return the appointment
            mockAppointmentRepository.Setup(mr => mr.GetByPetID(It.IsAny<int>()))
                .Returns((int x) => appointments.Where(a => a.petId == x));
            // Complete the setup of our Mock Product Repository
            this.MockAppointmentsRepository = mockAppointmentRepository.Object;
            }
    public readonly IAppointmentRepository MockAppointmentsRepository;


    [TestMethod]
        public void GetByExitsPetID()
        {
            // Try finding a product by id
            IEnumerable<Appointment> testAppointment = MockAppointmentsRepository.GetByPetID(1);

            Assert.IsNotNull(testAppointment); // Test if null
          //  Assert.IsInstanceOfType(testAppointment, typeof(Appointment)); // Test type
            Assert.AreEqual(2, testAppointment.Count()); // Verify it is the right product
        }

        [TestMethod]
        public void GetByID()
        {
            // Try finding a product by id
            Appointment testAppointment = MockAppointmentsRepository.GetById(1);

            Assert.IsNotNull(testAppointment); // Test if null
            Assert.IsInstanceOfType(testAppointment, typeof(Appointment)); // Test type
            Assert.AreEqual("This is appointment 1", testAppointment.detail); // Verify it is the right product
        }
    }
}
