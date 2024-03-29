## YAddress SQL Server UDF for Postal Address Correction, Validation, Standardization and Geocoding

YAddress SQL Server UDF (User Defined Function) calls YAddress Web API (http://www.yaddress.net)
to correct, validate, standardize and geocode postal addresses.
The UDF can be called from T-SQL queries and stored procedures or incorporated into ETL packages in SSIS. 

# Setup

1. Download YAddress UDF Binaries at https://github.com/YAddress/YAddressSqlServerUdf/releases/latest/download/YAddressSqlServerUdfBinaries.zip
2. Follow installation steps in "SQL Setup Script.sql". 


# Usage

### ProcessAddress

ProcessAddress is a table-valued UDF (User Defined Function). It returns a single row of data
with address fields in its columns.

```sql
ProcessAddress(@AddressLine1 nvarchar(255), @AddressLine2 nvarchar(255),
				@UserKey nvarchar(255), @BaseUrl nvarchar(1024))
```
**AddressLine1:** street address line.

**AddressLine2:** city, state, zip.

**UserKey:** YAddress Web API user key. Set to NULL if you do not have a YAddress account.

**BaseUrl:** Base URL for API calls. Set to NULL to use the standard base URL.

### Example
```sql
SELECT * FROM ProcessAddress('506 Fourth Avenue Unit 1', 'Asbury Prk, NJ', NULL, NULL) 
```
#### Results:
ErrorCode: 0<br>
ErrorMessage:<br>
AddressLine1: 506 4TH AVE APT 1<br>
AddressLine2: ASBURY PARK, NJ 07712-6086<br>
Number: 506<br>
PreDir:<br>
Street: 4TH<br>
Suffix: AVE<br>
PostDir:<br>
Sec: APT<br>
SecNumber: 1<br>
City: ASBURY PARK<br>
State: NJ<br>
Zip: 7712<br>
Zip4: 6086<br>
County: MONMOUTH<br>
CountyFP: 25<br>
CensusTract: 1015<br>
CensusBlock: 8070.03<br>
Latitude: 40.223571<br>
Longitude: -74.005973<br>
GeocodePrecision: 5 <br>
TimeZoneOffset: -5<br>
DstObserved: 1<br>
SalesTaxRate: 6.625<br>
SalesTaxJurisdiction: State of NJ

### SQL Spatial Geography

ProcessAddress returns address location as two floating point values of latitude and longitude. To convert them to SQL Server spatial type Geography: 

```sql
SELECT Location = GEOGRAPHY::Point(Latitude, Longitude, 4326)
FROM ProcessAddress('506 Fourth Avenue Unit 1', 'Asbury Prk, NJ', NULL, NULL) 
```

