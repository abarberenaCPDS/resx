-- ------------------------------------------------------
-- Script 20: How to overcome Blocking in SQL Server?
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------

/*
In this demostration we will see how we can overcome the IO limitations. 
*/
SET STATISTICS IO ON
GO
USE master
GO
-- Creating Files on Single Drive
CREATE DATABASE [SingleFileDB] ON  PRIMARY 
( NAME = N'SingleFileDB', FILENAME = N'H:\Data\SingleFileDB.mdf' , SIZE = 25048KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SingleFileDB_log', FILENAME = N'H:\Data\SingleFileDB_log.ldf' , SIZE = 25048KB , FILEGROWTH = 10%)
GO
-- Creating Files on Multiple Drive
CREATE DATABASE [DataOnFastDrive] ON  PRIMARY 
( NAME = N'DataOnFastDrive', FILENAME = N'D:\Data\DataOnFastDrive.mdf' , SIZE = 25048KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DataOnFastDrive_log', FILENAME = N'H:\Data\DataOnFastDrive_log.ldf' , SIZE = 25048KB , FILEGROWTH = 10%)
GO
-- Creating Files on Multiple Drive
CREATE DATABASE [DataOnSlowDrive] ON  PRIMARY 
( NAME = N'DataOnSlowDrive', FILENAME = N'H:\Data\DataOnSlowDrive.mdf' , SIZE = 25048KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DataOnSlowDrive_log', FILENAME = N'D:\Data\DataOnSlowDrive_log.ldf' , SIZE = 25048KB , FILEGROWTH = 10%)
GO
-- Creating Files on Faster Drive
CREATE DATABASE [BothOnFasterDrive] ON  PRIMARY 
( NAME = N'BothOnFasterDrive', FILENAME = N'D:\Data\BothOnFasterDrive.mdf' , SIZE = 25048KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BothOnFasterDrive_log', FILENAME = N'D:\Data\BothOnFasterDrive_log.ldf' , SIZE = 25048KB , FILEGROWTH = 10%)
GO


-------------------------------------------------------------------------
/* Insert Test */
-------------------------------------------------------------------------
-- Insert into SingleFileDB
-- Create Table TestTable
USE SingleFileDB
GO
CREATE TABLE [TestTable] (ID INT, 
						FirstName VARCHAR(100), 
						LastName VARCHAR(100), 
						City VARCHAR(100))
GO
-- Insert one Million Records
INSERT INTO [TestTable] (ID,FirstName,LastName,City)
SELECT TOP 500000 ROW_NUMBER() OVER (ORDER BY a.name) RowID, 
					'Bob'+ CAST(ROW_NUMBER() OVER (ORDER BY a.name)%10 AS VARCHAR), 
					CASE WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%2 = 1 THEN 'Smith' 
					ELSE 'Brown' END,
					CASE WHEN ROW_NUMBER() OVER (ORDER BY a.name)%10 = 1 THEN 'New York' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 5 THEN 'San Marino' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 3 THEN 'Los Angeles' 
					ELSE 'Houston' END
FROM sys.all_objects a
CROSS JOIN sys.all_objects b
GO 
-- Insert into DataOnFastDrive
-- Create Table TestTable
USE [DataOnFastDrive]
GO
CREATE TABLE [TestTable] (ID INT, 
						FirstName VARCHAR(100), 
						LastName VARCHAR(100), 
						City VARCHAR(100))
GO
-- Insert one Million Records
INSERT INTO [TestTable] (ID,FirstName,LastName,City)
SELECT TOP 500000 ROW_NUMBER() OVER (ORDER BY a.name) RowID, 
					'Bob'+ CAST(ROW_NUMBER() OVER (ORDER BY a.name)%10 AS VARCHAR), 
					CASE WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%2 = 1 THEN 'Smith' 
					ELSE 'Brown' END,
					CASE WHEN ROW_NUMBER() OVER (ORDER BY a.name)%10 = 1 THEN 'New York' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 5 THEN 'San Marino' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 3 THEN 'Los Angeles' 
					ELSE 'Houston' END
FROM sys.all_objects a
CROSS JOIN sys.all_objects b
GO 
-- Insert into DataOnSlowDrive
-- Create Table TestTable
USE [DataOnSlowDrive]
GO
CREATE TABLE [TestTable] (ID INT, 
						FirstName VARCHAR(100), 
						LastName VARCHAR(100), 
						City VARCHAR(100))
GO
-- Insert one Million Records
INSERT INTO [TestTable] (ID,FirstName,LastName,City)
SELECT TOP 500000 ROW_NUMBER() OVER (ORDER BY a.name) RowID, 
					'Bob'+ CAST(ROW_NUMBER() OVER (ORDER BY a.name)%10 AS VARCHAR), 
					CASE WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%2 = 1 THEN 'Smith' 
					ELSE 'Brown' END,
					CASE WHEN ROW_NUMBER() OVER (ORDER BY a.name)%10 = 1 THEN 'New York' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 5 THEN 'San Marino' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 3 THEN 'Los Angeles' 
					ELSE 'Houston' END
FROM sys.all_objects a
CROSS JOIN sys.all_objects b
GO 
-- Insert into BothOnFasterDrive
-- Create Table TestTable
USE [BothOnFasterDrive]
GO
CREATE TABLE [TestTable] (ID INT, 
						FirstName VARCHAR(100), 
						LastName VARCHAR(100), 
						City VARCHAR(100))
GO
-- Insert one Million Records
INSERT INTO [TestTable] (ID,FirstName,LastName,City)
SELECT TOP 500000 ROW_NUMBER() OVER (ORDER BY a.name) RowID, 
					'Bob'+ CAST(ROW_NUMBER() OVER (ORDER BY a.name)%10 AS VARCHAR), 
					CASE WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%2 = 1 THEN 'Smith' 
					ELSE 'Brown' END,
					CASE WHEN ROW_NUMBER() OVER (ORDER BY a.name)%10 = 1 THEN 'New York' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 5 THEN 'San Marino' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 3 THEN 'Los Angeles' 
					ELSE 'Houston' END
FROM sys.all_objects a
CROSS JOIN sys.all_objects b
GO 
-------------------------------------------------------------------------
/* Select Test */
-------------------------------------------------------------------------
USE SingleFileDB
GO
SELECT *
FROM TestTable
GO 
USE DataOnFastDrive
GO 
SELECT *
FROM TestTable
GO
USE DataOnSlowDrive
GO
SELECT *
FROM TestTable
GO
USE BothOnFasterDrive
GO
SELECT *
FROM TestTable
GO

-- Cleanup
USE master
GO
ALTER DATABASE SingleFileDB
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE SingleFileDB
GO
USE master
GO
ALTER DATABASE DataOnFastDrive
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE DataOnFastDrive
GO
USE master
GO
ALTER DATABASE DataOnSlowDrive
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE DataOnSlowDrive
GO
USE master
GO
ALTER DATABASE BothOnFasterDrive
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE BothOnFasterDrive
GO


