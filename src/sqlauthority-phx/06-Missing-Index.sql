-- ------------------------------------------------------
-- Script 04: How to identified MISSING indexes in SQL Server?
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------

/*
In this example we will learn about the script which helps to 
identify index which may potentially help to improve performance.
The script is commonly known as missing index script.
*/

USE tempdb
GO
-- Create Table NoIndex
CREATE TABLE MissingIndex (ID INT, ID2 INT, 
						FirstName VARCHAR(100), 
						LastName VARCHAR(100), 
						City VARCHAR(100))
GO
-- Insert one Million Records
INSERT INTO MissingIndex (ID, ID2, FirstName,LastName,City)
SELECT TOP 1000000 ROW_NUMBER() OVER (ORDER BY a.name) RowID,
				   ROW_NUMBER() OVER (ORDER BY a.name)*2 AS ID2,  
					'Bob', 
					CASE WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%2 = 1 THEN 'Smith' 
					ELSE 'Brown' END,
					CASE WHEN ROW_NUMBER() OVER (ORDER BY a.name)%10 = 1 THEN 'New York' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 5 THEN 'San Marino' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 3 THEN 'Los Angeles' 
					ELSE 'Houston' END
FROM sys.all_objects a
CROSS JOIN sys.all_objects b
GO
/* Enable execution plan using CTRL + M
OR 
Menu >> Query >> Include Actual Execution Plan
*/

-- Selecting range from table
SELECT ID
FROM MissingIndex
WHERE ID BETWEEN 1 AND 10000
GO 5

-- Selecting range from table
SELECT FirstName
FROM MissingIndex
WHERE ID2 > 1000 
GO 5

-- Selecting range from table
SELECT ID, FirstName
FROM MissingIndex
WHERE City  = 'New York'
GO 5

-- DMV Missing Indexes
SELECT t.name AS 'TableName'
    , 'CREATE NONCLUSTERED INDEX IX_' + t.name + '_MISSING_' 
        + CAST(ddmid.index_handle AS VARCHAR(10))
        + ' ON ' + ddmid.STATEMENT 
        + ' (' + IsNull(ddmid.equality_columns,'') 
        + CASE WHEN ddmid.equality_columns IS NOT NULL 
            AND ddmid.inequality_columns IS NOT NULL THEN ',' 
                ELSE '' END 
        + IsNull(ddmid.inequality_columns, '')
        + ')' 
        + IsNull(' INCLUDE (' + ddmid.included_columns + ');', ';'
        ) AS sql_statement
    , ddmigs.user_seeks
    , ddmigs.user_scans
    , CAST((ddmigs.user_seeks + ddmigs.user_scans) 
        * ddmigs.avg_user_impact AS INT) AS 'EstimatedImpact'
    , ddmigs.last_user_seek
FROM sys.dm_db_missing_index_groups AS ddmig
INNER JOIN sys.dm_db_missing_index_group_stats AS ddmigs
    ON ddmigs.group_handle = ddmig.index_group_handle
INNER JOIN sys.dm_db_missing_index_details AS ddmid 
    ON ddmig.index_handle = ddmid.index_handle
INNER JOIN sys.tables AS t
    ON ddmid.OBJECT_ID = t.OBJECT_ID
WHERE ddmid.database_id = DB_ID()
    AND CAST((ddmigs.user_seeks + ddmigs.user_scans) 
        * ddmigs.avg_user_impact AS INT) > 100
ORDER BY EstimatedImpact DESC;
GO

-- Clean up Database
DROP TABLE MissingIndex
GO