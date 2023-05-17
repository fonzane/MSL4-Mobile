using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class ModbusService
{
	private HttpClient client;
	private JsonSerializerOptions serializerOptions;

	public ModbusService()
	{
		client = new HttpClient();
        serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public async Task<ModbusResponse> GetModbusDevices(string ip, string sessionid)
    {

        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/DBDeviceTableData?pSessionID={sessionid}&pDeviceType=2");
        ModbusResponse modbusResponse = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                modbusResponse = JsonSerializer.Deserialize<ModbusResponse>(stringResponse);
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
        return modbusResponse;
    }

    public async Task<ModbusDeviceDetails> GetModbusDeviceDetails(string ip, string sessionid, int deviceID)
    {
        Uri uri = new Uri($"http://{ip}/LogWeb/servlet/DBDeviceData?pSessionID={sessionid}&pDeviceType=2&pDBAction=TYPE_READ&pDBDeviceID={deviceID}");
        ModbusDeviceDetails deviceDetails = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                deviceDetails = JsonSerializer.Deserialize<ModbusDeviceDetails>(stringResponse);
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
        return deviceDetails;
    }
}

public class ModbusResponse
{
    public string identifier { get; set; }
    public int numRows { get; set; }
    public string label { get; set; }
    public List<ModbusDevice> items { get; set; }
}

public class ModbusDevice
{
    public int pPosition { get; set; }
    public string pStreet { get; set; }
    public string pPortName { get; set; }
    public int pLoggedChannelCount { get; set; }
    public int pDeviceType { get; set; }
    public string pCity { get; set; }
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

public class ModbusDeviceDetails
{
    public int pDeviceID { get; set; }
    public int pStopBits { get; set; }
    public int pDataBits { get; set; }
    public bool pReadSecondary { get; set; }
    public string pPortName { get; set; }
    public string pDeviceSecAddress { get; set; }
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
    public int pLastSync { get; set; }
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