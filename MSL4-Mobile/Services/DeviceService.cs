using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class DeviceService
{
	private HttpClient client = AuthService.client;
    private JsonSerializerOptions serializerOptions;

    public DeviceService()
	{
        serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public async Task<List<Device>> GetDevices()
    {
        Uri uri = new Uri($"{AuthService.webmonitorURL}/device");
        List<Device> devices = new List<Device>();

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                devices = JsonSerializer.Deserialize<List<Device>>(stringResponse);
            }
            else
            {
                Console.WriteLine("HTTP Request Failure fetching devices: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);
        }
        List<Device> filteredDevices = devices.Where(d => !String.IsNullOrEmpty(d.hostname)).ToList();
        return filteredDevices;
    }
}

public class Device
{
	public string _id { get; set; }
	public bool hasEvents { get; set; }
	public bool hasData { get; set; }
	public string createdAt { get; set; }
	public string lastPingCheck { get; set; }
	public List<object> owner { get; set; }
	public bool hasOwnUrls { get; set; }
	public List<object> subdevices { get; set; }
	public bool hasGeolocation { get; set; }
	public string devicename { get; set; }
	public string uniqueid { get; set; }
	public string street { get; set; }
	public string postalcode { get; set; }
	public string city { get; set; }
	public bool isDevice { get; set; }
	public bool isOnline { get; set; }
	public Dictionary<string, string> customUrls { get; set; }
    public int __v { get; set; }
	public string lat { get; set; }
	public string lon { get; set; }
	public bool hasCustomTooltips { get; set; }
	public List<string> mapModes { get; set; }
	public bool isAlertDevice { get; set; }
	public string channelDataTimestamp { get; set; }
	//public List<object> events { get; set; }
	public bool offlineAlertSend { get; set; }
	public bool hasError { get; set; }
	public string type { get; set; }
    public string hostname { get; set; }
}