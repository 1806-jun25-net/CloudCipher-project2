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
                RepoTestInMemoryDBSetup.Setup(context);
            }
        }

        public static IEnumerable<object[]> AllRestaurantData =>
        new List<object[]>
        {
            new object[] { "1a", false },
            new object[] { "2b", false },
            new object[] { "3c", false },
            new object[] { "4d", false },
            new object[] { "1XxX1LLL", false },
            new object[] { "42__3~!2", false },
            new object[] { "67", false },
            new object[] { "324isANumber", false },
            new object[] { "1a", true },
            new object[] { "2b", true },
            new object[] { "3c", true },
            new object[] { "4d", true },
            new object[] { "1XxX1LLL", true },
            new object[] { "42__3~!2", true },
            new object[] { "67", true },
            new object[] { "324isANumber", true },
        };

        public static IEnumerable<object[]> ValidRestaurantData =>
        new List<object[]>
        {
            new object[] { "1a", false },
            new object[] { "2b", false },
            new object[] { "3c", false },
            new object[] { "4d", false },
            new object[] { "1a", true },
            new object[] { "2b", true },
            new object[] { "3c", true },
            new object[] { "4d", true },
        };

        public static IEnumerable<object[]> InvalidRestaurantData =>
        new List<object[]>
        {
            new object[] { "1XxX1LLL", false },
            new object[] { "42__3~!2", false },
            new object[] { "67", false },
            new object[] { "324isANumber", false },
            new object[] { "1XxX1LLL", true },
            new object[] { "42__3~!2", true },
            new object[] { "67", true },
            new object[] { "324isANumber", true },
        };

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
        [MemberData(nameof(AllRestaurantData))]
        public void DBContainsRestaurantShouldNotThrowExceptionIfDBIsEmpty(string Id, bool useAsync)
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
                if (useAsync)
                    rRepo.DBContainsRestaurantAsync(Id).Wait();
                else
                    rRepo.DBContainsRestaurant(Id);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(AllRestaurantData))]
        public void DBContainsRestaurantShouldReturnFalseIfIfDBIsEmpty(string Id, bool useAsync)
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
                if (useAsync)
                    result = rRepo.DBContainsRestaurantAsync(Id).Result;
                else
                    result = rRepo.DBContainsRestaurant(Id);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(ValidRestaurantData))]
        public void DBContainsRestaurantShouldReturnTrueIfRestaurantIdInDB(string Id, bool useAsync)
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
                if (useAsync)
                    result = rRepo.DBContainsRestaurantAsync(Id).Result;
                else
                    result = rRepo.DBContainsRestaurant(Id);
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(InvalidRestaurantData))]
        public void DBContainsRestaurantShouldReturnFalseIfRestaurantIdNotInDB(string Id, bool useAsync)
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
                if (useAsync)
                    result = rRepo.DBContainsRestaurantAsync(Id).Result;
                else
                    result = rRepo.DBContainsRestaurant(Id);
            }
            //Assert
            Assert.False(result);
        }

        //Testing of GetRestaurantByID
        [Theory]
        [MemberData(nameof(InvalidRestaurantData))]
        public void GetRestaurantByIDShouldThrowExceptionIfIdNotFound(string Id, bool useAsync)
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
                    if (useAsync)
                        rRepo.GetRestaurantByIDAsync(Id).Wait();
                    else
                        rRepo.GetRestaurantByID(Id);
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
        [MemberData(nameof(ValidRestaurantData))]
        public void GetRestaurantByIDShouldNotThrowExceptionIfIdIsInDB(string Id, bool useAsync)
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
                if (useAsync)
                    rRepo.GetRestaurantByIDAsync(Id).Wait();
                else
                    rRepo.GetRestaurantByID(Id);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(ValidRestaurantData))]
        public void GetRestaurantByIDShouldReturnRestaurantWithMatchingId(string Id, bool useAsync)
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
                if (useAsync)
                    r = rRepo.GetRestaurantByIDAsync(Id).Result;
                else
                    r = rRepo.GetRestaurantByID(Id);
            }

            //Assert
            Assert.Equal(Id, r.Id);
        }
        
        //Testing of AddRestaurant
        [Theory]
        [MemberData(nameof(ValidRestaurantData))]
        public void AddRestaurantShouldThrowExceptionIfIdAlreadyInDB(string Id, bool useAsync)
        {
            //Arrange
            string dbName;
            if (useAsync)
                dbName = "EmptyAddRestaurantAsyncTesting1DB";
            else
                dbName = "EmptyAddRestaurantTesting1DB";
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            Restaurant r = new Restaurant { Id = Id, Name = Id + " Diner", Lat = "loc", Lon = "loc" };
            Restaurant r2 = new Restaurant { Id = Id, Name = Id + " Diner", Lat = "loc", Lon = "loc" };
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
                    if (useAsync)
                        rRepo.AddRestaurantAsync(r).Wait();
                    else
                        rRepo.AddRestaurant(r);
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
        [MemberData(nameof(ValidRestaurantData))]
        public void AddRestaurantShouldThrowExceptionIfNameIsNull(string Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting2DB")
                .Options;

            Restaurant r = new Restaurant { Id = Id, Lat = "loc", Lon = "loc" };
            RestaurantRepo rRepo;
            bool result = false;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    if (useAsync)
                        rRepo.AddRestaurantAsync(r).Wait();
                    else
                        rRepo.AddRestaurant(r);
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
        [MemberData(nameof(ValidRestaurantData))]
        public void AddRestaurantShouldThrowExceptionIfLocationIsNull(string Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting2DB")
                .Options;

            Restaurant r = new Restaurant { Id = Id, Name = Id+" Diner" };
            RestaurantRepo rRepo;
            bool result = false;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                try
                {
                    if (useAsync)
                        rRepo.AddRestaurantAsync(r).Wait();
                    else
                        rRepo.AddRestaurant(r);
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
        [MemberData(nameof(ValidRestaurantData))]
        public void AddRestrauntShouldAddCorrectRestauranttoDB(string Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting3DB")
                .Options;

            Restaurant r = new Restaurant { Id = Id, Name = Id + " Diner", Lat = "loc", Lon = "loc" };
            RestaurantRepo rRepo;
            Restaurant result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                if (useAsync)
                    rRepo.AddRestaurantAsync(r).Wait();
                else
                    rRepo.AddRestaurant(r);
                result = context.Restaurant.Find(r.Id);
            }

            //Assert
            Assert.Equal(r, result);
        }

        //Testing of AddNewRestaurants
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AddAllRestaurantsShouldThrowExceptionIfRestaurantListIsNull(bool useAsync)
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
                    if (useAsync)
                        rRepo.AddNewRestaurantsAsync(null, new List<string>()).Wait();
                    else
                        rRepo.AddNewRestaurants(null, new List<string>());
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
        [InlineData(false)]
        [InlineData(true)]
        public void AddAllRestaurantsShouldNotThrowExceptionIfKeywordListIsNull(bool useAsync)
        {
            //Arrange
            string dbName;
            if (useAsync)
                dbName = "EmptyAddRestaurantAsyncTesting5DB";
            else
                dbName = "EmptyAddRestaurantTesting5DB";
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            RestaurantRepo rRepo;
            bool result = true;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                if (useAsync)
                    rRepo.AddNewRestaurantsAsync(new List<Restaurant>(), null).Wait();
                else
                    rRepo.AddNewRestaurants(new List<Restaurant>(), null);
            }
            //Test will fail and not reach this point if Exception is thrown
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AddAllRestaurantsShouldAddMultipleRestaurantsToDB(bool useAsync)
        {
            //Arrange
            string dbName;
            if (useAsync)
                dbName = "EmptyAddRestaurantAsyncTesting6DB";
            else
                dbName = "EmptyAddRestaurantTesting6DB";
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            RestaurantRepo rRepo;
            List<Restaurant> resultList;

            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                if (useAsync)
                    rRepo.AddNewRestaurantsAsync(new List<Restaurant>() {
                    new Restaurant { Id = "1a", Name = "1", Lat = "loc", Lon = "loc", Owner = "realUser" },
                    new Restaurant { Id = "2b", Name = "2", Lat = "loc", Lon = "loc" },
                    new Restaurant { Id = "3c", Name = "3", Lat = "loc", Lon = "loc" },
                    new Restaurant { Id = "4d", Name = "4", Lat = "loc", Lon = "loc" },
                    new Restaurant { Id = "5e", Name = "5", Lat = "loc", Lon = "loc", Owner = "realUser" },
                    new Restaurant { Id = "6f", Name = "6", Lat = "loc", Lon = "loc" },
                    new Restaurant { Id = "7g", Name = "7", Lat = "loc", Lon = "loc" }
                    }, new List<string>()).Wait();
                else
                    rRepo.AddNewRestaurants(new List<Restaurant>() {
                        new Restaurant { Id = "1a", Name = "1", Lat = "loc", Lon = "loc", Owner = "realUser" },
                        new Restaurant { Id = "2b", Name = "2", Lat = "loc", Lon = "loc" },
                        new Restaurant { Id = "3c", Name = "3", Lat = "loc", Lon = "loc" },
                        new Restaurant { Id = "4d", Name = "4", Lat = "loc", Lon = "loc" },
                        new Restaurant { Id = "5e", Name = "5", Lat = "loc", Lon = "loc", Owner = "realUser" },
                        new Restaurant { Id = "6f", Name = "6", Lat = "loc", Lon = "loc" },
                        new Restaurant { Id = "7g", Name = "7", Lat = "loc", Lon = "loc" }
                    }, new List<string>());
                context.SaveChanges();
                resultList = context.Restaurant.AsNoTracking().ToList();
            }
            //Test will fail and not reach this point if Exception is thrown
            //Assert
            Assert.Equal(7, resultList.Count);
        }
        
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AddAllRestaurantsShouldAddOnlyNewRestaurantsToDB(bool useAsync)
        {
            //Arrange
            string dbName;
            if (useAsync)
                dbName = "EmptyAddRestaurantAsyncTesting7DB";
            else
                dbName = "EmptyAddRestaurantTesting7DB";
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            RestaurantRepo rRepo;
            List<Restaurant> resultList;
            using (var context = new Project2DBContext(options))
            {
                context.Restaurant.Add(new Restaurant { Id = "1a", Name = "1", Lat = "loc", Lon = "loc", Owner = "realUser" });
                context.Restaurant.Add(new Restaurant { Id = "6f", Name = "6", Lat = "loc", Lon = "loc" });
                context.Restaurant.Add(new Restaurant { Id = "7g", Name = "7", Lat = "loc", Lon = "loc" });
                context.SaveChanges();
            }
            
            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);

                if (useAsync)
                    rRepo.AddNewRestaurantsAsync(new List<Restaurant>() {
                    new Restaurant { Id = "1a", Name = "1", Lat = "loc", Lon = "loc", Owner = "realUser" },
                    new Restaurant { Id = "2b", Name = "2", Lat = "loc", Lon = "loc" },
                    new Restaurant { Id = "3c", Name = "3", Lat = "loc", Lon = "loc" },
                    new Restaurant { Id = "4d", Name = "4", Lat = "loc", Lon = "loc" },
                    new Restaurant { Id = "5e", Name = "5", Lat = "loc", Lon = "loc", Owner = "realUser" },
                    new Restaurant { Id = "6f", Name = "6", Lat = "loc", Lon = "loc" },
                    new Restaurant { Id = "7g", Name = "7", Lat = "loc", Lon = "loc" }
                    }, new List<string>()).Wait();
                else
                    rRepo.AddNewRestaurants(new List<Restaurant>() {
                        new Restaurant { Id = "1a", Name = "1", Lat = "loc", Lon = "loc", Owner = "realUser" },
                        new Restaurant { Id = "2b", Name = "2", Lat = "loc", Lon = "loc" },
                        new Restaurant { Id = "3c", Name = "3", Lat = "loc", Lon = "loc" },
                        new Restaurant { Id = "4d", Name = "4", Lat = "loc", Lon = "loc" },
                        new Restaurant { Id = "5e", Name = "5", Lat = "loc", Lon = "loc", Owner = "realUser" },
                        new Restaurant { Id = "6f", Name = "6", Lat = "loc", Lon = "loc" },
                        new Restaurant { Id = "7g", Name = "7", Lat = "loc", Lon = "loc" }
                    }, new List<string>());

                context.SaveChanges();
                resultList = context.Restaurant.AsNoTracking().ToList();
            }

            //Assert
            Assert.Equal(7, resultList.Count);
        }
    }
}
