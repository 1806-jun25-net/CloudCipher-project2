using RestaurantAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantAPI.Testing
{
    public static class RepoTestInMemoryDBSetup
    {
        public static void Setup(Project2DBContext context)
        {
            //Only initialize the DB with data once
            if (context.AppUser.ToList().Count == 0)
            {
                context.AppUser.Add(new AppUser { Username = "realUser", FirstName = "a", LastName = "b", Email = "e" });
                context.AppUser.Add(new AppUser { Username = "decoyUser1", FirstName = "a", LastName = "b", Email = "e" });
                context.AppUser.Add(new AppUser { Username = "decoyUser2", FirstName = "a", LastName = "b", Email = "e" });
                context.AppUser.Add(new AppUser { Username = "decoyUser3", FirstName = "a", LastName = "b", Email = "e" });

                context.Restaurant.Add(new Restaurant { Id = "1a", Name = "1", Lat = "loc", Lon = "loc", Owner = "realUser" });
                context.Restaurant.Add(new Restaurant { Id = "2b", Name = "2", Lat = "loc", Lon = "loc" });
                context.Restaurant.Add(new Restaurant { Id = "3c", Name = "3", Lat = "loc", Lon = "loc" });
                context.Restaurant.Add(new Restaurant { Id = "4d", Name = "4", Lat = "loc", Lon = "loc" });
                context.Restaurant.Add(new Restaurant { Id = "5e", Name = "5", Lat = "loc", Lon = "loc", Owner = "realUser" });
                context.Restaurant.Add(new Restaurant { Id = "6f", Name = "6", Lat = "loc", Lon = "loc" });
                context.Restaurant.Add(new Restaurant { Id = "7g", Name = "7", Lat = "loc", Lon = "loc" });
                context.Restaurant.Add(new Restaurant { Id = "8h", Name = "8", Lat = "loc", Lon = "loc" });
                context.Restaurant.Add(new Restaurant { Id = "9i", Name = "9", Lat = "loc", Lon = "loc" });


                context.Blacklist.Add(new Blacklist { Username = "realUser", RestaurantId = "2b" });
                context.Blacklist.Add(new Blacklist { Username = "realUser", RestaurantId = "4d" });
                context.Blacklist.Add(new Blacklist { Username = "realUser", RestaurantId = "6f" });

                context.Favorite.Add(new Favorite { Username = "realUser", RestaurantId = "1a" });
                context.Favorite.Add(new Favorite { Username = "realUser", RestaurantId = "3c" });
                context.Favorite.Add(new Favorite { Username = "realUser", RestaurantId = "5e" });
                context.Favorite.Add(new Favorite { Username = "realUser", RestaurantId = "7g" });

                context.Keyword.Add(new Keyword { Word = "breakfast" });
                context.Keyword.Add(new Keyword { Word = "fast" });
                context.Keyword.Add(new Keyword { Word = "food" });

                context.Query.Add(new Query { Id = 1, Username = "realUser", QueryTime = DateTime.Now });
                context.Query.Add(new Query { Id = 2, Username = "realUser", QueryTime = DateTime.Now });
                context.Query.Add(new Query { Id = 3, Username = "realUser", QueryTime = DateTime.Now });
                context.Query.Add(new Query { Id = 4, Username = "realUser", QueryTime = DateTime.Now });
                context.Query.Add(new Query { Id = 5, Username = "realUser", QueryTime = DateTime.Now });
                context.Query.Add(new Query { Id = 6, Username = "realUser", QueryTime = DateTime.Now });
                context.Query.Add(new Query { Id = 7, Username = "realUser", QueryTime = DateTime.Now });
                context.Query.Add(new Query { Id = 8, Username = "realUser", QueryTime = DateTime.Now });

                context.QueryKeywordJunction.Add(new QueryKeywordJunction { QueryId = 1, Word = "breakfast" });
                context.QueryKeywordJunction.Add(new QueryKeywordJunction { QueryId = 1, Word = "fast" });
                context.QueryKeywordJunction.Add(new QueryKeywordJunction { QueryId = 1, Word = "food" });

                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 1, RestaurantId = "1a" });
                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 2, RestaurantId = "1a" });
                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 2, RestaurantId = "2b" });
                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 3, RestaurantId = "1a" });
                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 3, RestaurantId = "2b" });
                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 3, RestaurantId = "3c" });
                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 4, RestaurantId = "1a" });
                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 4, RestaurantId = "2b" });
                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 4, RestaurantId = "3c" });
                context.QueryRestaurantJunction.Add(new QueryRestaurantJunction { QueryId = 4, RestaurantId = "4d" });

                context.SaveChanges();
            }
        }
    }
}
