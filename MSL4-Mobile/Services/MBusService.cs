using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class MBusService
{
    private HttpClient client;
    private JsonSerializerOptions serializerOptions;

    public List<DataStruct> PeriodData { get; }

    public MBusService()
    {
        client = new HttpClient();
        serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        PeriodData = new List<DataStruct>()
        {
            new DataStruct{ id = 60, pLabel = "Minute" },
            new DataStruct{ id = 300, pLabel = "5 Minuten" },
            new DataStruct{ id = 900, pLabel = "15 Minuten" },
            new DataStruct{ id = 3600, pLabel = "Stunde" },
            new DataStruct{ id = 86400, pLabel = "Tag" },
        };
    }

    public async Task<MBusDeviceDetails> GetMBusDeviceDetails(string ip, string sessionid, int deviceID)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/DBDeviceData?pSessionID={sessionid}&pDBAction=TYPE_READ&pDeviceType=1&pDBDeviceID={deviceID}");
        MBusDeviceDetails mBusDeviceDetails = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                mBusDeviceDetails = JsonSerializer.Deserialize<MBusDeviceDetails>(stringResponse);
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
        return mBusDeviceDetails;
    }

    public async Task<List<MBusChannelData>> GetMBusChannelData(string ip, string sessionid, int pDBDeviceID)
    {
        Uri uri = new Uri($"http://{ip}/RestMBus/{sessionid}/?pDBDeviceID={pDBDeviceID}");
        List<MBusChannelData> mBusChannelData = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                mBusChannelData = JsonSerializer.Deserialize<List<MBusChannelData>>(stringResponse);
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
        return mBusChannelData;
    }

    public async void SetMBusChannelData(string ip, string sessionid, int pDBDeviceID, MBusChannelData channelData)
    {
        Uri uri = new Uri($"http://{ip}/RestMBus/{sessionid}/?pDBDeviceID={pDBDeviceID}/{channelData.id}");

        try
        {
            string json = JsonSerializer.Serialize<MBusChannelData>(channelData, serializerOptions);
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

    public async Task<MBusDeviceDetails> SetMBusDeviceDetails(string ip, string sessionid, MBusDeviceDetails deviceDetails)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/DBDeviceData");
        MBusDeviceDetails deviceDetailsResponse = null;
        try
        {
            string json = JsonSerializer.Serialize<MBusDeviceDetails>(deviceDetails, serializerOptions);
            Console.WriteLine(json.ToString());
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            Console.WriteLine(content.ToString());

            HttpResponseMessage response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                deviceDetailsResponse = JsonSerializer.Deserialize<MBusDeviceDetails>(stringResponse);
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
        return deviceDetailsResponse;
    }

    public async Task<MBusDevicesResponse> GetMBusDevices(string ip, string sessionid)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/DBDeviceTableData?pSessionID={sessionid}&pDeviceType=1");
        MBusDevicesResponse getMBusDevicesResponse = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                getMBusDevicesResponse = JsonSerializer.Deserialize<MBusDevicesResponse>(stringResponse);
            } else
            {
                Console.WriteLine("HTTP Request Failure: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);
        }
        return getMBusDevicesResponse;
    }

    public async Task<ComDataResponse> GetComData(string ip, string sessionid)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/SelectComData?pSessionID={sessionid}&pType=1");
        ComDataResponse comData = null;
        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                comData = JsonSerializer.Deserialize<ComDataResponse>(stringResponse);
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
        return comData;
    }

    public async Task<BaudDataResponse> GetBaudData(string ip, string sessionid)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/SelectBaudData?pSessionID={sessionid}");
        BaudDataResponse baudData = null;
        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                baudData = JsonSerializer.Deserialize<BaudDataResponse>(stringResponse);
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
        return baudData;
    }

    public async Task<MBusIndexDataResponse> GetMBusIndexData(string ip, string sessionid, int deviceID)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/SelectMBusIndex?pSessionID={sessionid}&pDBDeviceID={deviceID}");
        MBusIndexDataResponse indexData = null;
        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                indexData = JsonSerializer.Deserialize<MBusIndexDataResponse>(stringResponse);
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
        return indexData;
    }

    

	public async Task<MBusSearchResponse> InitializeMBusSearch(
		string ip,
		string sessionid,
		string interfaceName,
		string baudRate,
		string initialTimeStamp)
	{
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/CallServer?pSessionID={sessionid}&pMethod=SearchMBus" +
			$"&pDBDeviceID=0&pDeviceType=1&pPortName={interfaceName}&pBaud={baudRate}&pParity=2&pDataBits=8&pStopBits=1" +
			$"&pTCPADDress=&pPortType=1&pMyLogID={initialTimeStamp}&pDate={initialTimeStamp}");

		MBusSearchResponse searchResponse = null;
        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                searchResponse = JsonSerializer.Deserialize<MBusSearchResponse>(stringResponse);
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
        return searchResponse;
    }

    public async Task<MBusSearchResponse> CheckMBusSearchStatus(string ip, string sessionid, string initialTimeStamp)
    {
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/CallServer?pSessionID={sessionid}&pMethod=ReadLog&pMyLogID={initialTimeStamp}&pDate={timestamp}");
        MBusSearchResponse searchResponse = null;
        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                searchResponse = JsonSerializer.Deserialize<MBusSearchResponse>(stringResponse);
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
        return searchResponse;
    }

    public async Task<UnitData> GetUnitData(string ip, string sessionid)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/SelectUnit?pSessionID={sessionid}");
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

