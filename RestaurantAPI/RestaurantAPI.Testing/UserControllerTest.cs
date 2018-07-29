﻿using Microsoft.AspNetCore.Mvc;
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
            mockKRepo = new Mock<KeywordRepo>();
            mockQRepo = new Mock<QueryRepo>();
            mockRRepo = new Mock<IRestaurantRepo>();
            controller = new UserController(
               mockARepo.Object, mockKRepo.Object, mockQRepo.Object, mockRRepo.Object);
        }

        Mock<IAppUserRepo> mockARepo;
        Mock<KeywordRepo> mockKRepo;
        Mock<QueryRepo> mockQRepo;
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
