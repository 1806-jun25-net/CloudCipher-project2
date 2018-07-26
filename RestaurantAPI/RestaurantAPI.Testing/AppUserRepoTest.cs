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
        public AppUserRepoTest()
        {

        }

        [Fact]
        public void GetUserByUsernameAsyncShouldThrowExceptionIfUsernameNotFound()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryTestingDB")
                .Options;

            AppUser u;
            bool result = false;
            // Run the test against one instance of the context
            using (var context = new Project2DBContext(options))
            {
                var uRepo = new AppUserRepo(context);
                //context.Add(new AppUser { Username = "realUser", FirstName= "a", LastName= "b", Email= "e" });
                try
                {
                    var t = uRepo.GetUserByUsernameAsync("__fake testing username__");
                    t.Wait();
                    u = t.Result;
                }
                catch (AggregateException e)
                {
                    result = true;
                }
            }

            Assert.True(result);
        }

        [Fact]
        public void GetUserByUsernameAsyncShouldNotThrowExceptionIfUsernameIsInDB()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryTestingDB")
                .Options;

            AppUser u;
            bool result = true;
            // Run the test against one instance of the context
            using (var context = new Project2DBContext(options))
            {
                var uRepo = new AppUserRepo(context);
                context.Add(new AppUser { Username = "realUser", FirstName= "a", LastName= "b", Email= "e" });
                try
                {
                    var t = uRepo.GetUserByUsernameAsync("realUser");
                    t.Wait();
                    u = t.Result;
                }
                catch (Exception e)
                {
                    result = false;
                }
            }

            Assert.True(result);
        }
    }
}