public record UnitData
{
    public List<string> values { get; init; }
    public List<string> options { get; init; }
}

public readonly struct DataStruct
{
    public int id { get; init; }
    public string pLabel { get; init; }
}

public class MBusIndexDataResponse
{
    public string identifier { get; set; }
    public string label { get; set; }
    public List<DataStruct> items { get; set; }
}

public class MBusSearchResponse
{
	public string LogText { get; set; }
	public int LogType { get; set; }
	public int LogId { get; set; }
}

public class BaudDataResponse
{
	public string identifier { get; set; }
	public string label { get; set; }
	public List<DataStruct> items { get; set; }
}

public class ComDataResponse
{
	public string identifier { get; set; }
	public string label { get; set; }
	public List<ComData> items { get; set; }

	public class ComData
	{
		public string id { get; set; }
		public string pLabel { get; set; }
	}
}

public class MBusDevicesResponse
{
	public string identifier { get; set; }
	public int numRows { get; set; }
	public string label { get; set; }
	public List<MBusDeviceData> items { get; set; }
}

public class MBusDeviceData
{
	public int pPosition { get; set; }
	public string pStreet { get; set; }
	public string pPortName { get; set; } // Does this work as this member is not always present (optional)?
	public int pLoggedChannelCount { get; set; }
	public int pDeviceType { get; set; }
	public string pCity { get; set; }
	public string pDeviceSecAddress { get; set; } //
	public int pPeriod { get; set; }
	public string pName { get; set; }
	public string pPostalCode { get; set; }
	public string pLogin { get; set; }
	public bool pActive { get; set; }
	public string pTimestamp { get; set; }
	public int id { get; set; }
	public string pMBusMedium { get; set; }
	public string pDeviceTypeName { get; set; }
	public string pManufacturer { get; set; }
	public string pTCPAddress { get; set; }
	public int pDeviceAddress { get; set; }
}

public class MBusDeviceDetails
{
    public int pStopBits { get; set; }
    public int pDataBits { get; set; }
    public bool pReadSecondary { get; set; }
    public string pPortName { get; set; }
    public string pDeviceSecAddress { get; set; }
    public int pDBDeviceID { get; set; }
    public int pPeriod { get; set; }
    public string pName { get; set; }
    public int pRetryCount { get; set; }
    public string pPostalCode { get; set; }
    public int pBaud { get; set; }
    public string pLogin { get; set; }
    public bool pActive { get; set; }
    public string pError { get; set; }
    public string pTimestamp { get; set; }
    public string pManufacturer { get; set; }
    public string pTCPAddress { get; set; }
    public int pPortType { get; set; }
    public bool pMBusDoublePrimary { get; set; }
    public bool pCheckStatus { get; set; }
    public int pPosition { get; set; }
    public int pModbusType { get; set; }
    public string pStreet { get; set; }
    public double pLastSync { get; set; }
    public int pLoggedChannelCount { get; set; }
    public int pDeviceType { get; set; }
    public string pCity { get; set; }
    public int pParity { get; set; }
    public string pDBAction { get; set; }
    public string pDescription { get; set; }
    public bool pReadSecFrame { get; set; }
    public string pMBusMedium { get; set; }
    public int pDeviceAddress { get; set; }
    public string pPassword { get; set; }
    public string pSessionID { get; set; }
}

public class MBusChannelData
{
    public int pDBChannelNr { get; set; }
    public string pDIF { get; set; }
    public string pUnit { get; set; }
    public int pModbusRegisterTCP { get; set; }
    public string pValue { get; set; }
    public double pFactor { get; set; }
    public string pVIFE { get; set; }
    public string pValueCalc { get; set; }
    public string pName { get; set; }
    public int pDBVisualTypeID { get; set; }
    public int pChannelAddress { get; set; }
    public string pVIF { get; set; }
    public bool pActive { get; set; }
    public bool pType { get; set; }
    public int id { get; set; }
    public string pDIFE { get; set; }
    public UnitData unitData { get; set; }
}