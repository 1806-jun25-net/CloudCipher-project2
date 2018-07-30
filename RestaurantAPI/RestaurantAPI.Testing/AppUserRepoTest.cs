using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Library.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantAPI.Testing
{
    public class AppUserRepoTest
    {
        public AppUserRepoTest()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            using (var context = new Project2DBContext(options))
            {
                RepoTestInMemoryDBSetup.Setup(context);
            }
        }

        //Testing of GetUsers()
        [Fact]
        public void GetUsersShouldNotThrowExceptionIfDBIsEmpty()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB1")
                .Options;

            bool result = true;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetUsers();
                }
                catch 
                {
                    result = false;
                }
            }
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetUsersShouldReturnAListWithProperNumberOfUsers()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            AppUserRepo uRepo;
            List<AppUser> uList;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                uList = uRepo.GetUsers().ToList();
            }
            //Assert
            Assert.Equal(4, uList.Count);
        }

        
        //Testing of DBContainsUsername
        [Fact]
        public void DBContainsUsernameShouldNotThrowExceptionIfDBIsEmpty()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB2")
                .Options;

            bool result = true;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                uRepo.DBContainsUsername("LiterallyAnything");
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
        [InlineData("totallyNotAUserr")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void DBContainsUsernameShouldReturnFalseIfIfDBIsEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB3")
                .Options;
            bool result;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                result = uRepo.DBContainsUsername(username);
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
        public void DBContainsUsernameShouldReturnTrueIfUsernameInDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            bool result;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                result = uRepo.DBContainsUsername(username);
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void DBContainsUsernameShouldReturnFalseIfUsernameNotInDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            bool result;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                result = uRepo.DBContainsUsername(username);
            }
            //Assert
            Assert.False(result);
        }



        //Testing of GetUserByUsername
        [Theory]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetUserByUsernameShouldThrowExceptionIfUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetUserByUsername(username);
                }
                catch (NotSupportedException)
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
        public void GetUserByUsernameShouldNotThrowExceptionIfUsernameIsInDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            bool result = true;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                uRepo.GetUserByUsername(username);
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
        public void GetUserByUsernameShouldReturnUserWithMatchingUsername(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;

            AppUser u;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                u = uRepo.GetUserByUsername(username);
            }

            //Assert
            Assert.Equal(username, u.Username);
        }


        //Testing of GetBlacklistForUser
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetBlacklistForUserShouldThrowExceptionIfIsDBEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB4")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetBlacklistForUser(username);
                }
                catch (NotSupportedException)
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
        public void GetBlacklistForUserShouldThrowExceptionIfIsUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetBlacklistForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetBlacklistForUserShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            AppUserRepo uRepo;
            List<Restaurant> results;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                results = uRepo.GetBlacklistForUser("realUser").ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(3, results.Count);
        }

        //Testing of GetFavoritesForUser
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetFavoritesForUserShouldThrowExceptionIfIsDBEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB5")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetFavoritesForUser(username);
                }
                catch (NotSupportedException)
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
        public void GetFavoritesForUserShouldThrowExceptionIfIsUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetFavoritesForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetFavoritesForUserShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            AppUserRepo uRepo;
            List<Restaurant> results;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                results = uRepo.GetFavoritesForUser("realUser").ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(4, results.Count);
        }



        //Testing of GetQueriesForUser
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetQueriesForUserShouldThrowExceptionIfIsDBEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB6")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetQueriesForUser(username);
                }
                catch (NotSupportedException)
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
        public void GetQueriesForUserShouldThrowExceptionIfIsUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetQueriesForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetQueriesForUserShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            AppUserRepo uRepo;
            List<Query> results;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                results = uRepo.GetQueriesForUser("realUser").ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(8, results.Count);
        }


        //Testing for GetOwnedRestaurantsForUser
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void GetOwnedRestaurantsForUserShouldThrowExceptionIfIsDBEmpty(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB7")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetOwnedRestaurantsForUser(username);
                }
                catch (NotSupportedException)
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
        public void GetOwnedRestaurantsForUserShouldThrowExceptionIfIsUsernameNotFound(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            bool result = false;
            AppUserRepo uRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.GetOwnedRestaurantsForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
                }
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetOwnedRestaurantsForUserShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledUserDB")
                .Options;
            AppUserRepo uRepo;
            List<Restaurant> results;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                results = uRepo.GetOwnedRestaurantsForUser("realUser").ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(2, results.Count);
        }


        //Testing of AddUser
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        public void AddUserShouldThrowExceptionIfUsernameAlreadyInDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTesting1DB")
                .Options;

            AppUser u = new AppUser { Username = username, FirstName = "a", LastName = "b", Email = "e" };
            AppUser u2 = new AppUser { Username = username, FirstName = "blah", LastName = "bleh", Email = "e" };
            AppUserRepo uRepo;
            bool result = false;
            using (var context = new Project2DBContext(options))
            {
                context.AppUser.Add(u2);
                context.SaveChanges();
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                try
                {
                    uRepo.AddUser(u);
                }
                catch (DbUpdateException)
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
        public void AddUserShouldAddCorrectUsertoDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTesting2DB")
                .Options;

            AppUser u = new AppUser { Username = username, FirstName = "a", LastName = "b", Email = "e" };
            AppUserRepo uRepo;
            AppUser result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                uRepo = new AppUserRepo(context);
                uRepo.AddUser(u);
                result = context.AppUser.Find(username);
            }

            //Assert
            Assert.Equal(u, result);
        }

        //Testing of AddRestaurantToBlacklist
        [Theory]
        [InlineData("fakeUser", "1a")]
        [InlineData("totallyNotAUser", "2b")]
        [InlineData("zzzzzZZefea", "3c")]
        [InlineData("SoooooManyTestsToCome", "4d")]
        public void AddRestaurantToBlacklistShouldThrowExceptionIfUserNotInDB(string username, string rId)
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
                    uRepo.AddRestaurantToBlacklist(username, rId, rRepo);
                }
                catch (DbUpdateException)
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
        public void AddRestaurantToBlacklistShouldThrowExceptionIfRestaurantNotInDB(string username, string rId)
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
                    uRepo.AddRestaurantToBlacklist(username, rId, rRepo);
                }
                catch (DbUpdateException)
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
        public void AddRestaurantToBlacklistShouldSucceedIfUserAndRestaurantAreValid(string username, string rId)
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
                uRepo.AddRestaurantToBlacklist(username, rId, rRepo);
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
        public void AddRestaurantToBlacklistShouldThrowExceptionIfRestrauntAlreadyInUsersBlacklist(string username, string rId)
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
                    uRepo.AddRestaurantToBlacklist(username, rId, rRepo);
                }
                catch (DbUpdateException)
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
        public void AddRestaurantToFavoritesShouldThrowExceptionIfUserNotInDB(string username, string rId)
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
                    uRepo.AddRestaurantToFavorites(username, rId, rRepo);
                }
                catch (DbUpdateException)
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
        public void AddRestaurantToFavoritesShouldThrowExceptionIfRestaurantNotInDB(string username, string rId)
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
                    uRepo.AddRestaurantToFavorites(username, rId, rRepo);
                }
                catch (DbUpdateException)
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
        public void AddRestaurantToFavoritesShouldSucceedIfUserAndRestaurantAreValid(string username, string rId)
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
                uRepo.AddRestaurantToFavorites(username, rId, rRepo);
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
        public void AddRestaurantToFavoritesShouldThrowExceptionIfRestrauntAlreadyInUsersBlacklist(string username, string rId)
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
                    uRepo.AddRestaurantToFavorites(username, rId, rRepo);
                }
                catch (DbUpdateException)
                {
                    result = true;
                }
            }

            //Assert
            Assert.True(result);
        }

    }
}
