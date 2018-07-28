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
            UserType = otherUser.UserType
        };

        public static UserModel Map(AppUser otherUser) => new UserModel
        {
            Username = otherUser.Username,
            FirstName = otherUser.FirstName,
            LastName = otherUser.LastName,
            Email = otherUser.Email,
            UserType = otherUser.UserType
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


        // collection of objects conversions

        public static IEnumerable<UserModel> Map(IEnumerable<AppUser> others) => others.Select(Map);
        public static IEnumerable<AppUser> Map(IEnumerable<UserModel> others) => others.Select(Map);

        public static IEnumerable<RestaurantModel> Map(IEnumerable<Restaurant> others) => others.Select(Map);
        public static IEnumerable<Restaurant> Map(IEnumerable<RestaurantModel> others) => others.Select(Map);


    }
}
