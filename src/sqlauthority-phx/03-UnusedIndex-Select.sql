-- ------------------------------------------------------
-- Script 02: Why unused indexes are bad? (Select)
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------

/*
In this example we will see effect of unused index on Select Statement
We will create indexes on table and see the performance degradation for Select
*/

SET STATISTICS IO ON
GO
USE AdventureWorks2014
GO
-- Sample Query
SELECT SalesOrderDetailID, OrderQty
FROM [Sales].[SalesOrderDetail] sod
WHERE ProductID = (SELECT AVG(ProductID)
FROM [Sales].[SalesOrderDetail] sod1
WHERE sod.SalesOrderID = sod1.SalesOrderID
GROUP BY SalesOrderID)
GO 

-- Update Statistics of Table
UPDATE STATISTICS [Sales].[SalesOrderDetail] 
WITH FULLSCAN; 
GO  

-- Create an Index
CREATE NONCLUSTERED INDEX [IX_FirstTry] 
		ON [Sales].[SalesOrderDetail]
		([SalesOrderID] ASC, [ProductID] ASC)
INCLUDE ([SalesOrderDetailID], [OrderQty])
GO

-- Sample Query
SELECT SalesOrderDetailID, OrderQty
FROM [Sales].[SalesOrderDetail] sod
WHERE ProductID = (SELECT AVG(ProductID)
FROM [Sales].[SalesOrderDetail] sod1
WHERE sod.SalesOrderID = sod1.SalesOrderID
GROUP BY SalesOrderID)
GO

-- Create an Index
CREATE NONCLUSTERED INDEX [IX_SecondTry] 
		ON [Sales].[SalesOrderDetail]
		([ProductID] ASC, [SalesOrderID] ASC)
INCLUDE ([SalesOrderDetailID], [OrderQty])
GO

-- Clear the Cache
DBCC FREEPROCCACHE
GO

-- Sample Query
SELECT SalesOrderDetailID, OrderQty
FROM [Sales].[SalesOrderDetail] sod
WHERE ProductID = (SELECT AVG(ProductID)
FROM [Sales].[SalesOrderDetail] sod1
WHERE sod.SalesOrderID = sod1.SalesOrderID
GROUP BY SalesOrderID)
GO

USE [AdventureWorks2014]
GO
-- Drop Index
DROP INDEX [IX_FirstTry] ON [Sales].[SalesOrderDetail]
GO
-- Drop Index
DROP INDEX [IX_SecondTry] ON [Sales].[SalesOrderDetail]
GO



