## YAddress SQL Server UDF for Postal Address Correction, Validation, Standardization and Geocoding

YAddress SQL Server UDF (User Defined Function) calls YAddress Web API (http://www.yaddress.net)
to correct, validate, standardize and geocode postal addresses.
The UDF can be called from T-SQL queries and stored procedures or incorporated into ETL packages in SSIS. 

# Setup

1. Download YAddress UDF Binaries at http://www.yaddress.net/Downloads/YAddressSqlServerUdfBinaries.zip
2. Follow installation steps in "SQL Setup Script.sql". 


# Usage

### ProcessAddress

ProcessAddress is a table-valued UDF. Sample values returned by YAddress: 

```sql
SELECT * FROM ProcessAddress('506 Fourth Avenue Unit 1', 'Asbury Prk, NJ', NULL) 
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
SalesTaxRate: 6.625

### SQL Spatial Geography

ProcessAddress returns address location as two floating point values of latitude and longitude. To convert them to SQL Server spatial type Geography: 

```sql
SELECT Location = GEOGRAPHY::Point(Latitude, Longitude, 4326)
FROM ProcessAddress('506 Fourth Avenue Unit 1', 'Asbury Prk, NJ', NULL) 
```

