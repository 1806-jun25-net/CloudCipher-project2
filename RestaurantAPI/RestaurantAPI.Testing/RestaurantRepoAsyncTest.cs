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
                RepoTestInMemoryDBSetup.Setup(context);
            }
        }

        //Testing of DBContainsRestaurantAsync
        [Theory]
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        [InlineData("1XxX1LLL")]
        [InlineData("42__3~!2")]
        [InlineData("67")]
        [InlineData("324isANumber")]
        public void DBContainsRestaurantAsyncShouldNotThrowExceptionIfDBIsEmpty(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        [InlineData("1XxX1LLL")]
        [InlineData("42__3~!2")]
        [InlineData("67")]
        [InlineData("324isANumber")]
        public void DBContainsRestaurantAsyncShouldReturnFalseIfIfDBIsEmpty(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void DBContainsRestaurantAsyncShouldReturnTrueIfRestaurantIdInDB(string Id)
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
        [InlineData("1XxX1LLL")]
        [InlineData("42__3~!2")]
        [InlineData("67")]
        [InlineData("324isANumber")]
        public void DBContainsRestaurantAsyncShouldReturnFalseIfRestaurantIdNotInDB(string Id)
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

        //Testing of GetRestaurantByIDAsync
        [Theory]
        [InlineData("1XxX1LLL")]
        [InlineData("42__3~!2")]
        [InlineData("67")]
        [InlineData("324isANumber")]
        public void GetRestaurantByIDAsyncShouldThrowExceptionIfIdNotFound(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void GetRestaurantByIDAsyncShouldNotThrowExceptionIfIdIsInDB(string Id)
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
        [InlineData("1a")]
        [InlineData("2b")]
        [InlineData("3c")]
        [InlineData("4d")]
        public void GetRestaurantByIDAsyncShouldReturnRestaurantWithMatchingId(string Id)
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
    }
}
