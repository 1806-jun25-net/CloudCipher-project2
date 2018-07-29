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
    public class KeywordRepoAsyncTest
    {
        public KeywordRepoAsyncTest()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordAsyncDB")
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

                    context.Keyword.Add(new Keyword { Word = "breakfast" });
                    context.Keyword.Add(new Keyword { Word = "fast" });
                    context.Keyword.Add(new Keyword { Word = "food" });

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

        //Testing of DBContainsKeywordAsync
        [Theory]
        [InlineData("breakfast")]
        [InlineData("fast")]
        [InlineData("food")]
        [InlineData("trash")]
        [InlineData("brick")]
        [InlineData("hardware")]
        public void DBContainsKeywordAsyncShouldNotThrowExceptionIfDBIsEmpty(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordAsyncDB2")
                .Options;

            bool result = true;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                kRepo.DBContainsKeywordAsync(kw).Wait();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("breakfast")]
        [InlineData("fast")]
        [InlineData("food")]
        [InlineData("trash")]
        [InlineData("brick")]
        [InlineData("hardware")]
        public void DBContainsKeywordAsyncShouldReturnFalseIfIfDBIsEmpty(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordAsyncDB3")
                .Options;
            bool result;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                result = kRepo.DBContainsKeywordAsync(kw).Result;
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("breakfast")]
        [InlineData("fast")]
        [InlineData("food")]
        public void DBContainsKeywordAsyncShouldReturnTrueIfRestaurantIdInDB(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordAsyncDB")
                .Options;
            bool result;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                result = kRepo.DBContainsKeywordAsync(kw).Result;
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("trash")]
        [InlineData("brick")]
        [InlineData("hardware")]
        public void DBContainsKeywordAsyncShouldReturnFalseIfKeywordNotInDB(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordAsyncDB")
                .Options;
            bool result;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                result = kRepo.DBContainsKeywordAsync(kw).Result;
            }
            //Assert
            Assert.False(result);
        }

        //Testing of AddKeywordAsync
        [Theory]
        [InlineData("food")]
        [InlineData("breakfast")]
        [InlineData("fast")]
        public void AddKeywordAsyncShouldThrowExceptionIfKeywordIsPreset(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordAddTestingAsync1DB")
                .Options;

            Keyword k = new Keyword { Word = kw };
            Keyword k2 = new Keyword { Word = kw };
            KeywordRepo kRepo;
            bool result = false;
            using (var context = new Project2DBContext(options))
            {
                context.Keyword.Add(k2);
                context.SaveChanges();
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                try
                {
                    kRepo.AddKeywordAsync(k).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("breakfast")]
        [InlineData("fast")]
        [InlineData("food")]
        public void AddKeywordShouldAddCorrectKeywordToDB(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordAddTestingAsync2DB")
                .Options;

            Keyword k = new Keyword { Word = kw };
            KeywordRepo kRepo;
            Keyword result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                kRepo.AddKeywordAsync(k).Wait();
                result = context.Keyword.Find(k.Word);
            }

            //Assert
            Assert.Equal(k, result);
        }

    }
}
