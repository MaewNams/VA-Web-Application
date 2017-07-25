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

namespace VATests.Test.Models
{
    [TestClass]
    public class AdminRepoTests
    {

        public AdminRepoTests()
        {
            // create some mock products to play with
            IEnumerable<Administrator> admins = new List<Administrator>
                {
                    new Administrator { username = "test1", password = "test01" },
                    new Administrator { username = "test2", password = "test02" }
                };

            // Mock the Products Repository using Moq
            Mock<IAdministratorRepository> mockProductRepository = new Mock<IAdministratorRepository>();


            // Return the admin
            mockProductRepository
                .Setup(mr => mr.GetByUsernamrAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string u, string p) => admins.Where(x => x.username == u && x.password ==p)
                .FirstOrDefault());
            // Complete the setup of our Mock Product Repository
            this.MockProductsRepository = mockProductRepository.Object;
        }
        public readonly IAdministratorRepository MockProductsRepository;


        [TestMethod]
        public void GetValidUsernameAndValidPassword()
        {     // Try finding all products
            Administrator testProducts = MockProductsRepository.GetByUsernamrAndPassword("test1","test01");
            Assert.IsNotNull(testProducts); // Test if null
           // Assert.AreEqual(2, testProducts.username.); // Verify the correct Number
        }

        [TestMethod]
        public void GetByInValidUsernameAndValidPassword()
        {     // Try finding all products
            Administrator testProducts = MockProductsRepository.GetByUsernamrAndPassword("test1", "00");
            Assert.IsNull(testProducts); 
        }

        [TestMethod]
        public void GetByValidUsernameAndInvalidPassword()
        {     // Try finding all products
            Administrator testProducts = MockProductsRepository.GetByUsernamrAndPassword("12321", "test01");
            Assert.IsNull(testProducts);
        }

        [TestMethod]
        public void GetByInValidUsernameAndInValidPassword()
        {     // Try finding all products
            Administrator testProducts = MockProductsRepository.GetByUsernamrAndPassword("12321", "test01");
            Assert.IsNull(testProducts); 
        }
    }
}
