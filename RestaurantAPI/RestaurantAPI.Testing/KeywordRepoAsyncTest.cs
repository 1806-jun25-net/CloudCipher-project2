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
    public class KeywordRepoAsyncTest
    {
        public KeywordRepoAsyncTest()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordAsyncDB")
                .Options;
            using (var context = new Project2DBContext(options))
            {
                RepoTestInMemoryDBSetup.Setup(context);
            }
        }

        //Testing of DBContainsKeywordAsync
        [Theory]
        [InlineData("breakfast")]
        [InlineData("fast")]
        [InlineData("food")]
        [InlineData("trash")]
        [InlineData("brick")]
        [InlineData("hardware")]
        public void DBContainsKeywordAsyncShouldNotThrowExceptionIfDBIsEmpty(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordAsyncDB2")
                .Options;

            bool result = true;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                kRepo.DBContainsKeywordAsync(kw).Wait();
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("breakfast")]
        [InlineData("fast")]
        [InlineData("food")]
        [InlineData("trash")]
        [InlineData("brick")]
        [InlineData("hardware")]
        public void DBContainsKeywordAsyncShouldReturnFalseIfIfDBIsEmpty(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordAsyncDB3")
                .Options;
            bool result;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                result = kRepo.DBContainsKeywordAsync(kw).Result;
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("breakfast")]
        [InlineData("fast")]
        [InlineData("food")]
        public void DBContainsKeywordAsyncShouldReturnTrueIfRestaurantIdInDB(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordAsyncDB")
                .Options;
            bool result;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                result = kRepo.DBContainsKeywordAsync(kw).Result;
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("trash")]
        [InlineData("brick")]
        [InlineData("hardware")]
        public void DBContainsKeywordAsyncShouldReturnFalseIfKeywordNotInDB(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordAsyncDB")
                .Options;
            bool result;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                result = kRepo.DBContainsKeywordAsync(kw).Result;
            }
            //Assert
            Assert.False(result);
        }

        //Testing of AddKeywordAsync
        [Theory]
        [InlineData("food")]
        [InlineData("breakfast")]
        [InlineData("fast")]
        public void AddKeywordAsyncShouldThrowExceptionIfKeywordIsPreset(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordAddTestingAsync1DB")
                .Options;

            Keyword k = new Keyword { Word = kw };
            Keyword k2 = new Keyword { Word = kw };
            KeywordRepo kRepo;
            bool result = false;
            using (var context = new Project2DBContext(options))
            {
                context.Keyword.Add(k2);
                context.SaveChanges();
            }

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                try
                {
                    kRepo.AddKeywordAsync(k).Wait();
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
        [InlineData("breakfast")]
        [InlineData("fast")]
        [InlineData("food")]
        public void AddKeywordShouldAddCorrectKeywordToDB(string kw)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordAddTestingAsync2DB")
                .Options;

            Keyword k = new Keyword { Word = kw };
            KeywordRepo kRepo;
            Keyword result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                kRepo.AddKeywordAsync(k).Wait();
                result = context.Keyword.Find(k.Word);
            }

            //Assert
            Assert.Equal(k, result);
        }

    }
}
