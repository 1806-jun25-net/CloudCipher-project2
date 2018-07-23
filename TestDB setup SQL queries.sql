--CREATE SCHEMA Testing;

--DROP TABLE Testing.Users;
CREATE TABLE Testing.Users
(
	Username nvarchar(128) PRIMARY KEY,
	FirstName nvarchar(128),
	LastName nvarchar(128),
	FavQuery nvarchar(128)
);

--DROP TABLE Testing.Restaurants
CREATE TABLE Testing.Restaurants
(
	ID int PRIMARY KEY,
	Name nvarchar(128),
	Phone nvarchar(128),
	Hours nvarchar(128),
	Location nvarchar(128),
	Location2 nvarchar(128),
);

--location2 created as an option to store latitude and longitude in 2 separate fields if desired, 
--otherwise just store them all in location and use 2 null