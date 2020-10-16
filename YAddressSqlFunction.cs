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
        // Load XML doc
        XmlDocument doc = new XmlDocument();
        doc.LoadXml((string)obj);

        // Parse values
        ErrorCode = doc.GetInt("ErrorCode");
        ErrorMessage = doc.GetString("ErrorMessage");
        AddressLine1 = doc.GetOptionalString("AddressLine1");
        AddressLine2 = doc.GetOptionalString("AddressLine2");
        Number = doc.GetOptionalString("Number");
        PreDir = doc.GetOptionalString("PreDir");
        Street = doc.GetOptionalString("Street");
        Suffix = doc.GetOptionalString("Suffix");
        PostDir = doc.GetOptionalString("PostDir");
        Sec = doc.GetOptionalString("Sec");
        SecNumber = doc.GetOptionalString("SecNumber");
        City = doc.GetOptionalString("City");
        State = doc.GetOptionalString("State");
        Zip = doc.GetOptionalString("Zip");
        Zip4 = doc.GetOptionalString("Zip4");
        County = doc.GetOptionalString("County");
        StateFP = doc.GetOptionalString("StateFP");
        CountyFP = doc.GetOptionalString("CountyFP");
        CensusBlock = doc.GetOptionalString("CensusBlock");
        CensusTract = doc.GetOptionalString("CensusTract");
        Latitude = doc.GetOptionalDouble("Latitude") ?? SqlDouble.Null;
        Longitude = doc.GetOptionalDouble("Longitude") ?? SqlDouble.Null;
        GeoPrecision = doc.GetOptionalInt("GeoPrecision") ?? SqlInt32.Null;
        TimeZoneOffset = doc.GetOptionalInt("TimeZoneOffset") ?? SqlInt32.Null;
        DstObserved = doc.GetOptionalBool("DstObserved") ?? SqlBoolean.Null;
        SalesTaxRate = doc.GetOptionalDecimal("SalesTaxRate") ?? SqlMoney.Null;
        SalesTaxJurisdiction = doc.GetOptionalString("SalesTaxJurisdiction");
    }
}

// Required in .net 2.0 to use extension methods
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class
         | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}

static class XmlDocExtensions
{
    public static int GetInt(this XmlDocument doc, string sName)
    {
        // Find element
        XmlNode nd = doc.SelectSingleNode($"Address/{sName}");
        if (nd == null)
            throw new XmlException($"Node not found: {sName}");

        // Parse value
        int n;
        if (!int.TryParse(nd.InnerText, out n))
            throw new XmlException($"Node content is not int: {sName}");

        return n;
    }

    public static int? GetOptionalInt(this XmlDocument doc, string sName)
    {
        // Find element
        XmlNode nd = doc.SelectSingleNode($"Address/{sName}");
        if (nd == null)
            return null;

        // Parse value
        int n;
        if (!int.TryParse(nd.InnerText, out n))
            return null;

        return n;
    }

    public static string GetString(this XmlDocument doc, string sName)
    {
        // Find element
        XmlNode nd = doc.SelectSingleNode($"Address/{sName}");
        if (nd == null)
            throw new XmlException($"Node not found: {sName}");

        return nd.InnerText;
    }

    public static string GetOptionalString(this XmlDocument doc, string sName)
    {
        // Find element
        XmlNode nd = doc.SelectSingleNode($"Address/{sName}");
        if (nd == null)
            return null;

        return nd.InnerText;
    }

    public static double? GetOptionalDouble(this XmlDocument doc, string sName)
    {
        // Find element
        XmlNode nd = doc.SelectSingleNode($"Address/{sName}");
        if (nd == null)
            return null;

        // Parse value
        double f;
        if (!double.TryParse(nd.InnerText, out f))
            return null;

        return f;
    }

    public static decimal? GetOptionalDecimal(this XmlDocument doc, string sName)
    {
        // Find element
        XmlNode nd = doc.SelectSingleNode($"Address/{sName}");
        if (nd == null)
            return null;

        // Parse value
        decimal f;
        if (!decimal.TryParse(nd.InnerText, out f))
            return null;

        return f;
    }

    public static bool? GetOptionalBool(this XmlDocument doc, string sName)
    {
        // Find element
        XmlNode nd = doc.SelectSingleNode($"Address/{sName}");
        if (nd == null)
            return null;

        // Parse value
        bool b;
        if (!bool.TryParse(nd.InnerText, out b))
        {
            int n;
            if (!int.TryParse(nd.InnerText, out n))
                return null;
            return (n != 0);
        }

        return b;
    }

}