using RestaurantAPI.API.Models;
using RestaurantAPI.Data;
using RestaurantAPI.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RestaurantAPI.Testing
{
    public class MapperTest
    {
        [Fact]
        public void MapIntoAppUserShouldMatchValues()
        {
            AppUser result;
            UserModel initial = new UserModel() { Username = "user", FirstName = "first", LastName = "last", Email = "mail@e.com" };

            result = Mapper.Map(initial);

            Assert.Equal(initial.Username, result.Username);
            Assert.Equal(initial.FirstName, result.FirstName);
            Assert.Equal(initial.LastName, result.LastName);
            Assert.Equal(initial.Email, result.Email);
        }

        [Fact]
        public void MapIntoUserModelShouldMatchValues()
        {
            UserModel result;
            AppUser initial = new AppUser() { Username = "user", FirstName = "first", LastName = "last", Email = "mail@e.com" };

            result = Mapper.Map(initial);

            Assert.Equal(initial.Username, result.Username);
            Assert.Equal(initial.FirstName, result.FirstName);
            Assert.Equal(initial.LastName, result.LastName);
            Assert.Equal(initial.Email, result.Email);
        }

        [Fact]
        public void MapIntoRestaurantModelShouldMatchValues()
        {
            RestaurantModel result;
            Restaurant initial = new Restaurant()
            {
                Id = "Id",
                Name = "name",
                Hours = "9-5",
                Lat = "111.0524864",
                Lon = "1aefase64",
                Address = "123 W nope",
                Rating = 1.0m,
                PriceLevel = 1.5873m
            };

            result = Mapper.Map(initial);

            Assert.Equal(initial.Id, result.Id);
            Assert.Equal(initial.Name, result.Name);
            Assert.Equal(initial.Hours, result.Hours);
            Assert.Equal(initial.Lat, result.Lat);
            Assert.Equal(initial.Lon, result.Lon);
            Assert.Equal(initial.Address, result.Address);
            Assert.Equal(initial.Rating, result.Rating);
            Assert.Equal(initial.PriceLevel, result.PriceLevel);
            Assert.Equal(initial.Owner, result.Owner);
        }

        [Fact]
        public void MapIntoRestaurantShouldMatchValues()
        {
            Restaurant result;
            RestaurantModel  initial = new RestaurantModel()
            {
                Id = "Id",
                Name = "name",
                Hours = "9-5",
                Lat = "111.0524864",
                Lon = "1aefase64",
                Address = "123 W nope",
                Rating = 1.0m,
                PriceLevel = 1.5873m
            };

            result = Mapper.Map(initial);

            Assert.Equal(initial.Id, result.Id);
            Assert.Equal(initial.Name, result.Name);
            Assert.Equal(initial.Hours, result.Hours);
            Assert.Equal(initial.Lat, result.Lat);
            Assert.Equal(initial.Lon, result.Lon);
            Assert.Equal(initial.Address, result.Address);
            Assert.Equal(initial.Rating, result.Rating);
            Assert.Equal(initial.PriceLevel, result.PriceLevel);
            Assert.Equal(initial.Owner, result.Owner);
        }

        [Fact]
        public void MapIntoQueryModelShouldMatchValues()
        {
            QueryModel result;
            Query initial = new Query()
            {
                Id = 7,
                Username = "name",
                Lat = "111.0524864",
                Lon = "1aefase64",
                Radius = 1,
                QueryTime = new DateTime()
            };

            result = Mapper.Map(initial);

            Assert.Equal(initial.Id, result.Id);
            Assert.Equal(initial.Username, result.Username);
            Assert.Equal(initial.Lat, result.Lat);
            Assert.Equal(initial.Lon, result.Lon);
            Assert.Equal(initial.Radius, result.Radius);
            Assert.Equal(initial.QueryTime, result.QueryTime);
        }

        [Fact]
        public void MapIntoQueryShouldMatchValues()
        {
            Query result;
            QueryModel initial = new QueryModel()
            {
                Id = 7,
                Username = "name",
                Lat = "111.0524864",
                Lon = "1aefase64",
                Radius = 1,
                QueryTime = new DateTime()
            };

            result = Mapper.Map(initial);

            Assert.Equal(initial.Id, result.Id);
            Assert.Equal(initial.Username, result.Username);
            Assert.Equal(initial.Lat, result.Lat);
            Assert.Equal(initial.Lon, result.Lon);
            Assert.Equal(initial.Radius, result.Radius);
            Assert.Equal(initial.QueryTime, result.QueryTime);
        }

        [Fact]
        public void MapIntoAppUserListShouldMatchValues()
        {
            List <AppUser> result;
            IEnumerable<UserModel> initial = new List<UserModel>() { new UserModel() { Username = "user", FirstName = "first", LastName = "last", Email = "mail@e.com" } };

            result = Mapper.Map(initial).ToList();

            Assert.Equal(((List< UserModel>)initial)[0].Username, result[0].Username);
            Assert.Equal(((List<UserModel>)initial)[0].FirstName, result[0].FirstName);
            Assert.Equal(((List<UserModel>)initial)[0].LastName, result[0].LastName);
            Assert.Equal(((List<UserModel>)initial)[0].Email, result[0].Email);
        }

        [Fact]
        public void MapIntoUserModelListShouldMatchValues()
        {
            List<UserModel> result;
            IEnumerable<AppUser> initial = new List<AppUser>() { new AppUser() { Username = "user", FirstName = "first", LastName = "last", Email = "mail@e.com" } };

            result = Mapper.Map(initial).ToList();

            Assert.Equal(((List<AppUser>)initial)[0].Username, result[0].Username);
            Assert.Equal(((List<AppUser>)initial)[0].FirstName, result[0].FirstName);
            Assert.Equal(((List<AppUser>)initial)[0].LastName, result[0].LastName);
            Assert.Equal(((List<AppUser>)initial)[0].Email, result[0].Email);
        }

        [Fact]
        public void MapIntoRestaurantModelListShouldMatchValues()
        {
            List<RestaurantModel> result;
            IEnumerable<Restaurant> initial = new List<Restaurant>() { new Restaurant()
            {
                Id = "Id",
                Name = "name",
                Hours = "9-5",
                Lat = "111.0524864",
                Lon = "1aefase64",
                Address = "123 W nope",
                Rating = 1.0m,
                PriceLevel = 1.5873m
            }};

            result = Mapper.Map(initial).ToList();

            Assert.Equal(((List<Restaurant>)initial)[0].Id, result[0].Id);
            Assert.Equal(((List<Restaurant>)initial)[0].Name, result[0].Name);
            Assert.Equal(((List<Restaurant>)initial)[0].Hours, result[0].Hours);
            Assert.Equal(((List<Restaurant>)initial)[0].Lat, result[0].Lat);
            Assert.Equal(((List<Restaurant>)initial)[0].Lon, result[0].Lon);
            Assert.Equal(((List<Restaurant>)initial)[0].Address, result[0].Address);
            Assert.Equal(((List<Restaurant>)initial)[0].Rating, result[0].Rating);
            Assert.Equal(((List<Restaurant>)initial)[0].PriceLevel, result[0].PriceLevel);
            Assert.Equal(((List<Restaurant>)initial)[0].Owner, result[0].Owner);
        }

        [Fact]
        public void MapIntoRestaurantListShouldMatchValues()
        {
            List<Restaurant> result;
            IEnumerable<RestaurantModel> initial = new List<RestaurantModel>() { new RestaurantModel()
            {
                Id = "Id",
                Name = "name",
                Hours = "9-5",
                Lat = "111.0524864",
                Lon = "1aefase64",
                Address = "123 W nope",
                Rating = 1.0m,
                PriceLevel = 1.5873m
            }};

            result = Mapper.Map(initial).ToList();

            Assert.Equal(((List<RestaurantModel>)initial)[0].Id, result[0].Id);
            Assert.Equal(((List<RestaurantModel>)initial)[0].Name, result[0].Name);
            Assert.Equal(((List<RestaurantModel>)initial)[0].Hours, result[0].Hours);
            Assert.Equal(((List<RestaurantModel>)initial)[0].Lat, result[0].Lat);
            Assert.Equal(((List<RestaurantModel>)initial)[0].Lon, result[0].Lon);
            Assert.Equal(((List<RestaurantModel>)initial)[0].Address, result[0].Address);
            Assert.Equal(((List<RestaurantModel>)initial)[0].Rating, result[0].Rating);
            Assert.Equal(((List<RestaurantModel>)initial)[0].PriceLevel, result[0].PriceLevel);
            Assert.Equal(((List<RestaurantModel>)initial)[0].Owner, result[0].Owner);
        }

        [Fact]
        public void MapIntoQueryModelListShouldMatchValues()
        {
            List<QueryModel> result;
            IEnumerable<Query> initial = new List<Query>() { new Query()
            {
                Id = 7,
                Username = "name",
                Lat = "111.0524864",
                Lon = "1aefase64",
                Radius = 1,
                QueryTime = new DateTime()
            }};

            result = Mapper.Map(initial).ToList();

            Assert.Equal(((List<Query>)initial)[0].Id, result[0].Id);
            Assert.Equal(((List<Query>)initial)[0].Username, result[0].Username);
            Assert.Equal(((List<Query>)initial)[0].Lat, result[0].Lat);
            Assert.Equal(((List<Query>)initial)[0].Lon, result[0].Lon);
            Assert.Equal(((List<Query>)initial)[0].Radius, result[0].Radius);
            Assert.Equal(((List<Query>)initial)[0].QueryTime, result[0].QueryTime);
        }

        [Fact]
        public void MapIntoQueryListShouldMatchValues()
        {
            List<Query> result;
            IEnumerable<QueryModel> initial = new List<QueryModel>() { new QueryModel()
            {
                Id = 7,
                Username = "name",
                Lat = "111.0524864",
                Lon = "1aefase64",
                Radius = 1,
                QueryTime = new DateTime()
            }};

            result = Mapper.Map(initial).ToList();

            Assert.Equal(((List<QueryModel>)initial)[0].Id, result[0].Id);
            Assert.Equal(((List<QueryModel>)initial)[0].Username, result[0].Username);
            Assert.Equal(((List<QueryModel>)initial)[0].Lat, result[0].Lat);
            Assert.Equal(((List<QueryModel>)initial)[0].Lon, result[0].Lon);
            Assert.Equal(((List<QueryModel>)initial)[0].Radius, result[0].Radius);
            Assert.Equal(((List<QueryModel>)initial)[0].QueryTime, result[0].QueryTime);
        }
    }
}
