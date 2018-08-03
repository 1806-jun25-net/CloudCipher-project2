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

        public static IEnumerable<object[]> AllQueryData =>
        new List<object[]>
        {
            new object[] { "1", false },
            new object[] { "2", false },
            new object[] { "3", false },
            new object[] { "4", false },
            new object[] { "19", false },
            new object[] { "4232", false },
            new object[] { "67", false },
            new object[] { "324", false },
            new object[] { "1", true },
            new object[] { "2", true },
            new object[] { "3", true },
            new object[] { "4", true },
            new object[] { "19", true },
            new object[] { "4232", true },
            new object[] { "67", true },
            new object[] { "324", true },
        };

        public static IEnumerable<object[]> ValidQueryData =>
        new List<object[]>
        {
            new object[] { "1", false },
            new object[] { "2", false },
            new object[] { "3", false },
            new object[] { "4", false },
            new object[] { "1", true },
            new object[] { "2", true },
            new object[] { "3", true },
            new object[] { "4", true },
        };

        public static IEnumerable<object[]> InvalidQueryData =>
        new List<object[]>
        {
            new object[] { "19", false },
            new object[] { "4232", false },
            new object[] { "67", false },
            new object[] { "324", false },
            new object[] { "19", true },
            new object[] { "4232", true },
            new object[] { "67", true },
            new object[] { "324", true },
        };

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
        [MemberData(nameof(AllQueryData))]
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
        [MemberData(nameof(AllQueryData))]
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
        [MemberData(nameof(ValidQueryData))]
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
        [MemberData(nameof(InvalidQueryData))]
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
        [MemberData(nameof(InvalidQueryData))]
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
        [MemberData(nameof(ValidQueryData))]
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
        [MemberData(nameof(ValidQueryData))]
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
        [MemberData(nameof(InvalidQueryData))]
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
        [MemberData(nameof(ValidQueryData))]
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
        [MemberData(nameof(ValidQueryData))]
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
            Query result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                qRepo.AddQuery(q);
                result = context.Query.Find(q.Id);
            }

            //Assert
            Assert.Equal(q, result);
        }

    }
}