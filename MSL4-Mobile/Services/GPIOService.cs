using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class GPIOService
{
    private static HttpClient client = new HttpClient();
    private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}


public class GPIO
{
    public int pDBChannelNr { get; set; }
    public string pValueText { get; set; }
    public int pStartType { get; set; }
    public string pUnit { get; set; }
    public bool pValue { get; set; }
    public string pLimesMax { get; set; }
    public string pLimesMin { get; set; }
    public string pGPIOType { get; set; }
    public string pName { get; set; }
    public int pDBVisualTypeID { get; set; }
    public int pChannelAddress { get; set; }
    public string pDigitalModus { get; set; }
    public bool pActive { get; set; }
    public int pJobPeriod { get; set; }
    public int id { get; set; }
    public string pDescription { get; set; }
}

