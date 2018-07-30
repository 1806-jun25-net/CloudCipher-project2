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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        [InlineData("1XxX1LLL")]
        [InlineData("42__3~!2")]
        [InlineData("67")]
        [InlineData("324isANumber")]
        public void DBContainsRestaurantShouldNotThrowExceptionIfDBIsEmpty(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        [InlineData("1XxX1LLL")]
        [InlineData("42__3~!2")]
        [InlineData("67")]
        [InlineData("324isANumber")]
        public void DBContainsRestaurantShouldReturnFalseIfIfDBIsEmpty(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void DBContainsRestaurantShouldReturnTrueIfRestaurantIdInDB(string Id)
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
        [InlineData("1XxX1LLL")]
        [InlineData("42__3~!2")]
        [InlineData("67")]
        [InlineData("324isANumber")]
        public void DBContainsRestaurantShouldReturnFalseIfRestaurantIdNotInDB(string Id)
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

        //Testing of GetRestaurantByID
        [Theory]
        [InlineData("1XxX1LLL")]
        [InlineData("42__3~!2")]
        [InlineData("67")]
        [InlineData("324isANumber")]
        public void GetRestaurantByIDShouldThrowExceptionIfIdNotFound(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void GetRestaurantByIDShouldNotThrowExceptionIfIdIsInDB(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void GetRestaurantByIDShouldReturnRestaurantWithMatchingId(string Id)
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
        
        //Testing of AddRestaurant
        [Theory]
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void AddRestaurantShouldThrowExceptionIfIdAlreadyInDB(string Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddRestaurantTesting1DB")
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void AddRestaurantShouldThrowExceptionIfNameIsNull(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void AddRestaurantShouldThrowExceptionIfLocationIsNull(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void AddRestrauntShouldAddCorrectRestauranttoDB(string Id)
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
                context.Restaurant.Add(new Restaurant { Id = "1a", Name = "1", Lat = "loc", Lon = "loc", Owner = "realUser" });
                context.Restaurant.Add(new Restaurant { Id = "6f", Name = "6", Lat = "loc", Lon = "loc" });
                context.Restaurant.Add(new Restaurant { Id = "7g", Name = "7", Lat = "loc", Lon = "loc" });
                context.SaveChanges();
            }
            
            //Act
            using (var context = new Project2DBContext(options))
            {
                rRepo = new RestaurantRepo(context);
                
                rRepo.AddNewRestaurants( new List<Restaurant>() {
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
