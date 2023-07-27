using System;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Net;
using System.IO;

using SimpleJSON;

public class YAddressSqlFunction
{
    static readonly Version _ver =
        typeof(YAddressSqlFunction).Assembly.GetName().Version;
    static readonly string _sUserAgent =
        $"YAddressSqlServerUdf/{_ver.Major}.{_ver.Minor}.{_ver.Build}";

    // Init method of the User Defined Table Valued function
    [SqlFunction(FillRowMethodName = "FillRow")]
    public static IEnumerable InitMethod(string sAddressLine1, string sAddressLine2, 
        string sUserKey, string sBaseUrl)
    {
        // Call YAddress Web API
        string sUrl = string.Format(
            "{0}/Address?AddressLine1={1}&AddressLine2={2}&UserKey={3}",
            sBaseUrl ?? "http://www.yaddress.net/api",
            Uri.EscapeDataString(sAddressLine1 ?? ""),
            Uri.EscapeDataString(sAddressLine2 ?? ""),
            Uri.EscapeDataString(sUserKey ?? ""));
        string sRes;
        try
        {
            WebClient client = new WebClient();
            client.Headers["Accept"] = "application/json";
            client.Headers["User-Agent"] = _sUserAgent;
            sRes = client.DownloadString(sUrl);
        }
        catch (Exception ex)
        {
            throw new Exception("Yaddress Web API call failed.", ex);
        }

        // Return one result as ennumerable
        yield return sRes;
    }

    // FillRow method of the User Defined Table Valued function
    public static void FillRow(
        Object obj,
        out int ErrorCode,
        out string ErrorMessage,
        out string AddressLine1,
        out string AddressLine2,
        out string Number,
        out string PreDir,
        out string Street,
        out string Suffix,
        out string PostDir,
        out string Sec,
        out string SecNumber,
        out SqlBoolean SecValidated,
        out string City,
        out string State,
        out string Zip,
        out string Zip4,
        out string County,
        out string StateFP,
        out string CountyFP,
        out string CensusBlock,
        out string CensusTract,
        out SqlDouble Latitude,
        out SqlDouble Longitude,
        out SqlInt32 GeoPrecision,
        out SqlInt32 TimeZoneOffset,
        out SqlBoolean DstObserved,
        out SqlInt32 PlaceFP,
        out string CityMunicipality,
        out SqlMoney SalesTaxRate,
        out SqlInt32 SalesTaxJurisdiction,
        out string UspsCarrierRoute)
    {
        try
        {
            // Parse JSON
            JSONNode nd = JSON.Parse((string)obj);

            // Update values
            ErrorCode = nd["ErrorCode"].AsInt;
            ErrorMessage = nd["ErrorMessage"].AsString;
            AddressLine1 = nd["AddressLine1"].AsString;
            AddressLine2 = nd["AddressLine2"].AsString;
            Number = nd["Number"].AsString;
            PreDir = nd["PreDir"].AsString;
            Street = nd["Street"].AsString;
            Suffix = nd["Suffix"].AsString;
            PostDir = nd["PostDir"].AsString;
            Sec = nd["Sec"].AsString;
            SecNumber = nd["SecNumber"].AsString;
            SecValidated = nd["SecValidated"].AsNullableBool ?? SqlBoolean.Null;
            City = nd["City"].AsString;
            State = nd["State"].AsString;
            Zip = nd["Zip"].AsString;
            Zip4 = nd["Zip4"].AsString;
            County = nd["County"].AsString;
            StateFP = nd["StateFP"].AsString;
            CountyFP = nd["CountyFP"].AsString;
            CensusBlock = nd["CensusBlock"].AsString;
            CensusTract = nd["CensusTract"].AsString;
            Latitude = nd["Latitude"].AsNullableDouble ?? SqlDouble.Null;
            Longitude = nd["Longitude"].AsNullableDouble ?? SqlDouble.Null;
            GeoPrecision = nd["GeoPrecision"].AsInt;
            TimeZoneOffset = nd["TimeZoneOffset"].AsNullableInt ?? SqlInt32.Null;
            DstObserved = nd["DstObserved"].AsNullableBool ?? SqlBoolean.Null;
            PlaceFP = nd["PlaceFP"].AsNullableInt ?? SqlInt32.Null;
            CityMunicipality = nd["CityMunicipality"].AsString;
            SalesTaxRate = nd["SalesTaxRate"].AsNullableDecimal ?? SqlMoney.Null;
            SalesTaxJurisdiction = nd["SalesTaxJurisdiction"].AsNullableInt ?? SqlInt32.Null;
            UspsCarrierRoute = nd["UspsCarrierRoute"].AsString;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error parsing YAddress Web API response: {obj}", ex);
        }
    }
}
