USE SQLAuthority
GO
-- Enable Execution Plan CTRL +M
SET NOCOUNT ON;
SET STATISTICS IO, TIME ON;
DBCC FREEPROCCACHE;
DBCC DROPCLEANBUFFERS;
GO 

-- We have a single table and  Index on it
SELECT OrderNumber, OrderDate, DeliveryDate, OtherColumns,
       DENSE_RANK() 
            OVER ( PARTITION BY p.OrderDate
                  ORDER BY p.DeliveryDate DESC 
        	) AS DenseRank
FROM   dbo.Orders AS p
WHERE  OrderNumber = 530843
GO
/*
Table 'Orders'. Scan count 0, logical reads 3
CPU time = 0 ms,  elapsed time = 45 ms.
*/

-- Create a View
CREATE OR ALTER VIEW dbo.ViewofQuery
WITH SCHEMABINDING
AS
    SELECT OrderNumber, OrderDate, DeliveryDate, OtherColumns,
    DENSE_RANK() 
        OVER ( PARTITION BY p.OrderDate
                ORDER BY p.DeliveryDate DESC 
        ) AS DenseRank
	FROM   dbo.Orders AS p;
GO 
-- Create SP on the Query
CREATE OR ALTER PROCEDURE dbo.SPofQuery (@OrderNumber INT)
AS 
SET NOCOUNT, XACT_ABORT ON;
BEGIN
    SELECT OrderNumber, OrderDate, DeliveryDate, OtherColumns,
    DENSE_RANK() 
        OVER ( PARTITION BY p.OrderDate
                ORDER BY p.DeliveryDate DESC 
        ) AS DenseRank
	FROM   dbo.Orders AS p
	WHERE OrderNumber = @OrderNumber
END;
GO 


-- Run the View
SELECT *
FROM dbo.ViewofQuery
WHERE  OrderNumber = 530843
GO


-- Execute SP
EXEC dbo.SPofQuery @OrderNumber = 530843;
GO

