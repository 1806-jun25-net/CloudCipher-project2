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
    public class RestaurantRepoTest
    {
        public RestaurantRepoTest()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
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


        //Testing of GetRestaurants()
        [Fact]
        public void GetRestaurantsShouldNotThrowExceptionIfDBIsEmpty()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB1")
                .Options;

            bool result = true;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.GetRestaurants();
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
        public void GetRestaurantsShouldReturnAListWithProperNumberOfRestaurants()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;
            RestaurantRepo rRepo;
            List<Restaurant> rList;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rList = rRepo.GetRestaurants().ToList();
            }
            //Assert
            Assert.Equal(9, rList.Count);
        }

        //Testing of DBContainsRestaurant
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(67)]
        [InlineData(324)]
        public void DBContainsRestaurantShouldNotThrowExceptionIfDBIsEmpty(int Id)
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
                rRepo.DBContainsRestaurant(Id);
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
        public void DBContainsRestaurantShouldReturnFalseIfIfDBIsEmpty(int Id)
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
                result = rRepo.DBContainsRestaurant(Id);
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
        public void DBContainsRestaurantShouldReturnTrueIfRestaurantIdInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurant(Id);
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void DBContainsRestaurantShouldReturnFalseIfRestaurantIdNotInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurant(Id);
            }
            //Assert
            Assert.False(result);
        }


        //Testing of DBContainsRestaurantOverload (the one that takes in name and location rather than Id)
        [Theory]
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        [InlineData("7", "lol")]
        [InlineData("88", "loc")]
        [InlineData("790", "loc")]
        [InlineData("103", "loc")]
        public void DBContainsRestaurantOverloadShouldNotThrowExceptionIfDBIsEmpty(string name, string loc)
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
                rRepo.DBContainsRestaurant(name, loc);
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
        public void DBContainsRestaurantOverloadShouldReturnFalseIfIfDBIsEmpty(string name, string loc)
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
                result = rRepo.DBContainsRestaurant(name, loc);
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
        public void DBContainsRestaurantOverloadShouldReturnTrueIfRestaurantNameAndLocInDB(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurant(name, loc);
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
        public void DBContainsRestaurantOverloadShouldReturnFalseIfRestaurantAndLocNotInDB(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;
            bool result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.DBContainsRestaurant(name, loc);
            }
            //Assert
            Assert.False(result);
        }

        //Testing of GetRestaurantByID
        [Theory]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void GetRestaurantByIDShouldThrowExceptionIfIdNotFound(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;

            bool result = false;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.GetRestaurantByID(Id);
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetRestaurantByIDShouldNotThrowExceptionIfIdIsInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;
            bool result = true;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.GetRestaurantByID(Id);
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
        public void GetRestaurantByIDShouldReturnRestaurantWithMatchingId(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;

            Restaurant r;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                r = rRepo.GetRestaurantByID(Id);
            }

            //Assert
            Assert.Equal(Id, r.Id);
        }

        //Testing of GetRestaurantByNameAndLocation
        [Theory]
        [InlineData("7", "lol")]
        [InlineData("8", "not a real location")]
        [InlineData("88", "loc")]
        [InlineData("790", "loc")]
        [InlineData("103", "loc")]
        public void GetRestaurantByNameAndLocationShouldThrowExceptionIfNameAndLocationNotFound(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;

            bool result = false;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.GetRestaurantByNameAndLocation(name, loc);
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
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        public void GetRestaurantByNameAndLocationShouldNotThrowExceptionIfNameAndLocationIsInDB(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;
            bool result = true;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.GetRestaurantByNameAndLocation(name, loc);
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
        public void GetRestaurantByNameAndLocationShouldReturnRestaurantWithMatchingNameAndLocation(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;

            Restaurant r;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                r = rRepo.GetRestaurantByNameAndLocation(name, loc);
            }

            //Assert
            Assert.Equal(name, r.Name);
            Assert.Equal(loc, r.Location);
        }

        //Testing of GetRestaurantIDByNameAndLocation
        [Theory]
        [InlineData("7", "lol")]
        [InlineData("8", "not a real location")]
        [InlineData("88", "loc")]
        [InlineData("790", "loc")]
        [InlineData("103", "loc")]
        public void GetRestaurantIDByNameAndLocationShouldThrowExceptionIfNameAndLocationNotFound(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;

            bool result = false;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.GetRestaurantIDByNameAndLocation(name, loc);
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
        [InlineData("1", "loc")]
        [InlineData("2", "loc")]
        [InlineData("3", "loc")]
        [InlineData("4", "loc")]
        public void GetRestaurantIDByNameAndLocationShouldNotThrowExceptionIfNameAndLocationIsInDB(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;
            bool result = true;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.GetRestaurantIDByNameAndLocation(name, loc);
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
        public void GetRestaurantIDByNameAndLocationShouldReturnIdMatchingNameAndLocation(string name, string loc)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledRestaurantDB")
                .Options;

            int result;
            RestaurantRepo rRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                result = rRepo.GetRestaurantIDByNameAndLocation(name, loc);
            }

            //Assert
            Assert.Equal(result, Int32.Parse(name));
        }


        //Testing of AddRestaurant
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AddRestaurantShouldThrowExceptionIfIdAlreadyInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting1DB")
                .Options;

            Restaurant r = new Restaurant { Id = Id, Name = Id.ToString(), Location = "loc" };
            Restaurant r2 = new Restaurant { Id = Id, Name = Id.ToString(), Location = "loc" };
            RestaurantRepo rRepo;
            bool result = false;
            using (var context = new Project2DBContext(options))
            {
                context.Restaurant.Add(r2);
                context.SaveChanges();
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.AddRestaurant(r);
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AddRestaurantShouldThrowExceptionIfNameAndLocAlreadyInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting2DB")
                .Options;

            Restaurant r = new Restaurant { Id = 10, Name = Id.ToString(), Location = "loc" };
            Restaurant r2 = new Restaurant { Id = 11, Name = Id.ToString(), Location = "loc" };
            RestaurantRepo rRepo;
            bool result = false;
            using (var context = new Project2DBContext(options))
            {
                context.Restaurant.Add(r2);
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.AddRestaurant(r);
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AddRestrauntShouldAddCorrectRestauranttoDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting3DB")
                .Options;

            Restaurant r = new Restaurant {Name = Id.ToString(), Location = "loc" };
            RestaurantRepo rRepo;
            Restaurant result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.AddRestaurant(r);
                result = context.Restaurant.Find(r.Id);
            }

            //Assert
            Assert.Equal(r, result);
        }

        //Testing of AddNewRestaurants
        [Fact]
        public void AddAllRestaurantsShouldThrowExceptionIfRestaurantListIsNull()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting4DB")
                .Options;
            RestaurantRepo rRepo;
            bool result = false;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    rRepo.AddNewRestaurants(null, new List<string>());
                }
                catch (DbUpdateException)
                {
                    result = true;
                }
            }

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void AddAllRestaurantsShouldNotThrowExceptionIfKeywordListIsNull()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting5DB")
                .Options;
            RestaurantRepo rRepo;
            bool result = true;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.AddNewRestaurants(new List<Restaurant>(), null);
            }
            //Test will fail and not reach this point if Exception is thrown
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void AddAllRestaurantsShouldAddMultipleRestaurantsToDB()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting6DB")
                .Options;
            RestaurantRepo rRepo;
            List<Restaurant> resultList;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                rRepo.AddNewRestaurants(new List<Restaurant>() {
                    new Restaurant { Name = "1", Location = "loc", Owner = "realUser" },
                    new Restaurant { Name = "2", Location = "loc" },
                    new Restaurant { Name = "3", Location = "loc" },
                    new Restaurant { Name = "4", Location = "loc" },
                    new Restaurant { Name = "5", Location = "loc" },
                    new Restaurant { Name = "6", Location = "loc" },
                    new Restaurant { Name = "7", Location = "loc" }
                }, new List<string>());
                context.SaveChanges();
                resultList = context.Restaurant.AsNoTracking().ToList();
            }
            //Test will fail and not reach this point if Exception is thrown
            //Assert
            Assert.Equal(7, resultList.Count);
        }

        [Fact]
        public void AddAllRestaurantsShouldAddOnlyNewRestaurantsToDB()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting7DB")
                .Options;
            RestaurantRepo rRepo;
            List<Restaurant> resultList;
            using (var context = new Project2DBContext(options))
            {
                context.Restaurant.Add(new Restaurant { Name = "1", Location = "loc", Owner = "realUser" });
                context.Restaurant.Add(new Restaurant { Name = "6", Location = "loc" });
                context.Restaurant.Add(new Restaurant { Name = "7", Location = "loc" });
                context.SaveChanges();
            }
            
            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                
                rRepo.AddNewRestaurants( new List<Restaurant>() { 
                    new Restaurant { Name = "1", Location = "loc", Owner = "realUser" },
                    new Restaurant { Name = "2", Location = "loc" },
                    new Restaurant { Name = "3", Location = "loc" },
                    new Restaurant { Name = "4", Location = "loc" },
                    new Restaurant { Name = "5", Location = "loc" },
                    new Restaurant { Name = "6", Location = "loc" },
                    new Restaurant { Name = "7", Location = "loc" }
                }, new List<string>());
                
                context.SaveChanges();
                resultList = context.Restaurant.AsNoTracking().ToList();
            }

            //Assert
            Assert.Equal(7, resultList.Count);
        }
    }
}
