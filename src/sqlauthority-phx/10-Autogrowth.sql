-- ------------------------------------------------------
-- Script 06: How important the file growth setting is for Database?
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------

/*
File Autogrowth is very important property of MDF, NDF and LDF file in
SQL Server. It should be ideally set to monthly filegrowth. 
*/
-- Create Default Database
USE master
GO
CREATE DATABASE [DefaultDB] ON
( NAME = N'DefaultDB', FILENAME = N'E:\Data\DefaultDB.mdf')
 LOG ON 
( NAME = N'DefaultDB_log', FILENAME = N'E:\Data\DefaultDB_log.LDF')
GO
USE DefaultDB
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DefaultTable]') AND type in (N'U'))
DROP TABLE [dbo].[DefaultTable]
GO
-- Create Table DefaultTable
CREATE TABLE DefaultTable (ID INT, 
						FirstName VARCHAR(100), 
						LastName VARCHAR(100), 
						City VARCHAR(100))
GO
-- Insert Records
INSERT INTO DefaultTable (ID,FirstName,LastName,City)
SELECT TOP 100000 ROW_NUMBER() OVER (ORDER BY a.name) RowID, 
					'Mike'+ CAST(ROW_NUMBER() OVER (ORDER BY a.name)%10 AS VARCHAR), 
					CASE WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%2 = 1 THEN 'Smith' 
					ELSE 'Brown' END,
					CASE WHEN ROW_NUMBER() OVER (ORDER BY a.name)%10 = 1 THEN 'New York' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 5 THEN 'San Marino' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 3 THEN 'Los Angeles' 
					ELSE 'Houston' END
FROM sys.all_objects a
CROSS JOIN sys.all_objects b
GO 
USE master
GO
ALTER DATABASE DefaultDB
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE DefaultDB
GO



-- Create Default Database
USE master
GO
CREATE DATABASE [OptimizeDB] ON
( NAME = N'OptimizeDB', FILENAME = N'E:\Data\OptimizeDB.mdf' , 
		SIZE = 102400KB , MAXSIZE = UNLIMITED, FILEGROWTH = 102400KB )
 LOG ON 
( NAME = N'OptimizeDB_log', FILENAME = N'E:\Data\OptimizeDB_log.LDF' , 
		SIZE = 102400KB , MAXSIZE = UNLIMITED, FILEGROWTH = 102400KB )
GO
USE OptimizeDB
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DefaultTable]') AND type in (N'U'))
DROP TABLE [dbo].[DefaultTable]
GO
-- Create Table DefaultTable
CREATE TABLE DefaultTable (ID INT, 
						FirstName VARCHAR(100), 
						LastName VARCHAR(100), 
						City VARCHAR(100))
GO
-- Insert Records
INSERT INTO DefaultTable (ID,FirstName,LastName,City)
SELECT TOP 100000 ROW_NUMBER() OVER (ORDER BY a.name) RowID, 
					'Mike'+ CAST(ROW_NUMBER() OVER (ORDER BY a.name)%10 AS VARCHAR), 
					CASE WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%2 = 1 THEN 'Smith' 
					ELSE 'Brown' END,
					CASE WHEN ROW_NUMBER() OVER (ORDER BY a.name)%10 = 1 THEN 'New York' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 5 THEN 'San Marino' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 3 THEN 'Los Angeles' 
					ELSE 'Houston' END
FROM sys.all_objects a
CROSS JOIN sys.all_objects b
GO 
USE master
GO
ALTER DATABASE OptimizeDB
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE OptimizeDB
GO