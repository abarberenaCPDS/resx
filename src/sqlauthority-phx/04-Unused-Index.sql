-- ------------------------------------------------------
-- Script 03: How to identified UNUSED indexes in SQL Server?
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------

/*
In this example we will learn about the script which helps to 
identify index usages. 
The script is commonly known as unused index script.
*/

USE tempdb
GO
-- Create Table
CREATE TABLE UnusedIndex (ID INT, 
						FirstName VARCHAR(100), 
						LastName VARCHAR(100), 
						City VARCHAR(100))
GO

-- Create indexes
CREATE CLUSTERED INDEX [IX_UnusedIndex_ID] ON [dbo].[UnusedIndex] 
(
	[ID] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_UnusedIndex_FirstName] ON [dbo].[UnusedIndex] 
(
	[FirstName] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_UnusedIndex_LastName] ON [dbo].[UnusedIndex] 
(
	[LastName] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_UnusedIndex_City] ON [dbo].[UnusedIndex] 
(
	[City] ASC
) ON [PRIMARY]
GO

-- Insert One Hundred Thousand Records
-- INSERT 1
INSERT INTO UnusedIndex (ID,FirstName,LastName,City)
SELECT TOP 1000 ROW_NUMBER() OVER (ORDER BY a.name) RowID, 
					'Bob', 
					CASE WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%2 = 1 THEN 'Smith' 
					ELSE 'Brown' END,
					CASE WHEN ROW_NUMBER() OVER (ORDER BY a.name)%10 = 1 THEN 'New York' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 5 THEN 'San Marino' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 3 THEN 'Los Angeles' 
					ELSE 'Houston' END
FROM sys.all_objects a
CROSS JOIN sys.all_objects b
GO 7

INSERT INTO UnusedIndex (ID,FirstName,LastName,City)
SELECT TOP 1 ROW_NUMBER() OVER (ORDER BY a.name) RowID, 
					'Pinal', 
					CASE WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%2 = 1 THEN 'Dave' 
					ELSE 'Brown' END,
					CASE WHEN ROW_NUMBER() OVER (ORDER BY a.name)%10 = 1 THEN 'New York' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 5 THEN 'San Marino' 
						WHEN  ROW_NUMBER() OVER (ORDER BY a.name)%10 = 3 THEN 'Los Angeles' 
					ELSE 'Houston' END
FROM sys.all_objects a
CROSS JOIN sys.all_objects b
GO 

-- Selecting range from table
SELECT ID
FROM [dbo].[UnusedIndex]
WHERE ID BETWEEN 1 AND 10000
GO 2

-- Selecting range from table
SELECT FirstName
FROM [dbo].[UnusedIndex]
WHERE ID > 1000 
GO 5

-- Selecting range from table
SELECT ID, LastName
FROM [dbo].[UnusedIndex]
WHERE City  = 'New York'
GO 9

SELECT FirstName, LastName
FROM [dbo].[UnusedIndex]
WHERE LastName  = 'Dave'
GO 11

-- DMV Unused Indexes
DECLARE @dbid INT, @dbName VARCHAR(100);
SELECT @dbid = DB_ID(), @dbName = DB_NAME();
SELECT	  SCHEMA_NAME(o.schema_id) AS objectSchema
		, OBJECT_NAME(i.[OBJECT_ID]) AS objectName
        , i.name as IndexName
        , CASE 
            WHEN i.is_unique = 1 
                THEN 'UNIQUE ' 
            ELSE '' 
          END + i.type_desc AS 'IndexType'
        , ddius.user_seeks, ddius.user_scans
        , ddius.user_lookups, ddius.user_updates
        , rp.RowCounts
        , CASE 
            WHEN i.type = 2 And i.is_unique = 0
                THEN 'DROP INDEX ' + i.name 
                    + ' ON ' + @dbName 
                    + '.' + SCHEMA_NAME(o.schema_id) + '.'
					+ OBJECT_NAME(ddius.[OBJECT_ID]) + ';'
            WHEN i.type = 2 And i.is_unique = 1
                THEN 'ALTER TABLE ' + @dbName 
                    + '.' + SCHEMA_NAME(o.schema_id) + '.'
					+ OBJECT_NAME(ddius.[OBJECT_ID]) 
                    + ' DROP CONSTRAINT ' + i.name + ';'
            ELSE '' 
          END AS 'SQLDropStatement'
FROM sys.indexes AS i
INNER JOIN sys.dm_db_index_usage_stats ddius
    ON i.OBJECT_ID = ddius.OBJECT_ID
        AND i.index_id = ddius.index_id
INNER JOIN sys.objects o ON ddius.object_id = o.object_id
INNER JOIN ( SELECT [OBJECT_ID]
        , index_id
        , SUM([ROWS]) AS 'RowCounts'
        , COUNT(partition_id) AS 'PartitionCount'
    FROM sys.partitions
    GROUP BY [OBJECT_ID]
        , index_id) rp
    ON i.OBJECT_ID = rp.OBJECT_ID
        And i.index_id = rp.index_id
WHERE ddius.database_id = @dbid
AND i.is_unique = 0  -- remove this to see all the indexes
ORDER BY 
    (ddius.user_seeks + ddius.user_scans + ddius.user_lookups) DESC
    , user_updates DESC;
GO

-- Clean up Database
DROP TABLE UnusedIndex
GO