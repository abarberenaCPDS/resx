-- ------------------------------------------------------
-- Script 09: What about OLTP In-Memory Table with COLUMNSTORE indexes?
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------

/*
Simple demo to showcase ColumnStore Index and In-Memory OLTP Together
*/

USE WideWorldImporters
GO

-- Regular Table
CREATE TABLE Regular_People
([ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY NONCLUSTERED, 
[FullName] VARCHAR(256) NOT NULL,
[UnitPrice] INT NOT NULL,
[Quantity] INT NOT NULL
)
GO

-- In-Memory OLTP Table
CREATE TABLE InMemory_People
([ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 100000), 
[FullName] VARCHAR(256) NOT NULL,
[UnitPrice] INT NOT NULL,
[Quantity] INT NOT NULL
) WITH (MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_AND_DATA)
GO

-- Columnstore Table
CREATE TABLE Columnstore_People
([ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY , 
[FullName] VARCHAR(256) NOT NULL,
[UnitPrice] INT NOT NULL,
[Quantity] INT NOT NULL,
INDEX IX_InMemory_Columnstore_People CLUSTERED COLUMNSTORE
) 
GO

-- In-Memory OLTP Columnstore Table
CREATE TABLE InMemory_Columnstore_People
([ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 100000), 
[FullName] VARCHAR(256) NOT NULL,
[UnitPrice] INT NOT NULL,
[Quantity] INT NOT NULL,
INDEX IX_InMemory_Columnstore_People CLUSTERED COLUMNSTORE
) WITH (MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_AND_DATA)
GO

-- Insert Into Regular Table - 10 Million Rows
INSERT INTO Regular_People
SELECT TOP 1000000 p.FullName, il.UnitPrice, ol.Quantity
FROM Application.People p 
INNER JOIN Sales.InvoiceLines il ON p.PersonID = il.LastEditedBy
INNER JOIN Sales.OrderLines ol ON il.StockItemID = ol.StockItemID
GO

-- Insert Into InMemory Table - 10 Million Rows
INSERT INTO InMemory_People
SELECT TOP 1000000 p.FullName, il.UnitPrice, ol.Quantity
FROM Application.People p 
INNER JOIN Sales.InvoiceLines il ON p.PersonID = il.LastEditedBy
INNER JOIN Sales.OrderLines ol ON il.StockItemID = ol.StockItemID
GO

-- Insert Into Columnstore Table - 10 Million Rows
INSERT INTO Columnstore_People
SELECT TOP 1000000 p.FullName, il.UnitPrice, ol.Quantity
FROM Application.People p 
INNER JOIN Sales.InvoiceLines il ON p.PersonID = il.LastEditedBy
INNER JOIN Sales.OrderLines ol ON il.StockItemID = ol.StockItemID
GO

-- Insert Into InMemory Columnstore Table - 10 Million Rows
INSERT INTO InMemory_Columnstore_People
SELECT TOP 1000000 p.FullName, il.UnitPrice, ol.Quantity
FROM Application.People p 
INNER JOIN Sales.InvoiceLines il ON p.PersonID = il.LastEditedBy
INNER JOIN Sales.OrderLines ol ON il.StockItemID = ol.StockItemID
GO

SET STATISTICS IO, TIME ON; 

-- SQL Server 2012
ALTER DATABASE WideWorldImporters 
SET COMPATIBILITY_LEVEL = 110
GO

-- SQL Server 2014
ALTER DATABASE WideWorldImporters 
SET COMPATIBILITY_LEVEL = 120
GO

-- SQL Server 2016
ALTER DATABASE WideWorldImporters 
SET COMPATIBILITY_LEVEL = 130
GO

-- SQL Server 2017
ALTER DATABASE WideWorldImporters 
SET COMPATIBILITY_LEVEL = 140
GO
-- --------------------------------------------------------------------------
-- Test 1
-- CTRL +M 
-- Select Data from Regular Table
SELECT FullName, SUM(UnitPrice) TotalUnitPrice, COUNT(Quantity) TotalQuantity
FROM Regular_People
GROUP BY FullName
ORDER BY FullName
GO

-- Select Data from InMemory Table
SELECT FullName, SUM(UnitPrice) TotalUnitPrice, COUNT(Quantity) TotalQuantity
FROM InMemory_People
GROUP BY FullName
ORDER BY FullName
GO

-- Select Data from Columnstore Table
SELECT FullName, SUM(UnitPrice) TotalUnitPrice, COUNT(Quantity) TotalQuantity
FROM Columnstore_People
GROUP BY FullName
ORDER BY FullName
GO

-- Select Data from InMemory Columnstore Table
SELECT FullName, SUM(UnitPrice) TotalUnitPrice, COUNT(Quantity) TotalQuantity
FROM InMemory_Columnstore_People
GROUP BY FullName
ORDER BY FullName
GO

-- --------------------------------------------------------------------------
-- Test 1
-- CTRL +M 
-- Select Data from Regular Table
SELECT DISTINCT FullName, UnitPrice, Quantity
FROM Regular_People
WHERE UnitPrice > 10
ORDER BY FullName
GO

-- Select Data from InMemory Table
SELECT DISTINCT FullName, UnitPrice, Quantity
FROM InMemory_People
WHERE UnitPrice > 10
ORDER BY FullName
GO

-- Select Data from Columnstore Table
SELECT DISTINCT FullName, UnitPrice, Quantity
FROM Columnstore_People
WHERE UnitPrice > 10
ORDER BY FullName
GO

-- Select Data from InMemory Columnstore Table
SELECT DISTINCT FullName, UnitPrice, Quantity
FROM InMemory_Columnstore_People
WHERE UnitPrice > 10
ORDER BY FullName
GO


-- Clean up
DROP TABLE Regular_People
GO
DROP TABLE InMemory_People
GO
DROP TABLE Columnstore_People
GO
DROP TABLE InMemory_Columnstore_People
GO