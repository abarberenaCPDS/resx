-- ------------------------------------------------------
-- Script 07: Do COLUMNSTORE indexes help performance of query?
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------

/*
Columnstore index can help query performance for OLAP and datawarehousing.
*/

USE AdventureWorks2014
GO
-- Create New Table
CREATE TABLE [dbo].[MySalesOrderDetail](
[SalesOrderID] [int] NOT NULL,
[SalesOrderDetailID] [int] NOT NULL,
[CarrierTrackingNumber] [nvarchar](25) NULL,
[OrderQty] [smallint] NOT NULL,
[ProductID] [int] NOT NULL,
[SpecialOfferID] [int] NOT NULL,
[UnitPrice] [money] NOT NULL,
[UnitPriceDiscount] [money] NOT NULL,
[LineTotal] [numeric](38, 6) NOT NULL,
[rowguid] [uniqueidentifier] NOT NULL,
[ModifiedDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
-- Create clustered index
CREATE CLUSTERED INDEX [CL_MySalesOrderDetail] ON [dbo].[MySalesOrderDetail]
( [SalesOrderDetailID])
GO
-- Create Sample Data Table
-- WARNING: This Query may run upto 2-10 minutes based on your systems resources
INSERT INTO [dbo].[MySalesOrderDetail]
SELECT [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber], 
[OrderQty],[ProductID],[SpecialOfferID],[UnitPrice],
[UnitPriceDiscount],[LineTotal],[rowguid],[ModifiedDate]
FROM Sales.SalesOrderDetail S1
GO 50


-- Performance Test
-- Comparing Regular Index with ColumnStore Index
USE AdventureWorks2014
GO
SET STATISTICS IO ON
GO
-- Select Table with regular Index
SELECT ProductID, SUM(UnitPrice) SumUnitPrice, AVG(UnitPrice) AvgUnitPrice,
SUM(OrderQty) SumOrderQty, AVG(OrderQty) AvgOrderQty
FROM [dbo].[MySalesOrderDetail]
GROUP BY ProductID
ORDER BY ProductID
GO
-- Table 'MySalesOrderDetail'. Scan count 1, logical reads 342261, physical reads 0, read-ahead reads 0.
-- Create ColumnStore Index
CREATE NONCLUSTERED COLUMNSTORE INDEX [IX_MySalesOrderDetail_ColumnStore]
ON [MySalesOrderDetail]
(UnitPrice, OrderQty, ProductID)
GO
-- Select Table with Columnstore Index
SELECT ProductID, SUM(UnitPrice) SumUnitPrice, AVG(UnitPrice) AvgUnitPrice,
SUM(OrderQty) SumOrderQty, AVG(OrderQty) AvgOrderQty
FROM [dbo].[MySalesOrderDetail]
GROUP BY ProductID
ORDER BY ProductID
GO


TRUNCATE TABLE dbo.MySalesOrderDetail
GO
DROP TABLE dbo.MySalesOrderDetail
GO
