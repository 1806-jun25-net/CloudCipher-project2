using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Library.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantAPI.Testing
{
    public class AppUserRepoTest
    {
        //Testing of GetUsers()
        [Fact]
        public void GetUsersShouldNotThrowExceptionIfDBIsEmpty()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "GetUsersExceptionThrowDB")
                .Options;

            bool result = true;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                //context.Add(new AppUser { Username = "realUser", FirstName= "a", LastName= "b", Email= "e" });
                try
                {
                    uRepo.GetUsers();
                }
                catch (Exception e)
                {
                    result = false;
                }
            }
            //Assert
            Assert.True(result);
        }



        //Testing of GetUserByUsername
        [Fact]
        public void GetUserByUsernameShouldThrowExceptionIfUsernameNotFound()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "GetUserUsernameExceptionThrowDB")
                .Options;

            AppUser u;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                //context.Add(new AppUser { Username = "realUser", FirstName= "a", LastName= "b", Email= "e" });
                try
                {
                    u = uRepo.GetUserByUsername("__fake testing username__");
                }
                catch (NotSupportedException e)
                {
                    result = true;
                }
            }
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetUserByUsernameShouldNotThrowExceptionIfUsernameIsInDB()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "GetUserUsernameExceptionThrowDB")
                .Options;

            AppUser u;
            bool result = true;
            AppUserRepo uRepo;
            using (var context = new Project2DBContext(options))
            {
                context.AppUser.Add(new AppUser { Username = "realUser", FirstName = "a", LastName = "b", Email = "e" });
                context.SaveChanges();
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    u = uRepo.GetUserByUsername("realUser");

                }
                catch (Exception e)
                {
                    result = false;
                }
            }

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetUserByUsernameShouldReturnUserWithMatchingUsername()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "GetUserUsernameCorrectDB")
                .Options;

            AppUser u;
            bool result = true;
            AppUserRepo uRepo;
            using (var context = new Project2DBContext(options))
            {
                context.AppUser.Add(new AppUser { Username = "realUser", FirstName = "a", LastName = "b", Email = "e" });
                context.AppUser.Add(new AppUser { Username = "decoyUser1", FirstName = "a", LastName = "b", Email = "e" });
                context.AppUser.Add(new AppUser { Username = "decoyUser2", FirstName = "a", LastName = "b", Email = "e" });
                context.AppUser.Add(new AppUser { Username = "decoyUser3", FirstName = "a", LastName = "b", Email = "e" });
                context.SaveChanges();
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                u = uRepo.GetUserByUsername("realUser");
            }

            //Assert
            Assert.Equal("realUser", u.Username);
        }


    }
}
