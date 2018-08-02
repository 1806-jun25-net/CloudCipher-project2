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

        public static IEnumerable<object[]> AllUserData =>
        new List<object[]>
        {
            new object[] { "realUser", false },
            new object[] { "decoyUser1", false },
            new object[] { "decoyUser2", false },
            new object[] { "decoyUser3", false },
            new object[] { "fakeUser", false },
            new object[] { "totallyNotAUserr", false },
            new object[] { "zzzzzZZefea", false },
            new object[] { "SoooooManyTestsToCome", false },
            new object[] { "realUser", true },
            new object[] { "decoyUser1", true },
            new object[] { "decoyUser2", true },
            new object[] { "decoyUser3", true },
            new object[] { "fakeUser", true },
            new object[] { "totallyNotAUserr", true },
            new object[] { "zzzzzZZefea", true },
            new object[] { "SoooooManyTestsToCome", true },
        };

        public static IEnumerable<object[]> ValidUserData =>
        new List<object[]>
        {
            new object[] { "realUser", false },
            new object[] { "decoyUser1", false },
            new object[] { "decoyUser2", false },
            new object[] { "decoyUser3", false },
            new object[] { "realUser", true },
            new object[] { "decoyUser1", true },
            new object[] { "decoyUser2", true },
            new object[] { "decoyUser3", true },
        };

        public static IEnumerable<object[]> InvalidUserData =>
        new List<object[]>
        {
            new object[] { "fakeUser", false },
            new object[] { "totallyNotAUserr", false },
            new object[] { "zzzzzZZefea", false },
            new object[] { "SoooooManyTestsToCome", false },
            new object[] { "fakeUser", true },
            new object[] { "totallyNotAUserr", true },
            new object[] { "zzzzzZZefea", true },
            new object[] { "SoooooManyTestsToCome", true },
        };

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
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DBContainsUsernameShouldNotThrowExceptionIfDBIsEmpty(bool useAsync)
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
                if (useAsync)
                    uRepo.DBContainsUsernameAsync("LiterallyAnything").Wait();
                else
                    uRepo.DBContainsUsername("LiterallyAnything");
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(AllUserData))]
        public void DBContainsUsernameShouldReturnFalseIfIfDBIsEmpty(string username, bool useAsync)
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
                if (useAsync)
                    result = uRepo.DBContainsUsernameAsync(username).Result;
                else
                    result = uRepo.DBContainsUsername(username);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(ValidUserData))]
        public void DBContainsUsernameShouldReturnTrueIfUsernameInDB(string username, bool useAsync)
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
                if (useAsync)
                    result = uRepo.DBContainsUsernameAsync(username).Result;
                else
                    result = uRepo.DBContainsUsername(username);
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(InvalidUserData))]
        public void DBContainsUsernameShouldReturnFalseIfUsernameNotInDB(string username, bool useAsync)
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
                if (useAsync)
                    result = uRepo.DBContainsUsernameAsync(username).Result;
                else
                    result = uRepo.DBContainsUsername(username);
            }
            //Assert
            Assert.False(result);
        }



        //Testing of GetUserByUsername
        [Theory]
        [MemberData(nameof(InvalidUserData))]
        public void GetUserByUsernameShouldThrowExceptionIfUsernameNotFound(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.GetUserByUsernameAsync(username).Wait();
                    else
                        uRepo.GetUserByUsername(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
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
        [MemberData(nameof(ValidUserData))]
        public void GetUserByUsernameShouldNotThrowExceptionIfUsernameIsInDB(string username, bool useAsync)
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
                if (useAsync)
                    uRepo.GetUserByUsernameAsync(username).Wait();
                else
                    uRepo.GetUserByUsername(username);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(ValidUserData))]
        public void GetUserByUsernameShouldReturnUserWithMatchingUsername(string username, bool useAsync)
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
                if (useAsync)
                    u= uRepo.GetUserByUsernameAsync(username).Result;
                else
                    u = uRepo.GetUserByUsername(username);
            }

            //Assert
            Assert.Equal(username, u.Username);
        }


        //Testing of GetBlacklistForUser
        [Theory]
        [MemberData(nameof(AllUserData))]
        public void GetBlacklistForUserShouldThrowExceptionIfIsDBEmpty(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.GetBlacklistForUserAsync(username).Wait();
                    else
                        uRepo.GetBlacklistForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
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
        [MemberData(nameof(InvalidUserData))]
        public void GetBlacklistForUserShouldThrowExceptionIfIsUsernameNotFound(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.GetBlacklistForUserAsync(username).Wait();
                    else
                        uRepo.GetBlacklistForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
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
        [InlineData("realUser", 3, false)]
        [InlineData("decoyUser1", 2, false)]
        [InlineData("decoyUser2", 1, false)]
        [InlineData("decoyUser3", 0, false)]
        [InlineData("realUser", 3, true)]
        [InlineData("decoyUser1", 2, true)]
        [InlineData("decoyUser2", 1, true)]
        [InlineData("decoyUser3", 0, true)]
        public void GetBlacklistForUserShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound(string username, int expected, bool useAsync)
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
                if (useAsync)
                    results = uRepo.GetBlacklistForUserAsync(username).Result.ToList();
                else
                    results = uRepo.GetBlacklistForUser(username).ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(expected, results.Count);
        }

        //Testing of GetFavoritesForUser
        [Theory]
        [MemberData(nameof(AllUserData))]
        public void GetFavoritesForUserShouldThrowExceptionIfIsDBEmpty(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.GetFavoritesForUserAsync(username).Wait();
                    else
                        uRepo.GetFavoritesForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
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
        [MemberData(nameof(InvalidUserData))]
        public void GetFavoritesForUserShouldThrowExceptionIfIsUsernameNotFound(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.GetFavoritesForUserAsync(username).Wait();
                    else
                        uRepo.GetFavoritesForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
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
        [InlineData("realUser", 4, false)]
        [InlineData("decoyUser1", 3, false)]
        [InlineData("decoyUser2", 2, false)]
        [InlineData("decoyUser3", 1, false)]
        [InlineData("realUser", 4, true)]
        [InlineData("decoyUser1", 3, true)]
        [InlineData("decoyUser2", 2, true)]
        [InlineData("decoyUser3", 1, true)]
        public void GetFavoritesForUserShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound(string username, int expected, bool useAsync)
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
                if (useAsync)
                    results = uRepo.GetFavoritesForUserAsync(username).Result.ToList();
                else
                    results = uRepo.GetFavoritesForUser(username).ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(expected, results.Count);
        }



        //Testing of GetQueriesForUser
        [Theory]
        [MemberData(nameof(AllUserData))]
        public void GetQueriesForUserShouldThrowExceptionIfIsDBEmpty(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.GetQueriesForUserAsync(username).Wait();
                    else
                        uRepo.GetQueriesForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
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
        [MemberData(nameof(InvalidUserData))]
        public void GetQueriesForUserShouldThrowExceptionIfIsUsernameNotFound(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.GetQueriesForUserAsync(username).Wait();
                    else
                        uRepo.GetQueriesForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
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
        [InlineData("realUser", 8, false)]
        [InlineData("decoyUser1", 4, false)]
        [InlineData("decoyUser2", 2, false)]
        [InlineData("decoyUser3", 1, false)]
        [InlineData("realUser", 8, true)]
        [InlineData("decoyUser1", 4, true)]
        [InlineData("decoyUser2", 2, true)]
        [InlineData("decoyUser3", 1, true)]
        public void GetQueriesForUserShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound(string username, int expected, bool useAsync)
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
                if (useAsync)
                    results = uRepo.GetQueriesForUserAsync(username).Result.ToList();
                else
                    results = uRepo.GetQueriesForUser(username).ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(expected, results.Count);
        }


        //Testing for GetOwnedRestaurantsForUser
        [Theory]
        [MemberData(nameof(AllUserData))]
        public void GetOwnedRestaurantsForUserShouldThrowExceptionIfIsDBEmpty(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.GetOwnedRestaurantsForUserAsync(username).Wait();
                    else
                        uRepo.GetOwnedRestaurantsForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
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
        [MemberData(nameof(InvalidUserData))]
        public void GetOwnedRestaurantsForUserShouldThrowExceptionIfIsUsernameNotFound(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.GetOwnedRestaurantsForUserAsync(username).Wait();
                    else
                        uRepo.GetOwnedRestaurantsForUser(username);
                }
                catch (NotSupportedException)
                {
                    result = true;
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
        [InlineData("realUser", 2, false)]
        [InlineData("decoyUser1", 1, false)]
        [InlineData("decoyUser2", 1, false)]
        [InlineData("decoyUser3", 0, false)]
        [InlineData("realUser", 2, true)]
        [InlineData("decoyUser1", 1, true)]
        [InlineData("decoyUser2", 1, true)]
        [InlineData("decoyUser3", 0, true)]
        public void GetOwnedRestaurantsForUserShouldReturnCorrectNumberOfRestaurantsWhenUsernameFound(string username, int expected, bool useAsync)
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
                if (useAsync)
                    results = uRepo.GetOwnedRestaurantsForUserAsync(username).Result.ToList();
                else
                    results = uRepo.GetOwnedRestaurantsForUser(username).ToList();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.Equal(expected, results.Count);
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
        [MemberData(nameof(InvalidUserData))]
        public void AddRestaurantToBlacklistShouldThrowExceptionIfUserNotInDB(string username, bool useAsync)
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
                    if (useAsync)
                        uRepo.AddRestaurantToBlacklistAsync(username, "anything", rRepo).Wait();
                    else
                        uRepo.AddRestaurantToBlacklist(username, "anything", rRepo);

                }
                catch (DbUpdateException)
                {
                    result = true;
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
        [InlineData("realUser", "nope", false)]
        [InlineData("decoyUser1", "2fake4u", false)]
        [InlineData("decoyUser2", "garbage inputs", false)]
        [InlineData("decoyUser3", "4444_1_#52_", false)]
        [InlineData("realUser", "nope", true)]
        [InlineData("decoyUser1", "2fake4u", true)]
        [InlineData("decoyUser2", "garbage inputs", true)]
        [InlineData("decoyUser3", "4444_1_#52_", true)]
        public void AddRestaurantToBlacklistShouldThrowExceptionIfRestaurantNotInDB(string username, string rId, bool useAsync)
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
                    if (useAsync)
                        uRepo.AddRestaurantToBlacklistAsync(username, rId, rRepo).Wait();
                    else
                        uRepo.AddRestaurantToBlacklist(username, rId, rRepo);
                }
                catch (DbUpdateException)
                {
                    result = true;
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
        [InlineData("realUser", "1a", false)]
        [InlineData("decoyUser1", "2b", false)]
        [InlineData("decoyUser2", "3c", false)]
        [InlineData("decoyUser3", "4d", false)]
        [InlineData("realUser", "1a", true)]
        [InlineData("decoyUser1", "2b", true)]
        [InlineData("decoyUser2", "3c", true)]
        [InlineData("decoyUser3", "4d", true)]
        public void AddRestaurantToBlacklistShouldSucceedIfUserAndRestaurantAreValid(string username, string rId, bool useAsync)
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
                if (useAsync)
                    uRepo.AddRestaurantToBlacklistAsync(username, rId, rRepo).Wait();
                else
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
