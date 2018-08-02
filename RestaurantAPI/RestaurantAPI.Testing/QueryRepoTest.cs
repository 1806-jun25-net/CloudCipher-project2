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
    public class QueryRepoTest
    {
        public QueryRepoTest()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;
            using (var context = new Project2DBContext(options))
            {
                RepoTestInMemoryDBSetup.Setup(context);
            }
        }

        //Testing of GetQueries()
        [Fact]
        public void GetQueriesShouldNotThrowExceptionIfDBIsEmpty()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB1")
                .Options;

            bool result = true;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                try
                {
                    qRepo.GetQueries();
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
        public void GetQueriesShouldReturnAListWithProperNumberOfQueriess()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;
            QueryRepo qRepo;
            List<Query> qList;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                qList = qRepo.GetQueries().ToList();
            }
            //Assert
            Assert.Equal(15, qList.Count);
        }

        //Testing of DBContainsQuery
        [Theory]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(11, false)]
        [InlineData(4232, false)]
        [InlineData(67, false)]
        [InlineData(324, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(11, true)]
        [InlineData(4232, true)]
        [InlineData(67, true)]
        [InlineData(324, true)]
        public void DBContainsQueryShouldNotThrowExceptionIfDBIsEmpty(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyQueryDB2")
                .Options;

            bool result = true;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                if (useAsync)
                    qRepo.DBContainsQueryAsync(Id).Wait();
                else
                    qRepo.DBContainsQuery(Id);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(11, false)]
        [InlineData(4232, false)]
        [InlineData(67, false)]
        [InlineData(324, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(11, true)]
        [InlineData(4232, true)]
        [InlineData(67, true)]
        [InlineData(324, true)]
        public void DBContainsQueryShouldReturnFalseIfIfDBIsEmpty(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyQueryDB3")
                .Options;
            bool result;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                if (useAsync)
                    result = qRepo.DBContainsQueryAsync(Id).Result;
                else
                    result = qRepo.DBContainsQuery(Id);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        public void DBContainsQueryShouldReturnTrueIfRestaurantIdInDB(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;
            bool result;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                if (useAsync)
                    result = qRepo.DBContainsQueryAsync(Id).Result;
                else
                    result = qRepo.DBContainsQuery(Id);
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(19, false)]
        [InlineData(4232, false)]
        [InlineData(67, false)]
        [InlineData(324, false)]
        [InlineData(19, true)]
        [InlineData(4232, true)]
        [InlineData(67, true)]
        [InlineData(324, true)]
        public void DBContainsQueryShouldReturnFalseIfQueryIdNotInDB(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;
            bool result;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                if (useAsync)
                    result = qRepo.DBContainsQueryAsync(Id).Result;
                else
                    result = qRepo.DBContainsQuery(Id);
            }
            //Assert
            Assert.False(result);
        }

        //Testing of GetQueryByID
        [Theory]
        [InlineData(19, false)]
        [InlineData(4232, false)]
        [InlineData(67, false)]
        [InlineData(324, false)]
        [InlineData(19, true)]
        [InlineData(4232, true)]
        [InlineData(67, true)]
        [InlineData(324, true)]
        public void GetQueryByIDShouldThrowExceptionIfIdNotFound(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;

            bool result = false;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                try
                {
                    if (useAsync)
                        qRepo.GetQueryByIDAsync(Id).Wait();
                    else
                        qRepo.GetQueryByID(Id);
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
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        public void GetQueryByIDShouldNotThrowExceptionIfIdIsInDB(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;
            bool result = true;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                if (useAsync)
                    qRepo.GetQueryByIDAsync(Id).Wait();
                else
                    qRepo.GetQueryByID(Id);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        public void GetQueryByIDShouldReturnQueryWithMatchingId(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;

            Query q;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                if (useAsync)
                    q = qRepo.GetQueryByIDAsync(Id).Result;
                else
                    q = qRepo.GetQueryByID(Id);
            }

            //Assert
            Assert.Equal(Id, q.Id);
        }

        //Testing of GetRestaurantsInQuery
        [Theory]
        [InlineData(19, false)]
        [InlineData(4232, false)]
        [InlineData(67, false)]
        [InlineData(324, false)]
        [InlineData(19, true)]
        [InlineData(4232, true)]
        [InlineData(67, true)]
        [InlineData(324, true)]
        public void GetRestaurantsInQueryShouldThrowExceptionIfIdNotFound(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;

            bool result = false;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                try
                {
                    if (useAsync)
                        qRepo.GetRestaurantsInQueryAsync(Id).Wait();
                    else
                        qRepo.GetRestaurantsInQuery(Id);
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
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        public void GetRestaurantsInQueryShouldNotThrowExceptionIfIdIsInDB(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;
            bool result = true;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                if (useAsync)
                    qRepo.GetRestaurantsInQueryAsync(Id).Wait();
                else
                    qRepo.GetRestaurantsInQuery(Id);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        public void GetRestaurantsInQueryShouldReturnAllRestaurantsFromJunctionTable(int Id, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledQueryDB")
                .Options;

            List<Restaurant> rl;
            QueryRepo qRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                if (useAsync)
                    rl = qRepo.GetRestaurantsInQueryAsync(Id).Result;
                else
                    rl = qRepo.GetRestaurantsInQuery(Id);
            }

            //Assert
            Assert.Equal(Id, rl.Count);
        }

        //AddQuery
        [Theory]
        [InlineData("realUser")]
        [InlineData("decoyUser1")]
        [InlineData("decoyUser2")]
        [InlineData("decoyUser3")]
        public void AddQueryShouldAddCorrectQueryToDB(string username)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyQueryAddTesting2DB")
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
                qRepo.AddQuery(q);
                result = context.Query.Find(q.Id);
            }

            //Assert
            Assert.Equal(q, result);
        }

    }
}
