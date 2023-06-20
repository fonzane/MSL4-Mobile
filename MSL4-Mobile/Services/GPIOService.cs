using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class GPIOService
{
    private HttpClient client;
    private JsonSerializerOptions serializerOptions;

    public GPIOService()
    {
        client = new HttpClient();
        serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public async Task<List<string>> GetDigitalModes(string ip, string sessionid)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/SelectDigitalModus?pSessionID={sessionid}");
        List<string> digitalModes = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                digitalModes = JsonSerializer.Deserialize<List<string>>(stringResponse);
            }
            else
            {
                Console.WriteLine("HTTP Request Failure: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
        return digitalModes;
    }

    public async Task<StartTypeResponse> GetStartTypes(string ip, string sessionid)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/SelectStartType?pSessionID={sessionid}");
        StartTypeResponse startTypes = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                startTypes = JsonSerializer.Deserialize<StartTypeResponse>(stringResponse);
            }
            else
            {
                Console.WriteLine("HTTP Request Failure: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);
        }
        return startTypes;
    }

    public async Task<List<GPIO>> GetGPIOData(string ip, string sessionid)
    {
        Uri uri = new Uri($"http://{ip}/RestMSL4GPIOTableData/{sessionid}/");
        List<GPIO> gPIOData = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                gPIOData = JsonSerializer.Deserialize<List<GPIO>>(stringResponse);
            }
            else
            {
                Console.WriteLine("HTTP Request Failure: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);
        }
        return gPIOData;

    }
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

public record StartTypeResponse
{
    public List<int> values;
    public List<string> options;
}
