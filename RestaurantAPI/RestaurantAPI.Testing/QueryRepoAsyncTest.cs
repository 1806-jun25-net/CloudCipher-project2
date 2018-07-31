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
    public class QueryRepoAsyncTest
    {
        public QueryRepoAsyncTest()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryAsyncDB")
                .Options;
            using (var context = new Project2DBContext(options))
            {
                RepoTestInMemoryDBSetup.Setup(context);
            }
        }

        //Testing of DBContainsQueryAsync
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(67)]
        [InlineData(324)]
        public void DBContainsAsyncQueryShouldNotThrowExceptionIfDBIsEmpty(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyQueryAsyncDB2")
                .Options;

            bool result = true;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                qRepo.DBContainsQueryAsync(Id).Wait();
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
        [InlineData(67)]
        [InlineData(324)]
        public void DBContainsQueryAsyncShouldReturnFalseIfIfDBIsEmpty(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyQueryAsyncDB3")
                .Options;
            bool result;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                result = qRepo.DBContainsQueryAsync(Id).Result;
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
        public void DBContainsQueryAsyncShouldReturnTrueIfRestaurantIdInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryAsyncDB")
                .Options;
            bool result;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                result = qRepo.DBContainsQueryAsync(Id).Result;
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void DBContainsQueryAsyncShouldReturnFalseIfRestaurantIdNotInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryAsyncDB")
                .Options;
            bool result;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                result = qRepo.DBContainsQueryAsync(Id).Result;
            }
            //Assert
            Assert.False(result);
        }

        //Testing of GetQueryByIDAsync
        [Theory]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void GetQueryByIDAsyncShouldThrowExceptionIfIdNotFound(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryAsyncDB")
                .Options;

            bool result = false;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                try
                {
                    qRepo.GetQueryByIDAsync(Id).Wait();
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
        public void GetQueryByIDAsyncShouldNotThrowExceptionIfIdIsInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryAsyncDB")
                .Options;
            bool result = true;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                qRepo.GetQueryByIDAsync(Id).Wait();
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
        public void GetQueryByIDAsyncShouldReturnRestaurantWithMatchingId(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryAsyncDB")
                .Options;

            Query q;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                q = qRepo.GetQueryByIDAsync(Id).Result;
            }

            //Assert
            Assert.Equal(Id, q.Id);
        }

        //Testing of GetRestaurantsInQueryAsync
        [Theory]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void GetRestaurantsInQueryAsyncShouldThrowExceptionIfIdNotFound(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryAsyncDB")
                .Options;

            bool result = false;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                try
                {
                    qRepo.GetRestaurantsInQueryAsync(Id).Wait();
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
        public void GetRestaurantsInQueryAsyncShouldNotThrowExceptionIfIdIsInDB(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryAsyncDB")
                .Options;
            bool result = true;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                qRepo.GetRestaurantsInQueryAsync(Id).Wait();
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
        public void GetRestaurantsInQueryAsyncShouldReturnAllRestaurantsFromJunctionTable(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryAsyncDB")
                .Options;

            List<Restaurant> rl;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                rl = qRepo.GetRestaurantsInQueryAsync(Id).Result;
            }

            //Assert
            Assert.Equal(Id, rl.Count);
        }

        //Testing of AddQuery
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AddQueryAsyncShouldThrowExceptionIfIdIsPreset(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTesting3DB")
                .Options;

            Query q = new Query { Id = Id, Username = "tester", QueryTime = DateTime.Now };
            Query q2 = new Query { Id = Id, Username = "tester", QueryTime = DateTime.Now };
            QueryRepo qRepo;
            KeywordRepo kRepo;
            bool result = false;
            using (var context = new Project2DBContext(options))
            {
                context.Query.Add(q2);
                context.SaveChanges();
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                kRepo = new KeywordRepo(context);
                try
                {
                    qRepo.AddQueryAsync(q, kRepo).Wait();
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
        public void AddQueryAsyncShouldAddCorrectQueryToDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyAddTesting3DB")
                .Options;

            Query q = new Query { Username = username, QueryTime = DateTime.Now };
            QueryRepo qRepo;
            KeywordRepo kRepo;
            Query result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                kRepo = new KeywordRepo(context);
                qRepo.AddQueryAsync(q, kRepo).Wait();
                result = context.Query.Find(q.Id);
            }

            //Assert
            Assert.Equal(q, result);
        }

        [Theory]
        [InlineData("food")]
        [InlineData("fast")]
        [InlineData("breakfast")]
        [InlineData("fish")]
        public void AddQueryAsyncShouldAddNewKeywordsToDB(string keyword)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyQueryAddTesting3DB")
                .Options;

            Query q = new Query { Username = "realUser", QueryTime = DateTime.Now };
            q.QueryKeywordJunction = new List<QueryKeywordJunction>() { new QueryKeywordJunction() { QueryId = q.Id, Word = keyword } };
            QueryRepo qRepo;
            KeywordRepo kRepo;
            Query result;
            QueryKeywordJunction result2;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                kRepo = new KeywordRepo(context);
                qRepo.AddQueryAsync(q, kRepo).Wait();
                result = context.Query.Find(q.Id);
                result2 = context.QueryKeywordJunction.Find(q.Id, keyword);
            }

            //Assert
            Assert.Equal(q, result);
            Assert.Equal(q.Id, result2.QueryId);
            Assert.Equal(keyword, result2.Word);
        }

    }
}
