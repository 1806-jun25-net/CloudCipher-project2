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
	ID int PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(128) NOT NULL,
	Phone nvarchar(128),
	Hours nvarchar(128),
	Location nvarchar(128) NOT NULL,
	Location2 nvarchar(128),
	Owner nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.AppUser(Username)
);

--DROP TABLE RestaurantSite.Query
CREATE TABLE RestaurantSite.Query
(
	ID int PRIMARY KEY IDENTITY(1,1),
	Username nvarchar(128) NOT NULL FOREIGN KEY REFERENCES RestaurantSite.AppUser(Username),
	Location nvarchar(128),
	Location2 nvarchar(128),
	Radius int,
	QueryTime DateTime NOT NULL,
	ReservationTime DateTime
);

--DROP TABLE RestaurantSite.Keyword
CREATE TABLE RestaurantSite.Keyword
(
	Word nvarchar(128) PRIMARY KEY
);


--DROP TABLE RestaurantSite.Favorite
CREATE TABLE RestaurantSite.Favorite
(
	RestaurantID int FOREIGN KEY REFERENCES RestaurantSite.Restaurant(ID),
	Username nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.AppUser(Username),
	PRIMARY KEY (RestaurantID, Username)
);

--DROP TABLE RestaurantSite.Blacklist
CREATE TABLE RestaurantSite.Blacklist
(
	RestaurantID int FOREIGN KEY REFERENCES RestaurantSite.Restaurant(ID),
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
	RestaurantID int FOREIGN KEY REFERENCES RestaurantSite.Restaurant(ID),
	Word nvarchar(128) FOREIGN KEY REFERENCES RestaurantSite.Keyword(Word),
	PRIMARY KEY (RestaurantID, Word)
);





--Inserting test data:

INSERT INTO RestaurantSite.AppUser
VALUES ('test', 'First', 'Last', 'a@a.com', 0),
		('admin', 'Tess', 'Est', 'winner@gmail.com', 1);

SELECT * FROM RestaurantSite.AppUser;