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
    public class KeywordRepoTest
    {
        public KeywordRepoTest()
        {
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordDB")
                .Options;
            using (var context = new Project2DBContext(options))
            {
                RepoTestInMemoryDBSetup.Setup(context);
            }
        }

        public static IEnumerable<object[]> AllKeywordData =>
        new List<object[]>
        {
            new object[] { "breakfast", false },
            new object[] { "fast", false },
            new object[] { "food", false },
            new object[] { "trash", false },
            new object[] { "brick", false },
            new object[] { "hardware", false },
            new object[] { "breakfast", true },
            new object[] { "fast", true },
            new object[] { "food", true },
            new object[] { "trash", true },
            new object[] { "brick", true },
            new object[] { "hardware", true },
        };

        public static IEnumerable<object[]> ValidKeywordData =>
        new List<object[]>
        {
            new object[] { "breakfast", false },
            new object[] { "fast", false },
            new object[] { "food", false },
            new object[] { "breakfast", true },
            new object[] { "fast", true },
            new object[] { "food", true },
        };

        public static IEnumerable<object[]> InvalidKeywordData =>
        new List<object[]>
        {
            new object[] { "trash", false },
            new object[] { "brick", false },
            new object[] { "hardware", false },
            new object[] { "trash", true },
            new object[] { "brick", true },
            new object[] { "hardware", true },
        };

        //Testing of GetKeywords()
        [Fact]
        public void GetKeywordsShouldNotThrowExceptionIfDBIsEmpty()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordDB1")
                .Options;

            bool result = true;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                try
                {
                    kRepo.GetKeywords();
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
        public void GetKeywordsShouldReturnAListWithProperNumberOfKeywords()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordDB")
                .Options;
            KeywordRepo kRepo;
            List<Keyword> kList;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                kList = kRepo.GetKeywords().ToList();
            }
            //Assert
            Assert.Equal(3, kList.Count);
        }

        //Testing of DBContainsKeyword
        [Theory]
        [MemberData(nameof(AllKeywordData))]
        public void DBContainsKeywordShouldNotThrowExceptionIfDBIsEmpty(string kw, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordDB2")
                .Options;

            bool result = true;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                if (useAsync)
                    kRepo.DBContainsKeywordAsync(kw).Wait();
                else
                    kRepo.DBContainsKeyword(kw);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(AllKeywordData))]
        public void DBContainsKeywordShouldReturnFalseIfIfDBIsEmpty(string kw, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordDB3")
                .Options;
            bool result;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                if (useAsync)
                    result = kRepo.DBContainsKeywordAsync(kw).Result;
                else
                    result = kRepo.DBContainsKeyword(kw);
            }
            //If exception is throw, test will exit before reaching Assert
            //Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(ValidKeywordData))]
        public void DBContainsKeywordShouldReturnTrueIfRestaurantIdInDB(string kw, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordDB")
                .Options;
            bool result;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                if (useAsync)
                    result = kRepo.DBContainsKeywordAsync(kw).Result;
                else
                    result = kRepo.DBContainsKeyword(kw);
            }
            //Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(InvalidKeywordData))]
        public void DBContainsKeywordShouldReturnFalseIfKeywordNotInDB(string kw, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "StaticFilledKeywordDB")
                .Options;
            bool result;
            KeywordRepo kRepo;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                if (useAsync)
                    result = kRepo.DBContainsKeywordAsync(kw).Result;
                else
                    result = kRepo.DBContainsKeyword(kw);
            }
            //Assert
            Assert.False(result);
        }

        //Testing of AddKeyword
        [Theory]
        [MemberData(nameof(ValidKeywordData))]
        public void AddKeywordShouldThrowExceptionIfKeywordIsPreset(string kw, bool useAsync)
        {
            //Arrange
            string dBName;
            if (useAsync)
                dBName = "EmptyKeywordAddTestingAsync1DB";
            else
                dBName = "EmptyKeywordAddTesting1DB";
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: dBName)
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
                    if (useAsync)
                        kRepo.AddKeywordAsync(k).Wait();
                    else
                        kRepo.AddKeyword(k);
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
        [MemberData(nameof(ValidKeywordData))]
        public void AddKeywordShouldAddCorrectKeywordToDB(string kw, bool useAsync)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project2DBContext>()
                .UseInMemoryDatabase(databaseName: "EmptyKeywordAddTesting2DB")
                .Options;

            Keyword k = new Keyword { Word = kw };
            KeywordRepo kRepo;
            Keyword result;

            //Act
            using (var context = new Project2DBContext(options))
            {
                kRepo = new KeywordRepo(context);
                if (useAsync)
                    kRepo.AddKeywordAsync(k).Wait();
                else
                    kRepo.AddKeyword(k);
                result = context.Keyword.Find(k.Word);
            }

            //Assert
            Assert.Equal(k, result);
        }
    }
}
