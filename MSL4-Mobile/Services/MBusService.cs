using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class MBusService
{
	private HttpClient client;
	private static JsonSerializerOptions serializerOptions;

    public MBusService()
    {
        client = new HttpClient();
        serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
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
	public List<BaudData> items { get; set; }

	public class BaudData
	{
		public int id { get; set; }
		public string pLabel { get; set; }
	}
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
}