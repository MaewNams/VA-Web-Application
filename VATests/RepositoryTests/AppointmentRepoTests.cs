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
            IList<Appointment> appointments = new List<Appointment>
                {
                    new Appointment { id=1, memberId = 1, petId =1, serviceId =1, detail ="This is appointment 1", suggestion = "none", startTime =new DateTime(1900, 1, 1),endTime =new DateTime(1900, 1, 1), status  = "Waiting" },
                       new Appointment { id=2, memberId = 1, petId =1, serviceId =2, detail ="Thi is appointment 2", suggestion = "none", startTime =new DateTime(1900, 1, 1),endTime =new DateTime(1900, 1, 1), status  = "Waiting" }
                };

        // Mock the Products Repository using Moq
        Mock<IAppointmentRepository> mockAppointmentRepository = new Mock<IAppointmentRepository>();

            // Return all the appointment
          //  mockAppointmentRepository.Setup(mr => mr.GetAll()).Returns(appointments);

            // Return the appointment get by id
            mockAppointmentRepository.Setup(mr => mr.GetById(It.IsAny<int>()))
                .Returns((int x) => appointments.Where(a => a.id == x).SingleOrDefault());


            // Return the appointment
            mockAppointmentRepository.Setup(mr => mr.GetByPetID(It.IsAny<int>()))
                .Returns((int x) => appointments.Where(a => a.petId == x));
          


            // Allows us to test saving a product
            mockAppointmentRepository.Setup(mr => mr.Add(It.IsAny<Appointment>())
            ).Callback((Appointment newApp) =>
            {
                appointments.Add(newApp);
            });

            // Allows us to test saving a product
            mockAppointmentRepository.Setup(mr => mr.Delete(It.IsAny<Appointment>())
            ).Callback((Appointment newApp) =>
            {
                appointments.Remove(newApp);
            });
            // Complete the setup of our Mock Product Repository
            this.MockAppointmentsRepository = mockAppointmentRepository.Object;
            }
    public readonly IAppointmentRepository MockAppointmentsRepository;

      //  [TestMethod]
   /*     public void GetAll()
        {
            // Try finding all products
            IEnumerable<Appointment> testProducts = this.MockAppointmentsRepository.GetAll();

            Assert.IsNotNull(testProducts); // Test if null
            Assert.AreEqual(2, testProducts.Count()); // Verify the correct Number
        }*/

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

        [TestMethod]
        public void AddAppointment()
        {
            // Try finding a appointment by new id
            Appointment testAppointment = MockAppointmentsRepository.GetById(3);
            Assert.IsNull(testAppointment); // Test if null

            // Add appointment
            Appointment testadd = new Appointment() { id = 3, memberId = 1, petId = 1, serviceId = 1, detail = "This is appointment 3", suggestion = "none", startTime = new DateTime(1900, 1, 1), endTime = new DateTime(1900, 1, 1), status = "Waiting" };
            this.MockAppointmentsRepository.Add(testadd);


            // Try finding a appointment by new id again
            testAppointment = this.MockAppointmentsRepository.GetById(3);
            Assert.IsNotNull(testAppointment); // Test if null
            Assert.IsInstanceOfType(testAppointment, typeof(Appointment)); // Test type
            Assert.AreEqual("This is appointment 3", testAppointment.detail); // Verify it is the right product


            // Try finding all products
        //    IEnumerable<Appointment> testProducts = this.MockAppointmentsRepository.GetAll();

         //   Assert.IsNotNull(testProducts); // Test if null
         //   Assert.AreEqual(3, testProducts.Count()); // Verify the correct Number


        }

 

        [TestMethod]
        public void RemoveAppointment()
        {
            // Try finding a product by id
        //    Appointment testadd = new Appointment() { id = 3, memberId = 1, petId = 1, serviceId = 1, detail = "This is appointment 3", suggestion = "none", startTime = new DateTime(1900, 1, 1), endTime = new DateTime(1900, 1, 1), status = "Waiting" };
          //  this.MockAppointmentsRepository.Add(testadd);

            Appointment testAppointment = MockAppointmentsRepository.GetById(1);
            Assert.IsNotNull(testAppointment); // Test if null
                                               // Try finding all products
         //   IEnumerable<Appointment> appList = MockAppointmentsRepository.GetAll();
        //    Assert.AreEqual(2, appList.Count()); // Verify the correct Number

         this.MockAppointmentsRepository.Delete(testAppointment);

            Appointment testAppointment2 = MockAppointmentsRepository.GetById(1);
            Assert.IsNull(testAppointment2); // Test if null
  
            // Try finding all products
    //        IEnumerable<Appointment> appList2 = this.MockAppointmentsRepository.GetAll();
     //       Assert.AreEqual(1, appList2.Count()); // Verify the correct Number
            
        }
    }
}
