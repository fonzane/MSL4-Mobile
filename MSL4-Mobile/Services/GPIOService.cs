using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MSL4_Mobile.Services;

public class GPIOService
{
    private HttpClient client;
    private JsonSerializerOptions serializerOptions;

    public readonly List<string> gpioTypes;

    public GPIOService()
    {
        gpioTypes = new List<string> { "Input", "Output" };
        client = new HttpClient();
        serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public async Task<DBDataType> GetVisualTypes(string ip, string sessionid)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/SelectDBVisualType?pSessionID={sessionid}");
        DBDataType visualTypes = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                visualTypes = JsonSerializer.Deserialize<DBDataType>(stringResponse);
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
        return visualTypes;
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
                Console.WriteLine("Got GPIO Data");
                Console.WriteLine(stringResponse);
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

    public async void SetGPIOData(string ip, string sessionid, string id, GPIO gpioData)
    {
        Uri uri = new Uri($"http://{ip}/RestMSL4GPIOTableData/{sessionid}/{id}");

        try
        {
            string json = JsonSerializer.Serialize<GPIO>(gpioData, serializerOptions);
            Console.WriteLine(json.ToString());
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            Console.WriteLine(content.ToString());

            HttpResponseMessage response = await client.PutAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response " + stringResponse);
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
    public string pAlertMessage { get; set; }
    public string pName { get; set; }
    public int pDBVisualTypeID { get; set; }
    public int pChannelAddress { get; set; }
    public string pDigitalModus { get; set; }
    public bool pActive { get; set; }
    public int pJobPeriod { get; set; }
    public int id { get; set; }
    public string pDescription { get; set; }
    [JsonIgnore]
    public KeyValuePair<int, string> initialStartType { get; set; }
    [JsonIgnore]
    public string initialVisualType { get; set; }
}

public record DBDataType
{
    public List<int> values { get; set; }
    public List<string> options { get; set; }
}
