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
    public class RestaurantRepoAsyncTest
    {
        public RestaurantRepoAsyncTest()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
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

        //Testing of DBContainsRestaurantAsync
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(67)]
        [InlineData(324)]
        public void DBContainsRestaurantAsyncShouldNotThrowExceptionIfDBIsEmpty(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB2")
                .Options;

            bool result = true;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.DBContainsRestaurantAsync(Id).Wait();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void DBContainsRestaurantAsyncShouldReturnFalseIfIfDBIsEmpty(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB3")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurantAsync(Id).Result;
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void DBContainsRestaurantAsyncShouldReturnTrueIfRestaurantIdInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurantAsync(Id).Result;
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void DBContainsRestaurantAsyncShouldReturnFalseIfRestaurantIdNotInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurantAsync(Id).Result;
            }
            //Assert
            Assert.False(result);
        }


        //Testing of DBContainsRestaurantAsyncOverload (the one that takes in name and location rather than Id)
        [Theory]
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        [InlineData("7", "lol")]
        [InlineData("88", "loc")]
        [InlineData("790", "loc")]
        [InlineData("103", "loc")]
        public void DBContainsRestaurantAsyncOverloadShouldNotThrowExceptionIfDBIsEmpty(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB4")
                .Options;

            bool result = true;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.DBContainsRestaurantAsync(name, loc).Wait();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        [InlineData("7", "lol")]
        [InlineData("88", "loc")]
        [InlineData("790", "loc")]
        [InlineData("103", "loc")]
        public void DBContainsRestaurantAsyncOverloadShouldReturnFalseIfIfDBIsEmpty(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB5")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurantAsync(name, loc).Result;
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        public void DBContainsRestaurantAsyncOverloadShouldReturnTrueIfRestaurantNameAndLocInDB(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurantAsync(name, loc).Result;
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("7", "lol")]
        [InlineData("8", "not a real location")]
        [InlineData("88", "loc")]
        [InlineData("790", "loc")]
        [InlineData("103", "loc")]
        public void DBContainsRestaurantAsyncOverloadShouldReturnFalseIfRestaurantAndLocNotInDB(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurantAsync(name, loc).Result;
            }
            //Assert
            Assert.False(result);
        }

        //Testing of GetRestaurantByIDAsync
        [Theory]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void GetRestaurantByIDAsyncShouldThrowExceptionIfIdNotFound(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;

            bool result = false;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.GetRestaurantByIDAsync(Id).Wait();
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetRestaurantByIDAsyncShouldNotThrowExceptionIfIdIsInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;
            bool result = true;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.GetRestaurantByIDAsync(Id).Wait();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetRestaurantByIDAsyncShouldReturnRestaurantWithMatchingId(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;

            Restaurant r;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                r = rRepo.GetRestaurantByIDAsync(Id).Result;
            }

            //Assert
            Assert.Equal(Id, r.Id);
        }

        //Testing of GetRestaurantByNameAndLocationAsync
        [Theory]
        [InlineData("7", "lol")]
        [InlineData("8", "not a real location")]
        [InlineData("88", "loc")]
        [InlineData("790", "loc")]
        [InlineData("103", "loc")]
        public void GetRestaurantByNameAndLocationAsyncShouldThrowExceptionIfNameAndLocationNotFound(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;

            bool result = false;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.GetRestaurantByNameAndLocationAsync(name, loc).Wait();
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
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        public void GetRestaurantByNameAndLocationAsyncShouldNotThrowExceptionIfNameAndLocationIsInDB(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;
            bool result = true;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.GetRestaurantByNameAndLocationAsync(name, loc).Wait();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        public void GetRestaurantByNameAndLocationAsyncShouldReturnRestaurantWithMatchingNameAndLocation(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;

            Restaurant r;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                r = rRepo.GetRestaurantByNameAndLocationAsync(name, loc).Result;
            }

            //Assert
            Assert.Equal(name, r.Name);
            Assert.Equal(loc, r.Location);
        }

        //Testing of GetRestaurantIDByNameAndLocationAsync
        [Theory]
        [InlineData("7", "lol")]
        [InlineData("8", "not a real location")]
        [InlineData("88", "loc")]
        [InlineData("790", "loc")]
        [InlineData("103", "loc")]
        public void GetRestaurantIDByNameAndLocationAsyncShouldThrowExceptionIfNameAndLocationNotFound(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;

            bool result = false;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.GetRestaurantIDByNameAndLocationAsync(name, loc).Wait();
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
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        public void GetRestaurantIDByNameAndLocationAsyncShouldNotThrowExceptionIfNameAndLocationIsInDB(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;
            bool result = true;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.GetRestaurantIDByNameAndLocationAsync(name, loc).Wait();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        public void GetRestaurantIDByNameAndLocationAsyncShouldReturnIdMatchingNameAndLocation(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurant2DB")
                .Options;

            int result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.GetRestaurantIDByNameAndLocationAsync(name, loc).Result;
            }

            //Assert
            Assert.Equal(result, Int32.Parse(name));
        }
    }
}
