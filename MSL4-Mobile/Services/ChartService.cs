using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class ChartService
{
	private HttpClient client;
	private JsonSerializerOptions serializerOptions;

	public ChartService()
	{
		client = AuthService.client;
		serializerOptions = new JsonSerializerOptions
		{
			Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};
	}

	public async Task<DBChannelResponse> GetDBChannelData()
	{
		Uri uri = new Uri($"{AuthService.mslAddress}/LogWeb/servlet/SelectDBChannel?pSessionID={AuthService.sessionid}&pDBDeviceID={AuthService.dbDeviceID}");
		DBChannelResponse dBChannelResponse = null;

		try
		{
			HttpResponseMessage response = await client.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				string stringResponse = await response.Content.ReadAsStringAsync();
				dBChannelResponse = JsonSerializer.Deserialize<DBChannelResponse>(stringResponse);
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

		return dBChannelResponse;
	}
}

public class DBChannelResponse
{
	public string identifier { get; set; }
	public string label { get; set; }
	public List<Channel> items { get; set; }
}

public record Channel
{
	public int id { get; set; }
	public string pLabel { get; set; }
}

