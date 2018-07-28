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
                //Only initialize the DB with data once
                if (context.AppUser.ToList().Count == 0)
                {
                    context.AppUser.Add(new AppUser { Username = "realUser", FirstName = "a", LastName = "b", Email = "e" });
                    context.AppUser.Add(new AppUser { Username = "decoyUser1", FirstName = "a", LastName = "b", Email = "e" });
                    context.AppUser.Add(new AppUser { Username = "decoyUser2", FirstName = "a", LastName = "b", Email = "e" });
                    context.AppUser.Add(new AppUser { Username = "decoyUser3", FirstName = "a", LastName = "b", Email = "e" });

                    context.Restaurant.Add(new Restaurant { Id = 1, Name = "1", Location = "loc", Owner = "realUser"});
                    context.Restaurant.Add(new Restaurant { Id = 2, Name = "2", Location = "loc" });
                    context.Restaurant.Add(new Restaurant { Id = 3, Name = "3", Location = "loc" });
                    context.Restaurant.Add(new Restaurant { Id = 4, Name = "4", Location = "loc" });
                    context.Restaurant.Add(new Restaurant { Id = 5, Name = "5", Location = "loc" , Owner = "realUser"});
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
                context.Dispose();
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
                .UseInMemoryDatabase(databaseName: "EmptyAddTestingDB")
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
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void AddRestaurantToBlacklistShouldThrowExceptionIfUserNotInDB(string username)
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
                    uRepo.AddRestaurantToBlacklist(username, 1, rRepo);
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
        [InlineData("realUser", 1000)]
        [InlineData("decoyUser1", 98)]
        [InlineData("decoyUser2", 52)]
        [InlineData("decoyUser3", 18)]
        public void AddRestaurantToBlacklistShouldThrowExceptionIfRestaurantNotInDB(string username, int rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTestingDB")
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
        [InlineData("realUser", 1)]
        [InlineData("decoyUser1", 2)]
        [InlineData("decoyUser2", 3)]
        [InlineData("decoyUser3", 4)]
        public void AddRestaurantToBlacklistShouldSucceedIfUserAndRestaurantAreValid(string username, int rId)
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
        [InlineData("realUser", 1)]
        [InlineData("decoyUser1", 2)]
        [InlineData("decoyUser2", 3)]
        [InlineData("decoyUser3", 4)]
        public void AddRestaurantToBlacklistShouldThrowExceptionIfRestrauntAlreadyInUsersBlacklist(string username, int rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTestingDB")
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
        [InlineData("fakeUser")]
        [InlineData("totallyNotAUser")]
        [InlineData("zzzzzZZefea")]
        [InlineData("SoooooManyTestsToCome")]
        public void AddRestaurantToFavoritesShouldThrowExceptionIfUserNotInDB(string username)
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
                    uRepo.AddRestaurantToFavorites(username, 1, rRepo);
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
        [InlineData("realUser", 1000)]
        [InlineData("decoyUser1", 98)]
        [InlineData("decoyUser2", 52)]
        [InlineData("decoyUser3", 18)]
        public void AddRestaurantToFavoritesShouldThrowExceptionIfRestaurantNotInDB(string username, int rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTestingDB")
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
        [InlineData("realUser", 1)]
        [InlineData("decoyUser1", 2)]
        [InlineData("decoyUser2", 3)]
        [InlineData("decoyUser3", 4)]
        public void AddRestaurantToFavoritesShouldSucceedIfUserAndRestaurantAreValid(string username, int rId)
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
        [InlineData("realUser", 1)]
        [InlineData("decoyUser1", 2)]
        [InlineData("decoyUser2", 3)]
        [InlineData("decoyUser3", 4)]
        public void AddRestaurantToFavoritesShouldThrowExceptionIfRestrauntAlreadyInUsersBlacklist(string username, int rId)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTestingDB")
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
