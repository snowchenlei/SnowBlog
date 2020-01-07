--CREATE DATABASE ClBlog
--ON PRIMARY(
--	NAME = ClBlog_date,
--	FILENAME = 'C:\XX.mdf',
--	SIZE = 10,  
--    MAXSIZE = UNLIMITED,  
--    FILEGROWTH = 5
--)
--LOG ON 
--(  
--    NAME='ClBlog_dat',  
--    FILENAME='C:\XX.ldf',  
--    SIZE =5MB,  
--    MAXSIZE = 25MB,
--    FILEGROWTH =5MB
--)  
--GO
CREATE TABLE [Admin]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	UserName NVARCHAR(50),
	PassWord NVARCHAR(50),
	CreateDate DATETIME 
)

CREATE TABLE [Blog]
(
	ID 	INT IDENTITY(1,1) PRIMARY KEY,
	SourceType INT,
	Title NVARCHAR(50),
	CategoryId INT,
	Body VARCHAR(2000),
	Html  VARCHAR(2000),
	Description VARCHAR(2000),
	CreateDate DATETIME,
	EditDate DATETIME
)
CREATE TABLE [BlogTag]
(
	ID 	INT IDENTITY(1,1) PRIMARY KEY,
	BId INT,
	TId INT
)
CREATE TABLE [Category]
(
	ID 	INT IDENTITY(1,1) PRIMARY KEY,
	PID INT,
	Name VARCHAR(50),
	Sort INT,
	isDelete INT,
	CreateDate DATETIME,
	UpdateDate DATETIME
)
CREATE TABLE [Tag]
(
	ID 	INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(50),
	CreateDate DATETIME
)