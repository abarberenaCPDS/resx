-- ------------------------------------------------------
-- Script 17: How to overcome CPU bottlenecks - under utilization / over utlization?
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------

/*
We will run queries with multiple processors and derived optimal max degree of parallelism
*/
-- --------------------------------------------------------------------------
-- Test 1 
-- --------------------------------------------------------------------------
-- Clear Wait Types
DBCC SQLPERF('sys.dm_os_wait_stats', CLEAR);
GO

USE AdventureWorks2014
GO
SELECT *
FROM Sales.SalesOrderDetail
ORDER BY UnitPrice DESC
-- OPTION (MAXDOP 1) -- Single CPU Utilization at Query Level
GO 30

-- Run Wait Statistics Query
/* Results
	8 CPU - x Seconds - CXPACKET Wait
	4 CPU - x Seconds - CXPACKET Wait
	2 CPU - x Seconds - CXPACKET Wait
	1 CPU - x Seconds - CXPACKET Wait
*/

-- --------------------------------------------------------------------------
-- Test 2 
-- --------------------------------------------------------------------------
-- Clear Wait Types
DBCC SQLPERF('sys.dm_os_wait_stats', CLEAR);
GO

USE AdventureWorks2014
GO
SELECT soh.SalesOrderID,SUM(SubTotal) RowCnt     
FROM [Sales].[SalesOrderHeader] soh
INNER JOIN [Sales].[SalesPerson] sp 
		ON sp.BusinessEntityID = soh.SalesPersonID
INNER JOIN [Sales].[SalesOrderDetail] sod
		ON sod.SalesOrderID = soh.SalesOrderID
INNER JOIN [Sales].[SalesPersonQuotaHistory] spq
		ON spq.BusinessEntityID = soh.SalesPersonID
GROUP BY soh.SalesOrderID, UnitPrice
ORDER BY UnitPrice DESC
OPTION (MAXDOP 1) -- Single CPU Utilization at Query Level
GO 30

-- Run Wait Statistics Query
/* Results
	8 CPU - x Seconds - CXPACKET Wait
	4 CPU - x Seconds - CXPACKET Wait
	2 CPU - x Seconds - CXPACKET Wait
	1 CPU - x Seconds - CXPACKET Wait
*/

-- --------------------------------------------------------------------------
-- Solution 1: Change Max Degree of parallelism for database
-- --------------------------------------------------------------------------

EXEC sys.sp_configure 'show advanced options',1
RECONFIGURE
GO

EXEC sys.sp_configure N'max degree of parallelism', N'2'
GO
RECONFIGURE WITH OVERRIDE
GO
-- --------------------------------------------------------------------------
-- Solution 2: Change cost threshold for parallelism for server to impact expensive queries
-- --------------------------------------------------------------------------
EXEC sys.sp_configure 'show advanced options',1
RECONFIGURE
GO

-- Query related to cost threshold for parallelism
EXEC sys.sp_configure N'cost threshold for parallelism', N'50'
GO
RECONFIGURE WITH OVERRIDE
GO