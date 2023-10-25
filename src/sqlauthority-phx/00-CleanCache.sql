-- ------------------------------------------------------
-- Script 00: How to Clear the Cache?
-- (c) https://blog.sqlauthority.com
-- Subscribe to newsletter at https://go.sqlauthority.com 
-- Questions: pinal@sqlauthority.com 
-- ------------------------------------------------------
-- Older Method
DBCC FREEPROCCACHE
GO
-- Newer Method
ALTER DATABASE SCOPED CONFIGURATION CLEAR PROCEDURE_CACHE
GO