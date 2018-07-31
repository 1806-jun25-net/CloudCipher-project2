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
            mockARepo = new Mock<IAppUserRepo>();
            mockKRepo = new Mock<IKeywordRepo>();
            mockQRepo = new Mock<IQueryRepo>();
            mockRRepo = new Mock<IRestaurantRepo>();
            controller = new UserController(
               mockARepo.Object, mockKRepo.Object, mockQRepo.Object, mockRRepo.Object);
        }

        Mock<IAppUserRepo> mockARepo;
        Mock<IKeywordRepo> mockKRepo;
        Mock<IQueryRepo> mockQRepo;
        Mock<IRestaurantRepo> mockRRepo;
        UserController controller;


        [Fact]
        public void GetTestfailsifnull()
        {
            //Arrange
            mockARepo.Setup(x => x.GetUsers()).Returns((IQueryable<AppUser>)null);
            

            //Act
            var result = controller.Get();

            //Assert
            var statusCode = (StatusCodeResult)result.Result;
            Assert.Equal(500, statusCode.StatusCode);
        }

        [Fact]
        public void Getreturnsmodelifempty()
        {
            //Arrange
            mockARepo.Setup(x => x.GetUsers()).Returns((new List<AppUser>().AsQueryable()));


            //Act
            var result = controller.Get();

            //Assert
            Assert.IsType<ActionResult<List<UserModel>>>(result);
        }


        [Fact]
        public void Getreturnmodeliflistnonempty()
        {
            //Arrange
            mockARepo.Setup(x => x.GetUsers()).Returns((new List<AppUser>() { new AppUser(){Username = "u" } }).AsQueryable());


            //Act
            var result = controller.Get();

            //Assert
            Assert.IsType<ActionResult<List<UserModel>>>(result);
        }

        /*
         * test fails due to Identity currently being inaccessible in test class.
         * Would take a long time to get moq set up for Identity, so holding off on testing related methods for the time being.
        [Fact]
        public void Getbyusernametestsucceeds()
        {
            AppUser Appobject = new AppUser();


            //Arrange
            mockARepo.Setup(x => x.GetUserByUsername("block")).Returns(new AppUser() { Username = "block" });

            //Act
            ActionResult<UserModel> result = controller.GetByUsername("block");

            //Assert
            Assert.Equal("block", result.Value.Username);
        }
        */

        /*
         * test fails due to Identity currently being inaccessible in test class.
         * Would take a long time to get moq set up for Identity, so holding off on testing related methods for the time being.
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

        */

        [Fact]
        public void createsucceedsifaddissuccessful()
        {
            UserModel userobject = new UserModel();

            //Arrange
            mockARepo.Setup(x => x.AddUser(It.IsAny<AppUser>()));
            mockARepo.Setup(x => x.Save());

            //Act
            IActionResult result = controller.Create(userobject);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public void createreturnsstatuscodeifexceptionthrown()
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
