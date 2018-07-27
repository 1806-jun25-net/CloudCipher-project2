using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantAPI.API.Controllers;
using RestaurantAPI.API.Models;
using RestaurantAPI.Data;
using RestaurantAPI.Library.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;


namespace RestaurantAPI.Testing
{
    public class UserControllerTest
    {
        public UserControllerTest()
        {
            mockARepo = new Mock<AppUserRepo>();
            mockKRepo = new Mock<KeywordRepo>();
            mockQRepo = new Mock<QueryRepo>();
            mockRRepo = new Mock<RestaurantRepo>();
            controller = new UserController(
               mockARepo.Object, mockKRepo.Object, mockQRepo.Object, mockRRepo.Object);
        }

        Mock<AppUserRepo> mockARepo;
        Mock<KeywordRepo> mockKRepo;
        Mock<QueryRepo> mockQRepo;
        Mock<RestaurantRepo> mockRRepo;
        UserController controller;

        [Fact]
        public void GetTestpasses()
        {
            //Arrange
            mockARepo.Setup(x => x.GetUsers()).Returns((IQueryable<AppUser>)null);


            //Act
            ActionResult result = controller.Get();

            //Assert
            var statusCode = (StatusCodeResult)result;
            Assert.Equal(501, statusCode.StatusCode);
        }

        [Fact]
        public void GetTestfails()
        {
            //Arrange
            mockARepo.Setup(x => x.GetUsers()).Returns((IQueryable<AppUser>)null);


            //Act
            ActionResult result = controller.Get();

            //Assert
            var statusCode = (StatusCodeResult)result;
            Assert.Equal(501, statusCode.StatusCode);
        }


        [Fact]
        public void Getbyusernametestsucceeds()
        {
            AppUser Appobject = new AppUser();

            //Arrange
            mockARepo.Setup(x => x.GetUserByUsername("block")).Returns(Appobject);

            //Act
            ActionResult<UserModel> result = controller.GetByUsername("block");

            //Assert
            Assert.Equal("block", result.Value.Username);
        }

        [Fact]
        public void Getbyusernametestfails()
        {
            //Arrange
            mockARepo.Setup(x => x.GetUserByUsername("block")).Throws<NotSupportedException>();


            //Act
            ActionResult<UserModel> result = controller.GetByUsername("block");

            //Assert
            var statusCode = (StatusCodeResult)result.Result;
            Assert.Equal(400, statusCode.StatusCode);

        }

        [Fact]
        public void createtest()
        {
            UserModel userobject = new UserModel();

            //Arrange
            mockARepo.Setup(x => x.AddUser(It.IsAny<AppUser>()));
            mockARepo.Setup(x => x.Save());

            //Act
            IActionResult result = controller.Create(userobject);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void createtestfails()
        {
            //Arrange
            UserModel userobject = new UserModel();

            mockARepo.Setup(x => x.AddUser(It.IsAny<AppUser>())).Throws<Exception>();
            mockARepo.Setup(x => x.Save());

            //Act
            IActionResult result = controller.Create(userobject);

            //Assert
            var statusCode = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(400, statusCode.StatusCode);
        }
       /* [Fact]
        public void testtest()
        {
            //Arrange 
            UserModel userModel = new UserModel();

            //Act
            var result = controller.Create(userModel);

            //Assert
            

        }
        */
    }
}
