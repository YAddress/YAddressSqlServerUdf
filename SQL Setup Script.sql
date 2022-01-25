-- Enable CLR integration
EXEC sp_configure 'clr enabled', 1
RECONFIGURE
GO

-- Set proper permissions on the database
ALTER DATABASE [Database Name] 
SET TRUSTWORTHY ON
GO

-- Create assemply from the DLL
-- Replace the path as appropriate
CREATE ASSEMBLY YAddressSqlFunction
FROM 'C:\temp\YAddressSqlFunction.dll' 
WITH PERMISSION_SET = UNSAFE;
GO

-- Create SetBaseUrl User Defined Function
CREATE PROCEDURE SetBaseUrl(@BaseUrl nvarchar(255))
AS
EXTERNAL NAME YAddressSqlFunction.YAddressSqlFunction.SetBaseUrl
GO

-- Create ProcessAddress User Defined Function
CREATE FUNCTION ProcessAddress(@AddressLine1 nvarchar(255), @AddressLine2 nvarchar(255),
								@UserKey nvarchar(255))
RETURNS TABLE 
(
        ErrorCode int,
        ErrorMessage nvarchar(255),
        AddressLine1 nvarchar(255),
        AddressLine2 nvarchar(255),
        Number nvarchar(255),
        PreDir nvarchar(255),
        Street nvarchar(255),
        Suffix nvarchar(255),
        PostDir nvarchar(255),
        Sec nvarchar(255),
        SecNumber nvarchar(255),
        City nvarchar(255),
        State nvarchar(255),
        Zip nvarchar(255),
        Zip4 nvarchar(255),
        County nvarchar(255),
        StateFP nvarchar(255),
        CountyFP nvarchar(255),
        CensusTract nvarchar(255),
        CensusBlock nvarchar(255),
        Latitude float,
        Longitude float,
        GeocodePrecision int,
        TimeZoneOffset int,
        DstObserved bit,
        SalesTaxRate smallmoney,
        SalesTaxJurisdiction int
)
AS 
EXTERNAL NAME YAddressSqlFunction.YAddressSqlFunction.InitMethod
GO

-- Test the function
SELECT * FROM ProcessAddress('506 Fourth Avenue Unit 1', 'Asbury Prk, NJ', NULL)
