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
	Lat nvarchar(128),
	Lon nvarchar(128),
	Address nvarchar(128),
	Rating decimal,
	PriceLevel decimal,
	Owner nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.AppUser(Username)
);
--ALTER TABLE RestaurantSite.Restaurant ALTER COLUMN Lat nvarchar(128) NULL;
--ALTER TABLE RestaurantSite.Restaurant ALTER COLUMN Lon nvarchar(128) NULL;

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
VALUES ('test', 'First', 'Last', 'a@a.com'),
		('admin', 'Tess', 'Est', 'winner@gmail.com');

INSERT INTO RestaurantSite.AppUser
VALUES ('admin.2', 'The', 'Admin', 'a@a.com');

INSERT INTO RestaurantSite.AppUser
VALUES ('RegisterTest1', 'Reggie', 'T', 'rr@t.org');

SELECT * FROM RestaurantSite.AppUser;

SELECT * FROM RestaurantSite.Query;

SELECT * FROM RestaurantSite.QueryKeywordJunction;

SELECT * FROM RestaurantSite.Restaurant WHERE (id = '23128183dd152237b6a91accbed5035268e88027') Order By Name;

SELECT * FROM RestaurantSite.RestaurantKeywordJunction;

SELECT * FROM RestaurantSite.Keyword;

SELECT * FROM RestaurantSite.QueryRestaurantJunction;

SELECT * FROM RestaurantSite.Blacklist WHERE (Username = 'admin.2') Order By RestaurantID;

SELECT * FROM RestaurantSite.Favorite;
/*
DELETE FROM RestaurantSite.QueryKeywordJunction;
DELETE FROM RestaurantSite.QueryRestaurantJunction;
DELETE FROM RestaurantSite.RestaurantKeywordJunction;
DELETE FROM RestaurantSite.Blacklist;
DELETE FROM RestaurantSite.Favorite;
DELETE FROM RestaurantSite.Keyword;
DELETE FROM RestaurantSite.Restaurant;
DELETE FROM RestaurantSite.Query;
*/