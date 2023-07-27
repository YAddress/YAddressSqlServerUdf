using System;
using System.Collections;
using System.Data.SqlTypes;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable e = YAddressSqlFunction.InitMethod(
                "506 Fourth Avenue Unit 1", "Asbury Prk, NJ", null, null);

            int ErrorCode;
            string ErrorMessage;
            string AddressLine1;
            string AddressLine2;
            string Number;
            string PreDir;
            string Street;
            string Suffix;
            string PostDir;
            string Sec;
            string SecNumber;
            SqlBoolean SecValidated;
            string City;
            string State;
            string Zip;
            string Zip4;
            string County;
            string StateFP;
            string CountyFP;
            string CensusBlock;
            string CensusTract;
            SqlDouble Latitude;
            SqlDouble Longitude;
            SqlInt32 GeoPrecision;
            SqlInt32 TimeZoneOffset;
            SqlBoolean DstObserved;
            SqlInt32 PlaceFP;
            string CityMunicipality;
            SqlMoney SalesTaxRate;
            SqlInt32 SalesTaxJurisdiction;
            string UspsCarrierRoute;

            IEnumerator r = e.GetEnumerator();
            r.MoveNext();
            YAddressSqlFunction.FillRow(r.Current,
                out ErrorCode,
                out ErrorMessage,
                out AddressLine1,
                out AddressLine2,
                out Number,
                out PreDir,
                out Street,
                out Suffix,
                out PostDir,
                out Sec,
                out SecNumber,
                out SecValidated,
                out City,
                out State,
                out Zip,
                out Zip4,
                out County,
                out StateFP,
                out CountyFP,
                out CensusBlock,
                out CensusTract,
                out Latitude,
                out Longitude,
                out GeoPrecision,
                out TimeZoneOffset,
                out DstObserved,
                out PlaceFP,
                out CityMunicipality,
                out SalesTaxRate,
                out SalesTaxJurisdiction,
                out UspsCarrierRoute);
        }
    }
}
