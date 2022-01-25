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
                "506 Fourth Avenue Unit 1", "Asbury Prk, NJ", null);


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
            SqlMoney SalesTaxRate;
            SqlInt32 SalesTaxJurisdiction;

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
                out SalesTaxRate,
                out SalesTaxJurisdiction);
        }
    }
}
