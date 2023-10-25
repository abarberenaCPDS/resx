-- ------------------------------------------------------
-- Script 14: How to identify frequently running functions?
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------

/*
This query works for SQL Server 2016 onwards and provides details about the UDF 
execution counts and other statistics.
*/

SELECT TOP 50
DB_NAME(fs.database_id) DatabaseName,
OBJECT_NAME(object_id, database_id) FunctionName,
fs.execution_count,
fs.cached_time, fs.last_execution_time, fs.total_elapsed_time,
fs.total_worker_time, fs.total_logical_reads, fs.total_physical_reads,
fs.total_elapsed_time/fs.execution_count AS [avg_elapsed_time],
fs.last_elapsed_time
FROM sys.dm_exec_function_stats AS fs WITH (NOLOCK)
ORDER BY [avg_elapsed_time] DESC
OPTION (RECOMPILE)
GO