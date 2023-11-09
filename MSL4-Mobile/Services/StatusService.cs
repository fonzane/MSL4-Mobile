using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class StatusService
{
	private HttpClient client;
	private JsonSerializerOptions serializerOptions;

	public StatusService()
	{
        //client = new HttpClient();
        client = AuthService.client;
        serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public async Task<MSL4StatusData> GetMSL4Status(string ip, string sessionid)
    {
        Uri uri = new Uri($"{AuthService.mslAddress}/LogWeb/servlet/MSL4StatusData?pSessionID={sessionid}");
        MSL4StatusData statusData = null;

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                statusData = JsonSerializer.Deserialize<MSL4StatusData>(stringResponse);
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
        return statusData;
    }
}

public class MSL4StatusData
{
    public int dbeviceid { get; set; }
    public string memtotal { get; set; }
    public string spacetotal { get; set; }
    public bool pModemActive { get; set; }
    public string range { get; set; }
    public string lat { get; set; }
    public string lon { get; set; }
    public string swaptotal { get; set; }
    public string zertifikat { get; set; }
    public string cellid { get; set; }
    public string error { get; set; }
    public string areacode { get; set; }
    public string mac { get; set; }
    public string percentcpu { get; set; }
    public bool tuerkontakt { get; set; }
    public string empfang { get; set; }
    public string mobilnumber { get; set; }
    public string modemaccesstechnology { get; set; }
    public string wlanip { get; set; }
    public bool stromausfall { get; set; }
    public string modemerrortext { get; set; }
    public string lanip { get; set; }
    public string spacefree { get; set; }
    public string vpnip { get; set; }
    public string modemip { get; set; }
    public string memavailable { get; set; }
    public string swapfree { get; set; }
    public string volt { get; set; }
    public string name { get; set; }
    public string percentmem { get; set; }
    public string temperatur { get; set; }
}