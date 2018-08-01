CREATE SCHEMA RestaurantSite;

--DROP TABLE RestaurantSite.AppUser
CREATE TABLE RestaurantSite.AppUser
(
	Username nvarchar(128) PRIMARY KEY,
	FirstName nvarchar(128) NOT NULL,
	LastName nvarchar(128) NOT NULL,
	Email nvarchar(128) NOT NULL
);
--ALTER TABLE RestaurantSite.AppUser DROP COLUMN UserType ;

--DROP TABLE RestaurantSite.Restaurant
CREATE TABLE RestaurantSite.Restaurant
(
	ID nvarchar(128) PRIMARY KEY,
	Name nvarchar(128) NOT NULL,
	Hours nvarchar(256),
	Lat nvarchar(128) NOT NULL,
	Lon nvarchar(128) NOT NULL,
	Address nvarchar(128),
	Rating decimal,
	PriceLevel decimal,
	Owner nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.AppUser(Username)
);

--DROP TABLE RestaurantSite.Query
CREATE TABLE RestaurantSite.Query
(
	ID int PRIMARY KEY IDENTITY,
	Username nvarchar(128) NOT NULL FOREIGN KEY REFERENCES RestaurantSite.AppUser(Username),
	Lat nvarchar(128),
	Lon nvarchar(128),
	Radius int,
	QueryTime DateTime NOT NULL
);

--DROP TABLE RestaurantSite.Keyword
CREATE TABLE RestaurantSite.Keyword
(
	Word nvarchar(128) PRIMARY KEY
);


--DROP TABLE RestaurantSite.Favorite
CREATE TABLE RestaurantSite.Favorite
(
	RestaurantID nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.Restaurant(ID),
	Username nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.AppUser(Username),
	PRIMARY KEY (RestaurantID, Username)
);

--DROP TABLE RestaurantSite.Blacklist
CREATE TABLE RestaurantSite.Blacklist
(
	RestaurantID nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.Restaurant(ID),
	Username nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.AppUser(Username),
	PRIMARY KEY (RestaurantID, Username)
);

--DROP TABLE RestaurantSite.QueryKeywordJunction
CREATE TABLE RestaurantSite.QueryKeywordJunction
(
	QueryID int FOREIGN KEY REFERENCES RestaurantSite.Query(ID),
	Word nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.Keyword(Word),
	PRIMARY KEY (QueryID, Word)
);

--DROP TABLE RestaurantSite.RestaurantKeywordJunction
CREATE TABLE RestaurantSite.RestaurantKeywordJunction
(
	RestaurantID nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.Restaurant(ID),
	Word nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.Keyword(Word),
	PRIMARY KEY (RestaurantID, Word)
);

--DROP TABLE RestaurantSite.QueryRestaurantJunction
CREATE TABLE RestaurantSite.QueryRestaurantJunction
(
	QueryID int FOREIGN KEY REFERENCES RestaurantSite.Query(ID),
	RestaurantID nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.Restaurant(ID),
	PRIMARY KEY (QueryID, RestaurantID)
);


--Inserting test data:

INSERT INTO RestaurantSite.AppUser
VALUES ('test', 'First', 'Last', 'a@a.com', 0),
		('admin', 'Tess', 'Est', 'winner@gmail.com', 1);

SELECT * FROM RestaurantSite.AppUser;

SELECT * FROM RestaurantSite.Query;

SELECT * FROM RestaurantSite.QueryKeywordJunction;

SELECT * FROM RestaurantSite.Restaurant;

SELECT * FROM RestaurantSite.RestaurantKeywordJunction;

SELECT * FROM RestaurantSite.Keyword;

SELECT * FROM RestaurantSite.QueryRestaurantJunction;

--DELETE FROM RestaurantSite.Query;