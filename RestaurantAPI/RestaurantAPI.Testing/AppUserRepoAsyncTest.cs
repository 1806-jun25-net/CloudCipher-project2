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
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            using (var context = new Project2DBContext(options))
            {
                RepoTestInMemoryDBSetup.Setup(context);
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
                uRepo.DBContainsUsernameAsync("LiterallyAnything").Wait();
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
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
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
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
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


        //Testing of GetUserByUsernameAsync
        [Theory]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetUserByUsernameAsyncShouldThrowExceptionIfUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;

            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetUserByUsernameAsync(username).Wait();
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
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        public void GetUserByUsernameAsyncShouldNotThrowExceptionIfUsernameIsInDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            bool result = true;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                uRepo.GetUserByUsernameAsync(username).Wait();
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
        public void GetUserByUsernameAsyncShouldReturnUserWithMatchingUsername(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;

            AppUser u;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                u = uRepo.GetUserByUsernameAsync(username).Result;
            }

            //Assert
            Assert.Equal(username, u.Username);
        }
        
        //Testing of GetBlacklistForUserAsync
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetBlacklistForUserAsyncShouldThrowExceptionIfIsDBEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetBlacklistForUserAsync(username).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetBlacklistForUserAsyncShouldThrowExceptionIfIsUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetBlacklistForUserAsync(username).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetBlacklistForUserAsyncShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            AppUserRepo uRepo;
            List<Restaurant> results;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                results = uRepo.GetBlacklistForUserAsync("realUser").Result.ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(3, results.Count);
        }

        //Testing of GetFavoritesForUserAsync
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetFavoritesForUserAsyncShouldThrowExceptionIfIsDBEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetFavoritesForUserAsync(username).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetFavoritesForUserAsyncShouldThrowExceptionIfIsUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetFavoritesForUserAsync(username).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetFavoritesForUserAsyncShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            AppUserRepo uRepo;
            List<Restaurant> results;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                results = uRepo.GetFavoritesForUserAsync("realUser").Result.ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(4, results.Count);
        }


        //Testing of GetQueriesForUserAsync
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetQueriesForUserAsyncShouldThrowExceptionIfIsDBEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetQueriesForUserAsync(username).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetQueriesForUserAsyncShouldThrowExceptionIfIsUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetQueriesForUserAsync(username).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetQueriesForUserAsyncShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            AppUserRepo uRepo;
            List<Query> results;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                results = uRepo.GetQueriesForUserAsync("realUser").Result.ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(8, results.Count);
        }

        //Testing for GetOwnedRestaurantsForUserAsync
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetOwnedRestaurantsForUserAsyncShouldThrowExceptionIfIsDBEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetOwnedRestaurantsForUserAsync(username).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetOwnedRestaurantsForUserAsyncShouldThrowExceptionIfIsUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetOwnedRestaurantsForUserAsync(username).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetOwnedRestaurantsForUserAsyncShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledDB2")
                .Options;
            AppUserRepo uRepo;
            List<Restaurant> results;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                results = uRepo.GetOwnedRestaurantsForUserAsync("realUser").Result.ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(2, results.Count);
        }

        //Testing of AddRestaurantToBlacklistAsync
        [Theory]
        [InlineData("fakeUser", "1a")]
        [InlineData("totallyNotAUser", "2b")]
        [InlineData("zzzzzZZefea", "3c")]
        [InlineData("SoooooManyTestsToCome", "4d")]
        public void AddRestaurantToBlacklistAsyncShouldThrowExceptionIfUserNotInDB(string username, string rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB8")
                .Options;
            AppUserRepo uRepo;
            RestaurantRepo rRepo;
            bool result = false;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                rRepo = new RestaurantRepo(context);
                try
                {
                    uRepo.AddRestaurantToBlacklistAsync(username, rId, rRepo).Wait();
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
        [InlineData("realUser", "nope")]
        [InlineData("decoyUser1", "2fake4u")]
        [InlineData("decoyUser2", "garbage inputs")]
        [InlineData("decoyUser3", "4444_1_#52_")]
        public void AddRestaurantToBlacklistAsyncShouldThrowExceptionIfRestaurantNotInDB(string username, string rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTesting3DB")
                .Options;
            AppUserRepo uRepo;
            RestaurantRepo rRepo;
            bool result = false;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                rRepo = new RestaurantRepo(context);
                try
                {
                    uRepo.AddRestaurantToBlacklistAsync(username, rId, rRepo).Wait();
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
        [InlineData("realUser", "1a")]
        [InlineData("decoyUser1", "2b")]
        [InlineData("decoyUser2", "3c")]
        [InlineData("decoyUser3", "4d")]
        public void AddRestaurantToBlacklistAsyncShouldSucceedIfUserAndRestaurantAreValid(string username, string rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            AppUserRepo uRepo;
            RestaurantRepo rRepo;
            Blacklist result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                rRepo = new RestaurantRepo(context);
                uRepo.AddRestaurantToBlacklistAsync(username, rId, rRepo).Wait();
                result = context.Blacklist.Find(rId, username);
            }

            //Assert
            Assert.Equal(username, result.Username);
            Assert.Equal(rId, result.RestaurantId);
        }

        [Theory]
        [InlineData("realUser", "1a")]
        [InlineData("decoyUser1", "2b")]
        [InlineData("decoyUser2", "3c")]
        [InlineData("decoyUser3", "4d")]
        public void AddRestaurantToBlacklistAsyncShouldThrowExceptionIfRestrauntAlreadyInUsersBlacklist(string username, string rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTesting4DB")
                .Options;
            AppUserRepo uRepo;
            RestaurantRepo rRepo;
            bool result = false;
            using (var context = new Project2DBContext(options))
            {
                context.Blacklist.Add(new Blacklist { Username = username, RestaurantId = rId });
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                rRepo = new RestaurantRepo(context);
                try
                {
                    uRepo.AddRestaurantToBlacklistAsync(username, rId, rRepo).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }

            //Assert
            Assert.True(result);
        }


        //Testing of AddRestaurantToFavorites
        [Theory]
        [InlineData("fakeUser", "1a")]
        [InlineData("totallyNotAUser", "2b")]
        [InlineData("zzzzzZZefea", "3c")]
        [InlineData("SoooooManyTestsToCome", "4d")]
        public void AddRestaurantToFavoritesAsyncShouldThrowExceptionIfUserNotInDB(string username, string rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB9")
                .Options;
            AppUserRepo uRepo;
            RestaurantRepo rRepo;
            bool result = false;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                rRepo = new RestaurantRepo(context);
                try
                {
                    uRepo.AddRestaurantToFavoritesAsync(username, rId, rRepo).Wait();
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
        [InlineData("realUser", "nope")]
        [InlineData("decoyUser1", "2fake4u")]
        [InlineData("decoyUser2", "garbage inputs")]
        [InlineData("decoyUser3", "4444_1_#52_")]
        public void AddRestaurantToFavoritesAsyncShouldThrowExceptionIfRestaurantNotInDB(string username, string rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTesting5DB")
                .Options;
            AppUserRepo uRepo;
            RestaurantRepo rRepo;
            bool result = false;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                rRepo = new RestaurantRepo(context);
                try
                {
                    uRepo.AddRestaurantToFavoritesAsync(username, rId, rRepo).Wait();
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
        [InlineData("realUser", "1a")]
        [InlineData("decoyUser1", "2b")]
        [InlineData("decoyUser2", "3c")]
        [InlineData("decoyUser3", "4d")]
        public void AddRestaurantToFavoritesAsyncShouldSucceedIfUserAndRestaurantAreValid(string username, string rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            AppUserRepo uRepo;
            RestaurantRepo rRepo;
            Favorite result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                rRepo = new RestaurantRepo(context);
                uRepo.AddRestaurantToFavoritesAsync(username, rId, rRepo).Wait();
                result = context.Favorite.Find(rId, username);
            }

            //Assert
            Assert.Equal(username, result.Username);
            Assert.Equal(rId, result.RestaurantId);
        }

        [Theory]
        [InlineData("realUser", "1a")]
        [InlineData("decoyUser1", "2b")]
        [InlineData("decoyUser2", "3c")]
        [InlineData("decoyUser3", "4d")]
        public void AddRestaurantToFavoritesAsyncShouldThrowExceptionIfRestrauntAlreadyInUsersBlacklist(string username, string rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddFavoritesTestingDB")
                .Options;
            AppUserRepo uRepo;
            RestaurantRepo rRepo;
            bool result = false;
            using (var context = new Project2DBContext(options))
            {
                context.Favorite.Add(new Favorite { Username = username, RestaurantId = rId });
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                rRepo = new RestaurantRepo(context);
                try
                {
                    uRepo.AddRestaurantToFavoritesAsync(username, rId, rRepo).Wait();
                }
                catch (AggregateException)
                {
                    result = true;
                }
            }

            //Assert
            Assert.True(result);
        }
    }
}
