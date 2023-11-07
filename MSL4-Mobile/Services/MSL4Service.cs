using System;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class MSL4Service
{
	private HttpClient client;
    private JsonSerializerOptions serializerOptions;

    public List<PeriodDataResponse.PeriodData> PeriodData = new List<PeriodDataResponse.PeriodData>
    {
        new PeriodDataResponse.PeriodData{ id=10, pLabel="10 Sekunden" },
        new PeriodDataResponse.PeriodData{ id=60, pLabel="Minute" },
        new PeriodDataResponse.PeriodData{ id=300, pLabel="5 Minuten" },
        new PeriodDataResponse.PeriodData{ id=900, pLabel="15 Minuten" },
        new PeriodDataResponse.PeriodData{ id=3600, pLabel="Stunde" },
        new PeriodDataResponse.PeriodData{ id=86400, pLabel="Tag" }
    };

    public MSL4Service()
	{
        client = new HttpClient();
        serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public async Task<MSL4Data> SetMSL4Data(string ip, MSL4Data mSL4Data)
    {
        Uri uri = new Uri($"{AuthService.mslAddress}/LogWeb/servlet/MSL4DeviceData");
        MSL4Data mSL4DataResponse = null;
        try
        {
            string json = JsonSerializer.Serialize<MSL4Data>(mSL4Data, serializerOptions);
            Console.WriteLine(json.ToString());
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            Console.WriteLine(content.ToString());

            HttpResponseMessage response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                mSL4DataResponse = JsonSerializer.Deserialize<MSL4Data>(stringResponse);
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
        return mSL4DataResponse;
    }

    public async Task<MSL4Data> GetMSL4Data(string ip, string sessionid)
    {
        Uri uri = new Uri($"{AuthService.mslAddress}/LogWeb/servlet/MSL4DeviceData?pSessionID={sessionid}");
        MSL4Data mSL4Data = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                mSL4Data = JsonSerializer.Deserialize<MSL4Data>(stringResponse);
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
        return mSL4Data;
    }

    public async Task<PeriodDataResponse> GetMSL4PeriodData(string ip, string sessionid)
    {
        Uri uri = new Uri($"{AuthService.mslAddress}/LogWeb/servlet/SelectPeriodData?pSessionID={sessionid}");
        PeriodDataResponse periodDataResponse = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                periodDataResponse = JsonSerializer.Deserialize<PeriodDataResponse>(stringResponse);
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
        return periodDataResponse;
    }
}

public class MSL4Data
{
    public int pDataBits { get; set; }
    public bool pReadSecondary { get; set; }
    public string pPortName { get; set; }
    public string pDeviceSecAddress { get; set; }
    public int pDBDeviceID { get; set; }
    public int pPeriod { get; set; }
    public string pName { get; set; }
    public string pPostalCode { get; set; }
    public int pBaud { get; set; }
    public string pLogin { get; set; }
    public bool pActive { get; set; }
    public string pError { get; set; }
    public string pTimestamp { get; set; }
    public string pManufacturer { get; set; }
    public string pTCPAddress { get; set; }
    public int pPortType { get; set; }
    public string pStreet { get; set; }
    public int pLastSync { get; set; }
    public int pLoggedChannelCount { get; set; }
    public int pDeviceType { get; set; }
    public string pCity { get; set; }
    public int pParity { get; set; }
    public string pDBAction { get; set; }
    public string pDescription { get; set; }
    public int pDeviceAddress { get; set; }
    public string pPassword { get; set; }
    public string pSessionID { get; set; }
}

public class MSL4PostData
{
    public string pDBDeviceID { get; set; }
    public string pSessionID { get; set; }
    public string pDBAction { get; set; }
    public string pDeviceType { get; set; }
    public string pName { get; set; }
    public string pDescription { get; set; }
    public string pActive { get; set; }
    public string pPostalCode { get; set; }
    public string pPeriod { get; set; }
    public string pCity { get; set; }
    public string pManufacturer { get; set; }
    public string pStreet { get; set; }
}

public class PeriodDataResponse
{
    public string identifier { get; set; }
    public string label { get; set; }
    public List<PeriodData> items { get; set; }

    public class PeriodData
    {
        public int id { get; set; }
        public string pLabel { get; set; }
    }
}