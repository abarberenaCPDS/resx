-- ------------------------------------------------------
-- Script 13: What is the impact of functions on query performance? (Select clause)
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------
/*
Function and Slow Query Performance - SELECT
*/

Use [tempdb]
GO

CREATE FUNCTION dbo.ComputeNum(@i int)
RETURNS INT
WITH SCHEMABINDING
BEGIN
  RETURN @i 
END
GO

-- turn on graphical plan here CTRL+M
SET STATISTICS IO, TIME ON
GO

-- First without the UDF you can see a parallel scan is in place
SELECT SUM(S1.OrderQty + S2.OrderQty) 
FROM AdventureWorks2014.sales.SalesOrderDetail S1,  
AdventureWorks2014.sales.SalesOrderDetail S2
WHERE S1.productid = 997
GO

-- With UDF - Can take upto ? seconds
SELECT SUM(tempdb.dbo.ComputeNum(S1.OrderQty + S2.OrderQty))
FROM AdventureWorks2014.sales.SalesOrderDetail S1,  
AdventureWorks2014.sales.SalesOrderDetail S2
WHERE S1.productid = 997
GO


DROP FUNCTION dbo.ComputeNum
GO

-- UDF can make queries create Serial plans and queries slower









-- With UDF - Can take upto ? seconds
SELECT tempdb.dbo.ComputeNum(SUM(S1.OrderQty + S2.OrderQty))
FROM AdventureWorks2014.sales.SalesOrderDetail S1,  
AdventureWorks2014.sales.SalesOrderDetail S2
WHERE S1.productid = 997
GO