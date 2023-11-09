using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MSL4_Mobile.Services;

public class MeterService
{
	private HttpClient client;
	private JsonSerializerOptions serializerOptions;


	public MeterService()
	{
		client = AuthService.client;
        serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public async Task<List<Meter>> GetMeterData(string ip, string sessionid)
    {
        Uri uri = new Uri($"{AuthService.mslAddress}/RestMeter/{sessionid}/");
        List<Meter> meterData = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Got Meter Data");
                Console.WriteLine(stringResponse);
                meterData = JsonSerializer.Deserialize<List<Meter>>(stringResponse);
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
        return meterData;
    }

    public async void SetMeterData(string ip, string sessionid, string id, Meter meterData)
    {
        Uri uri = new Uri($"{AuthService.mslAddress}/RestMeter/{sessionid}/{id}");

        try
        {
            string json = JsonSerializer.Serialize<Meter>(meterData, serializerOptions);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

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

    public async Task<UnitData> GetUnitData(string ip, string sessionid)
    {
        Uri uri = new Uri($"{AuthService.mslAddress}/LogWeb/servlet/SelectUnit?pSessionID={sessionid}");
        UnitData unitResponse = null;
        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                unitResponse = JsonSerializer.Deserialize<UnitData>(stringResponse);
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
        return unitResponse;
    }
}

public class Meter
{
    public int pDBChannelNr { get; set; }
    public string pUnit { get; set; }
    public float pValue { get; set; }
    public string pLimesMax { get; set; }
    public float pFactor { get; set; }
    public float pLimesDelayIn { get; set; }
    public string pValueCalc { get; set; }
    public string pAlertMessage { get; set; }
    public string pName { get; set; }
    public int pDBVisualTypeID { get; set; }
    public int pChannelAddress { get; set; }
    public bool pActive { get; set; }
    public string pLimesMin { get; set; }
    public int id { get; set; }
    public string pDescription { get; set; }
    public float pLimesDelayOut { get; set; }
    public UnitData unitData { get; set; }
}