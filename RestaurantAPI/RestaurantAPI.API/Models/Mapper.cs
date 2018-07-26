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



        // collection of objects conversions

        public static IEnumerable<UserModel> Map(IEnumerable<AppUser> otherUsers) => otherUsers.Select(Map);

        public static IEnumerable<AppUser> Map(IEnumerable<UserModel> otherUsers) => otherUsers.Select(Map);


    }
}
