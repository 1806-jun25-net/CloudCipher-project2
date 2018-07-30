using RestaurantAPI.API.Models;
using RestaurantAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantAPI.Library
{
    public static class Mapper
    {
        // single object conversions
        public static AppUser Map(UserModel otherUser) => new AppUser
        {
            Username = otherUser.Username,
            FirstName = otherUser.FirstName,
            LastName = otherUser.LastName,
            Email = otherUser.Email,
            
        };

        public static UserModel Map(AppUser otherUser) => new UserModel
        {
            Username = otherUser.Username,
            FirstName = otherUser.FirstName,
            LastName = otherUser.LastName,
            Email = otherUser.Email,
        };

        public static RestaurantModel Map(Restaurant other) => new RestaurantModel
        {
            Id = other.Id,
            Name = other.Name,
            Phone = other.Phone,
            Hours = other.Hours,
            Location = other.Location,
            Location2 = other.Location2,
            Owner = other.Owner
        };

        public static Restaurant Map(RestaurantModel other) => new Restaurant
        {
            Id = other.Id,
            Name = other.Name,
            Phone = other.Phone,
            Hours = other.Hours,
            Location = other.Location,
            Location2 = other.Location2,
            Owner = other.Owner
        };

        public static QueryModel Map(Query other) => new QueryModel
        {
            Id = other.Id,
            Username = other.Username,
            Location = other.Location,
            Location2 = other.Location2,
            Radius = (int)other.Radius,
            QueryTime = other.QueryTime,
            ReservationTime = (DateTime)other.ReservationTime,
            Keywords = other.QueryKeywordJunction.Select(q => q.Word).ToList()
        };

        public static Query Map(QueryModel other) => new Query
        {
            Id = other.Id,
            Username = other.Username,
            Location = other.Location,
            Location2 = other.Location2,
            Radius = other.Radius,
            QueryTime = other.QueryTime,
            ReservationTime = other.ReservationTime,
            QueryKeywordJunction = other.Keywords.Select( k => new QueryKeywordJunction() { Word = k, QueryId = other.Id }).ToList()
        };

        public static string Map(Keyword other) => other.Word;
        public static Keyword Map(string other) => new Keyword { Word = other };


        // collection of objects conversions

        public static IEnumerable<UserModel> Map(IEnumerable<AppUser> others) => others.Select(Map);
        public static IEnumerable<AppUser> Map(IEnumerable<UserModel> others) => others.Select(Map);

        public static IEnumerable<RestaurantModel> Map(IEnumerable<Restaurant> others) => others.Select(Map);
        public static IEnumerable<Restaurant> Map(IEnumerable<RestaurantModel> others) => others.Select(Map);

        public static IEnumerable<QueryModel> Map(IEnumerable<Query> others) => others.Select(Map);
        public static IEnumerable<Query> Map(IEnumerable<QueryModel> others) => others.Select(Map);

        public static IEnumerable<string> Map(IEnumerable<Keyword> others) => others.Select(Map);
        public static IEnumerable<Keyword> Map(IEnumerable<string> others) => others.Select(Map);

    }
}
