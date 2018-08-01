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
            Assert.Equal(8, qList.Count);
        }

        //Testing of DBContainsQuery
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(67)]
        [InlineData(324)]
        public void DBContainsQueryShouldNotThrowExceptionIfDBIsEmpty(int Id)
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
                qRepo.DBContainsQuery(Id);
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
        public void DBContainsQueryShouldReturnFalseIfIfDBIsEmpty(int Id)
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
                result = qRepo.DBContainsQuery(Id);
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
        public void DBContainsQueryShouldReturnTrueIfRestaurantIdInDB(int Id)
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
                result = qRepo.DBContainsQuery(Id);
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(12)]
        [InlineData(4232)]
        [InlineData(67)]
        [InlineData(324)]
        public void DBContainsQueryShouldReturnFalseIfQueryIdNotInDB(int Id)
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
                result = qRepo.DBContainsQuery(Id);
            }
            //Assert
            Assert.False(result);
        }

        //Testing of GetQueryByID
        [Theory]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void GetQueryByIDShouldThrowExceptionIfIdNotFound(int Id)
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
                    qRepo.GetQueryByID(Id);
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
        public void GetQueryByIDShouldNotThrowExceptionIfIdIsInDB(int Id)
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
                qRepo.GetQueryByID(Id);
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
        public void GetQueryByIDShouldReturnQueryWithMatchingId(int Id)
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
                q = qRepo.GetQueryByID(Id);
            }

            //Assert
            Assert.Equal(Id, q.Id);
        }

        //Testing of GetRestaurantsInQuery
        [Theory]
        [InlineData(11)]
        [InlineData(4232)]
        [InlineData(66)]
        [InlineData(324)]
        public void GetRestaurantsInQueryShouldThrowExceptionIfIdNotFound(int Id)
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
                    qRepo.GetRestaurantsInQuery(Id);
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
        public void GetRestaurantsInQueryShouldNotThrowExceptionIfIdIsInDB(int Id)
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
                qRepo.GetRestaurantsInQuery(Id);
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
        public void GetRestaurantsInQueryShouldReturnAllRestaurantsFromJunctionTable(int Id)
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
                rl = qRepo.GetRestaurantsInQuery(Id);
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
        public void AddQueryShouldThrowExceptionIfIdIsPreset(int Id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyQueryAddTesting1DB")
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
                    qRepo.AddQuery(q);
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

        [Theory]
        [InlineData("food")]
        [InlineData("fast")]
        [InlineData("breakfast")]
        [InlineData("fish")]
        public void AddQueryShouldAddNewKeywordsToDB(string keyword)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyQueryAddTesting3DB")
                .Options;

            Query q = new Query { Username = "realUser", QueryTime = DateTime.Now };
            q.QueryKeywordJunction = new List<QueryKeywordJunction>() {  new QueryKeywordJunction() { QueryId = q.Id, Word = keyword } };
            QueryRepo qRepo;
            KeywordRepo kRepo;
            Query result;
            QueryKeywordJunction result2;

            //Act
            using (var context = new Project2DBContext(options))
            {
                qRepo = new QueryRepo(context);
                kRepo = new KeywordRepo(context);
                qRepo.AddQuery(q);
                result = context.Query.Find(q.Id);
                result2 = context.QueryKeywordJunction.Find( q.Id, keyword);
            }

            //Assert
            Assert.Equal(q, result);
            Assert.Equal(q.Id, result2.QueryId);
            Assert.Equal(keyword, result2.Word);
        }
    }
}
