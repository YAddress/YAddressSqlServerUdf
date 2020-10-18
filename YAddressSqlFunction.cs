using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class YAddressSqlFunction
{
    // Init method of the User Defined Table Valued function
    [SqlFunction(FillRowMethodName = "FillRow")]
    public static IEnumerable InitMethod(string sAddressLine1, string sAddressLine2, string sUserKey)
    {
        // Call YAddress Web API
        string sRequest = string.Format(
            "http://www.yaddress.net/api/address?AddressLine1={0}&AddressLine2={1}&UserKey={2}",
            Uri.EscapeDataString(sAddressLine1 ?? ""),
            Uri.EscapeDataString(sAddressLine2 ?? ""),
            Uri.EscapeDataString(sUserKey ?? ""));
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(sRequest);
        req.Accept = "application/xml";
        req.UserAgent = "YAddressSqlServerUdf/1.2.0";
        WebResponse res = req.GetResponse();
        StreamReader sr = new StreamReader(res.GetResponseStream());
        string sRes = sr.ReadToEnd();
        sr.Close();

        // One member array as an instance of IEnnumerable
        string[] arr = { sRes };
        return arr;
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
        out SqlMoney SalesTaxRate,
        out string SalesTaxJurisdiction)
    {
        try
        {
            // Load XML doc
            XmlDocument doc = new XmlDocument();
            doc.LoadXml((string)obj);
            XmlNode nd = doc.FirstChild;

            // Parse values
            ErrorCode = nd.GetInt("ErrorCode");
            ErrorMessage = nd.GetString("ErrorMessage");
            AddressLine1 = nd.GetOptionalString("AddressLine1");
            AddressLine2 = nd.GetOptionalString("AddressLine2");
            Number = nd.GetOptionalString("Number");
            PreDir = nd.GetOptionalString("PreDir");
            Street = nd.GetOptionalString("Street");
            Suffix = nd.GetOptionalString("Suffix");
            PostDir = nd.GetOptionalString("PostDir");
            Sec = nd.GetOptionalString("Sec");
            SecNumber = nd.GetOptionalString("SecNumber");
            City = nd.GetOptionalString("City");
            State = nd.GetOptionalString("State");
            Zip = nd.GetOptionalString("Zip");
            Zip4 = nd.GetOptionalString("Zip4");
            County = nd.GetOptionalString("County");
            StateFP = nd.GetOptionalString("StateFP");
            CountyFP = nd.GetOptionalString("CountyFP");
            CensusBlock = nd.GetOptionalString("CensusBlock");
            CensusTract = nd.GetOptionalString("CensusTract");
            Latitude = nd.GetOptionalDouble("Latitude") ?? SqlDouble.Null;
            Longitude = nd.GetOptionalDouble("Longitude") ?? SqlDouble.Null;
            GeoPrecision = nd.GetOptionalInt("GeoPrecision") ?? SqlInt32.Null;
            TimeZoneOffset = nd.GetOptionalInt("TimeZoneOffset") ?? SqlInt32.Null;
            DstObserved = nd.GetOptionalBool("DstObserved") ?? SqlBoolean.Null;
            SalesTaxRate = nd.GetOptionalDecimal("SalesTaxRate") ?? SqlMoney.Null;
            SalesTaxJurisdiction = nd.GetOptionalString("SalesTaxJurisdiction");
        }
        catch(Exception ex)
        {
            throw new Exception($"Error parsing YAddress Web API response: {obj}", ex);
        }
    }
}

// Required in .net 2.0 to use extension methods
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class
         | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}

static class XmlNodeExtensions
{
    public static int GetInt(this XmlNode xml, string sName)
    {
        return int.Parse(xml.SelectSingleNode(sName).InnerText);
    }

    public static int? GetOptionalInt(this XmlNode xml, string sName)
    {
        if (int.TryParse(xml.SelectSingleNode(sName)?.InnerText, out int n))
            return n;
        return null;
    }

    public static string GetString(this XmlNode xml, string sName)
    {
        return xml.SelectSingleNode(sName).InnerText;
    }

    public static string GetOptionalString(this XmlNode xml, string sName)
    {
        return xml.SelectSingleNode(sName)?.InnerText;
    }

    public static double? GetOptionalDouble(this XmlNode xml, string sName)
    {
        if (double.TryParse(xml.SelectSingleNode(sName)?.InnerText, out double f))
            return f;
        return null;
    }

    public static decimal? GetOptionalDecimal(this XmlNode xml, string sName)
    {
        if (decimal.TryParse(xml.SelectSingleNode(sName)?.InnerText, out decimal f))
            return f;
        return null;
    }

    public static bool? GetOptionalBool(this XmlNode xml, string sName)
    {
        XmlNode nd = xml.SelectSingleNode(sName);
        if (bool.TryParse(nd?.InnerText, out bool b))
            return b;
        if (int.TryParse(nd?.InnerText, out int n))
            return (n != 0);
        return null;
    }

}