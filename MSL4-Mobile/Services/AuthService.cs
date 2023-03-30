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
        client.DefaultRequestHeaders.Add("Cookie", "authorization=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRzY2hhZmZlcnRAZnctc3lzdGVtZS5kZSIsIm5hbWUiOiJUaW0gU2NoYWZmZXJ0Iiwicm9sZXMiOlsic3VwZXJ1c2VyIl0sImRpdmlzaW9uIjoiVXNlciIsIl9pZCI6IjYxYjIwYTQ4ZWViZWY4YmEzYzkwMzE2OSIsImFkbWluaXN0cmF0b3MiOlsiNjFiMjBhNDhlZWJlZjhiYTNjOTAzMTY5Il0sImlhdCI6MTY3ODg4NzI1NiwiZXhwIjoxNjc5MjMyODU2fQ.qE7FwU7Vu34ZHJzCeYUBXCZiYEdslvhUca0LSSUxHCA");
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
				Console.WriteLine("SessionID: " + sessionid);
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

