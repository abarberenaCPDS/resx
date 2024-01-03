# Multi-tenant

## Resources

### Promising

- [Pattern: Hybrid Tenancy](https://samnewman.io/patterns/deployment/hybrid-tenancy/)
- [Pattern: API Gateway / Backends for Frontends](https://microservices.io/patterns/apigateway.html)
- [Gateway Routing pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/gateway-routing)
- [Strategies for Using PostgreSQL as a Database for Multi-Tenant Services](https://dev.to/lbelkind/strategies-for-using-postgresql-as-a-database-for-multi-tenant-services-4abd)
- [Very Good -  Database design basics (Check the Denormalization Techniques)](https://support.microsoft.com/en-us/office/database-design-basics-eb2159cf-1e30-401a-8084-bd4f9c9ca1f5)
- [Data de-identification](https://www.youtube.com/watch?v=JLDpbXbT6wo&t=1s)
- [Advanced masking and de-identification](https://cloud.google.com/security/products/sensitive-data-protection?hl=en#advanced-masking-and-de-identification)
- [De-identify BigQuery data at query time](https://cloud.google.com/dlp/docs/deidentify-bq-tutorial)

### PostreSQL

- [Multi-Tenancy on PostgreSQL : An Introduction](https://opensource-db.com/multi-tenancy-on-postgres/)
- [Schema Flexibility and Data Sharing in Multi-Tenant Databases](https://mediatum.ub.tum.de/doc/1075044/10759044.pdf)
- [Strategies for Using PostgreSQL as a Database for Multi-Tenant Services](https://dev.to/lbelkind/strategies-for-using-postgresql-as-a-database-for-multi-tenant-services-4abd)
- [Multitenancy and Azure Database for PostgreSQL](https://learn.microsoft.com/en-us/azure/architecture/guide/multitenant/service/postgresql)
- [Row level security](https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/concepts-security#row--level-security)
- [Multi-tenancy in Postgres watch @ 23:45 for Considerations](https://www.youtube.com/watch?v=jTNeooqyTnc)
- [Cool for diagrams](https://opensource-db.com/multi-tenancy-on-postgres/)
- [COSMOS DB - Prepare tables for a multitenant data architecture w/ Exercices](https://learn.microsoft.com/en-us/training/modules/design-multi-tenant-saas-apps-with-azure-cosmos-db-for-postgresql/2-prep-tables-for-multi-tenant-data-architecture)

### MSSQL

- [Row-level security](https://learn.microsoft.com/en-us/sql/relational-databases/security/row-level-security?view=sql-server-ver16)
- [Multitenancy and Azure SQL Database](https://learn.microsoft.com/en-us/azure/architecture/guide/multitenant/service/sql-database)
- [Checklist for architecting and building multitenant solutions on Azure](https://learn.microsoft.com/en-us/azure/architecture/guide/multitenant/checklist)
- [Multi-tenant SaaS database tenancy patterns](https://learn.microsoft.com/en-us/azure/azure-sql/database/saas-tenancy-app-design-patterns?view=azuresql)

### Azure

- [Multitenant SaaS on Azure](https://learn.microsoft.com/en-us/azure/architecture/example-scenario/multi-saas/multitenant-saas)
- [Architectural approaches for storage and data in multitenant solutions](https://learn.microsoft.com/en-us/azure/architecture/guide/multitenant/approaches/storage-data)
- [Video indexed and annotated for multi-tenant SaaS app using Azure SQL Database](https://learn.microsoft.com/en-us/azure/azure-sql/database/saas-tenancy-video-index-wingtip-brk3120-20171011?view=azuresql)
- [Boring YouTube - Design patterns for SaaS applications on Azure SQL Database](https://www.youtube.com/watch?v=jjNmcKBVjrc)
- [Sharding pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/sharding)

### GCP

- [REVIEW Content de-identification](https://cloud.google.com/security/products/sensitive-data-protection?hl=en)
- [Enforce access control](https://cloud.google.com/bigquery/docs/column-level-security#api)

### AWS

- [Good reference on PostgreSQL pool model](https://docs.aws.amazon.com/prescriptive-guidance/latest/saas-multitenant-managed-postgresql/pool.html)
- [Multi-tenant data isolation with PostgreSQL Row Level Security](https://aws.amazon.com/blogs/database/multi-tenant-data-isolation-with-postgresql-row-level-security/)
- [Github - PostgreSQL RLS](https://github.com/aws-samples/aws-saas-factory-postgresql-rls)


### Academia
- [Modular models for systems based on multi-tenant services: A multi-level petri-net-based approach](https://pdf.sciencedirectassets.com/280416/1-s2.0-S1319157823X00093/1-s2.0-S1319157823002252/main.pdf?X-Amz-Security-Token=IQoJb3JpZ2luX2VjELz%2F%2F%2F%2F%2F%2F%2F%2F%2F%2FwEaCXVzLWVhc3QtMSJHMEUCIQCeHnjCyplJ0H8jKmm28rO547VeVGb4JekIC17hfxNAowIgeFvPUhpIATElHeqgt5W898EZaSjF87TZb%2F40zRutJOsqswUIVBAFGgwwNTkwMDM1NDY4NjUiDHz%2Fo29ugQPIa4gn3iqQBcO9drJr%2Bg%2BhpF5hslRnRrgljaUVjIFwg2sqfgcgNijyIlhzs%2BDbmpGrlPpYvjl2Www9TDF%2F4b4d3kXvyFcnyOBg4kmK8jDCyB%2BAvKtbeCYFC8WFOmsd80MlOfgymiMfDuYlXkYhwR43VL%2FVaxZHPAirYjRqbhqMJl%2FBtci6N6P9MUbskwUoeec2v4B105rLJS7E9uxGI7dK5nfQ3maO95e13htB64I9HHvzxvnLG6vMIdlmdwCBxw1BwHGXtRRHBES17PgByPaHxgclj5m7GUS6wO7cLXxExX6ClaCb1%2FPx4DBxPGxaqckVNTVXE6ZxT30R%2FEGQM1L2XqtxEF8PscS307XHcvo6J4t7aVQZZArHk0lFp3CZNolB3j%2BvgW78FcjZgVO%2BGz%2BQxZz0boNjSKILoAgSBxAgWyJMCqS2%2Fwv9MZ%2BwgdCLltV4zGGtEuUbNw0GZhz70HlZtr069SRTyTnoyttEQlIo4qKXRzU0pCtuqf4OAz9OjEtpX5LXz8umUReuVbhP7orUa%2B1t1LRWKRcg%2FUIuYgqRNR%2B9QkfdrOitJGWp7My4Qt6h8ilxvMMZdAz1ir337AMCEBu8FjSZG%2Bsd6se2enPaRxAwUX0BXf%2Fp%2BFZmj9j7l5kaD9DwCWkJ7znvC6lcX6%2FHDDNN89V%2BNW2cH7az5txrZOUbbCfORzeW5psakUvEKYd6wxSV4%2Bpw5ouZClFj7USw54Megf%2FUHgJR8gFHkwTFtqc%2FmHg5ZI4RFBZVAnXC6t3U4IEFGELOyA9F2Q0rfObq%2BqJb5TZYeQirMZlzmpLsNUJSjX5vereRLycJ5R11J4R9QEb%2FgLuENL6aGRI1zgxhcPlZBQvk9YW9xsl%2FDli8rQJM%2BJBny%2FUUMM%2Bj06wGOrEBKJrM4TltAPKTOmvyJennven7UE4%2FmBCg%2F2q%2BqItL4%2F8HSdt4cFet%2F2oFl7HYHRkLS5cGetV0slx8ujsQ4U%2B8S%2FAFcN2hWd%2FbiETlSa5oIQccj2%2FgVDO%2FEQu5td0eC3in%2FOiuTvtrbHES803hVZVHJyYMNO%2BK2oW7Q2xuMOka5c7i%2FiWwYvWPQEIKmCPDsQsZaMeERdQrUln98j36IEbB5dambtsiBvD9vPisZaFiChQv&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Date=20240103T043657Z&X-Amz-SignedHeaders=host&X-Amz-Expires=300&X-Amz-Credential=ASIAQ3PHCVTYZCBYZZUH%2F20240103%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Signature=a9c6b41e601e334c253d743a2672417db465ca9a78f8586e5447b48a2bbfe132&hash=2f7dd4bee1c19c442b02f5d7e63128c209d47d6c0e87a5852b0c224b18970b4e&host=68042c943591013ac2b2430a89b270f6af2c76d8dfd086a07176afe7c76c2c61&pii=S1319157823002252&tid=spdf-a1e903af-c9d3-4a13-acf9-ebb83f42492b&sid=711ed850423e9644d57be9f4a747985bdebdgxrqa&type=client&tsoh=d3d3LnNjaWVuY2VkaXJlY3QuY29t&ua=0f155d56055a5a02515555&rr=83f88b312bae2ec0&cc=us)

### Youtube

- [PostgreSql Row Level Security](https://www.youtube.com/watch?app=desktop&v=uivCzFwyUhc)
- [Implementing Row Level Security on SQL Server](https://www.youtube.com/watch?app=desktop&v=q0e_0lw23F0)
- [Multi-tenancy strategies with Django+PostgreSQL](https://www.youtube.com/watch?app=desktop&v=j-bbaf6hCMo)