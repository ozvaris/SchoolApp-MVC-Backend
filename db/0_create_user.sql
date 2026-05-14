-- "DefaultConnection": "Server=127.0.0.1,1433;Database=SchoolAppDb;User Id=schoolapp_user;Password=YourStrongPassword!123;TrustServerCertificate=True;"
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'schoolapp_user')
BEGIN
    CREATE LOGIN [schoolapp_user]
    WITH PASSWORD = 'YourStrongPassword!123';
END;

USE [SchoolAppDb];

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'schoolapp_user')
BEGIN
    CREATE USER [schoolapp_user] FOR LOGIN [schoolapp_user];
END;

ALTER ROLE [db_datareader] ADD MEMBER [schoolapp_user];
ALTER ROLE [db_datawriter] ADD MEMBER [schoolapp_user];