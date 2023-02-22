using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class AuthService
{
	private static HttpClient client = new HttpClient();
	public static string sessionid;
	public static string ipaddress;

	public static async Task<string> GetSessionID(string ip)
	{
		ipaddress = ip;
		Uri uri = new Uri($"http://{ip}/LogWeb/servlet/FOLogin?pUser=administrator&pPassword=admin");
		AuthResponse authResponse = new AuthResponse();

		try
		{
			HttpResponseMessage response = await client.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				string stringResponse = await response.Content.ReadAsStringAsync();
				authResponse = JsonSerializer.Deserialize<AuthResponse>(stringResponse);
				sessionid = authResponse.sessionid.ToString();
			}
		} catch (Exception ex)
		{
			Console.WriteLine(@"\tERROR {0}", ex.Message);
		}
		return authResponse.sessionid.ToString();
    }
}

public class AuthResponse
{
	public bool passwordok { get; set; }
	public double sessionid { get; set; }
	public bool userok { get; set; }
}

