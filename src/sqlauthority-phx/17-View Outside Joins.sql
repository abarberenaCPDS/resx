-- ------------------------------------------------------
--  Why unused indexes are bad? (Insert/Delete)
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------
SET STATISTICS IO ON
GO
USE AdventureWorks2014
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_ViewLimit1]'))
DROP VIEW [dbo].[vw_ViewLimit1]
GO
-- Create View on sample tables
CREATE VIEW vw_ViewLimit1
AS 
SELECT [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber]
      ,[OrderQty],sod.[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount]
      ,[LineTotal],[ReferenceOrderID]
FROM Sales.SalesOrderDetail sod
INNER JOIN Production.TransactionHistory th ON sod.SalesOrderID = th.ReferenceOrderID
WHERE SalesOrderDetailID > 111111
GO

SELECT [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber]
      ,[OrderQty],sod.[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount]
      ,[LineTotal],[ReferenceOrderID]
FROM Sales.SalesOrderDetail sod
INNER JOIN Production.TransactionHistory th ON sod.SalesOrderID = th.ReferenceOrderID
WHERE SalesOrderDetailID > 111111;

SELECT *
FROM vw_ViewLimit1;
-- Filter View with WHERE condition
SELECT *
FROM vw_ViewLimit1
WHERE SalesOrderDetailID > 111111
GO
-- Regular SELECT statement with WHERE condition
SELECT [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber]
      ,[OrderQty],sod.[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount]
      ,[LineTotal],[ReferenceOrderID]
FROM Sales.SalesOrderDetail sod
INNER JOIN Production.TransactionHistory th ON sod.SalesOrderID = th.ReferenceOrderID
WHERE SalesOrderDetailID > 111111
GO
/* Now let us try to retrieve the column which is not in View */
-- Select statement with extra column
SELECT [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber]
      ,[OrderQty],sod.[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount]
      ,[LineTotal],[ReferenceOrderID]
      ,th.[Quantity]
FROM Sales.SalesOrderDetail sod
INNER JOIN Production.TransactionHistory th ON sod.SalesOrderID = th.ReferenceOrderID
WHERE SalesOrderDetailID > 111111
GO
-- View with extra column
SELECT	 [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber]
		,[OrderQty], v1.[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount]
		,[LineTotal],v1.[ReferenceOrderID]
		,th.[Quantity]	
FROM vw_ViewLimit1 v1
INNER JOIN Production.TransactionHistory th ON v1.SalesOrderID = th.ReferenceOrderID
WHERE SalesOrderDetailID > 111111
GO



-- Let us create missing index
CREATE NONCLUSTERED INDEX IX_RightNow
ON [Sales].[SalesOrderDetail] ([SalesOrderDetailID])
INCLUDE ([SalesOrderID],[CarrierTrackingNumber],[OrderQty],[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount],[LineTotal])
GO
/* Now let us try to retrieve the column which is not in View */
-- Select statement with extra column
SELECT [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber]
      ,[OrderQty],sod.[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount]
      ,[LineTotal],[ReferenceOrderID]
      ,th.[Quantity]
FROM Sales.SalesOrderDetail sod
INNER JOIN Production.TransactionHistory th ON sod.SalesOrderID = th.ReferenceOrderID
WHERE SalesOrderDetailID > 111111
GO
-- View with extra column
SELECT	 [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber]
		,[OrderQty], v1.[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount]
		,[LineTotal],v1.[ReferenceOrderID]
		,th.[Quantity]	
FROM vw_ViewLimit1 v1
INNER JOIN Production.TransactionHistory th ON v1.SalesOrderID = th.ReferenceOrderID
WHERE SalesOrderDetailID > 111111
GO

/* Now let us add DISTINCT */
-- Select statement with extra column
SELECT DISTINCT [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber]
      ,[OrderQty],sod.[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount]
      ,[LineTotal],[ReferenceOrderID]
      ,th.[Quantity]
FROM Sales.SalesOrderDetail sod
INNER JOIN Production.TransactionHistory th ON sod.SalesOrderID = th.ReferenceOrderID
WHERE SalesOrderDetailID > 111111
--OPTION (MAXDOP 1) 
GO
-- View with extra column
SELECT	DISTINCT [SalesOrderID],[SalesOrderDetailID],[CarrierTrackingNumber]
		,[OrderQty], v1.[ProductID],[SpecialOfferID],[UnitPrice],[UnitPriceDiscount]
		,[LineTotal],v1.[ReferenceOrderID]
		,th.[Quantity]	
FROM vw_ViewLimit1 v1
INNER JOIN Production.TransactionHistory th ON v1.SalesOrderID = th.ReferenceOrderID
WHERE SalesOrderDetailID > 111111
--OPTION (MAXDOP 1) 
GO

DROP INDEX IX_RightNow ON [Sales].[SalesOrderDetail]
GO

-- Clean up
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_ViewLimit1]'))
DROP VIEW [dbo].[vw_ViewLimit1]
GO
