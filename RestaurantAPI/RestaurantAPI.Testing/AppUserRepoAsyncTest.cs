using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Library.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RestaurantAPI.Testing
{
    public class AppUserRepoAsyncTest
    {
        public AppUserRepoAsyncTest()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB")
                .Options;
            using (var context = new Project2DBContext(options))
            {
                //Only initialize the DB with data once
                if (context.AppUser.ToList().Count == 0)
                {
                    context.AppUser.Add(new AppUser { Username = "realUser", FirstName = "a", LastName = "b", Email = "e" });
                    context.AppUser.Add(new AppUser { Username = "decoyUser1", FirstName = "a", LastName = "b", Email = "e" });
                    context.AppUser.Add(new AppUser { Username = "decoyUser2", FirstName = "a", LastName = "b", Email = "e" });
                    context.AppUser.Add(new AppUser { Username = "decoyUser3", FirstName = "a", LastName = "b", Email = "e" });

                    context.Restaurant.Add(new Restaurant { Id = 1, Name = "1", Location = "loc", Owner = "realUser" });
                    context.Restaurant.Add(new Restaurant { Id = 2, Name = "2", Location = "loc" });
                    context.Restaurant.Add(new Restaurant { Id = 3, Name = "3", Location = "loc" });
                    context.Restaurant.Add(new Restaurant { Id = 4, Name = "4", Location = "loc" });
                    context.Restaurant.Add(new Restaurant { Id = 5, Name = "5", Location = "loc", Owner = "realUser" });
                    context.Restaurant.Add(new Restaurant { Id = 6, Name = "6", Location = "loc" });
                    context.Restaurant.Add(new Restaurant { Id = 7, Name = "7", Location = "loc" });
                    context.Restaurant.Add(new Restaurant { Id = 8, Name = "8", Location = "loc" });
                    context.Restaurant.Add(new Restaurant { Id = 9, Name = "9", Location = "loc" });


                    context.Blacklist.Add(new Blacklist { Username = "realUser", RestaurantId = 2 });
                    context.Blacklist.Add(new Blacklist { Username = "realUser", RestaurantId = 4 });
                    context.Blacklist.Add(new Blacklist { Username = "realUser", RestaurantId = 6 });

                    context.Favorite.Add(new Favorite { Username = "realUser", RestaurantId = 1 });
                    context.Favorite.Add(new Favorite { Username = "realUser", RestaurantId = 3 });
                    context.Favorite.Add(new Favorite { Username = "realUser", RestaurantId = 5 });
                    context.Favorite.Add(new Favorite { Username = "realUser", RestaurantId = 7 });

                    context.Query.Add(new Query { Id = 1, Username = "realUser", QueryTime = DateTime.Now });
                    context.Query.Add(new Query { Id = 2, Username = "realUser", QueryTime = DateTime.Now });
                    context.Query.Add(new Query { Id = 3, Username = "realUser", QueryTime = DateTime.Now });
                    context.Query.Add(new Query { Id = 4, Username = "realUser", QueryTime = DateTime.Now });
                    context.Query.Add(new Query { Id = 5, Username = "realUser", QueryTime = DateTime.Now });
                    context.Query.Add(new Query { Id = 6, Username = "realUser", QueryTime = DateTime.Now });
                    context.Query.Add(new Query { Id = 7, Username = "realUser", QueryTime = DateTime.Now });
                    context.Query.Add(new Query { Id = 8, Username = "realUser", QueryTime = DateTime.Now });

                    context.QueryKeywordJunction.Add(new QueryKeywordJunction { QueryId = 1, Word = "breakfast" });
                    context.QueryKeywordJunction.Add(new QueryKeywordJunction { QueryId = 1, Word = "fast" });
                    context.QueryKeywordJunction.Add(new QueryKeywordJunction { QueryId = 1, Word = "food" });

                    context.SaveChanges();
                }
            }
        }


        //Testing of DBContainsUsernameAsync
        [Fact]
        public void DBContainsUsernameAsyncShouldNotThrowExceptionIfDBIsEmpty()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB")
                .Options;

            bool result = true;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                uRepo.DBContainsUsernameAsync("LiterallyAnything");
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void DBContainsUsernameAsyncShouldReturnFalseIfIfDBIsEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB")
                .Options;
            bool result;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                result = uRepo.DBContainsUsernameAsync(username).Result;
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        public void DBContainsUsernameAsyncShouldReturnTrueIfUsernameInDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB")
                .Options;
            bool result;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                result = uRepo.DBContainsUsernameAsync(username).Result;
            }
            //Assert
            Assert.True(result);

        }

        [Theory]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void DBContainsUsernameAsyncShouldReturnFalseIfUsernameNotInDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB")
                .Options;
            bool result;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                result = uRepo.DBContainsUsernameAsync(username).Result;
            }
            //Assert
            Assert.False(result);
        }

    }
}
