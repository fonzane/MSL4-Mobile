using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class AuthService
{
	private static HttpClient client = new HttpClient();
    private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    public static string sessionid;
	public static string ipaddress { get; set; } = "";
	public static string auth_token { get; set; }
	public static bool isConnected = false;
	public static string baseURL = "https://proxy.webmonitor.fw-systeme.de/?target=";
	public static string mslAddress;

	public static async Task<string> GetSessionID(string ip)
	{
		//client.DefaultRequestHeaders.Add("Cookie", "authorization=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRzY2hhZmZlcnRAZnctc3lzdGVtZS5kZSIsIm5hbWUiOiJUaW0gU2NoYWZmZXJ0Iiwicm9sZXMiOlsic3VwZXJ1c2VyIl0sImRpdmlzaW9uIjoiVXNlciIsIl9pZCI6IjYxYjIwYTQ4ZWViZWY4YmEzYzkwMzE2OSIsImFkbWluaXN0cmF0b3MiOlsiNjFiMjBhNDhlZWJlZjhiYTNjOTAzMTY5Il0sImlhdCI6MTY3ODg4NzI1NiwiZXhwIjoxNjc5MjMyODU2fQ.qE7FwU7Vu34ZHJzCeYUBXCZiYEdslvhUca0LSSUxHCA");
        ipaddress = ip;

		if (ip.Contains("192") || ip.Contains("10."))
		{
			mslAddress = $"http://{ip}";
		} else
		{
			mslAddress = $"http://{ip}";
		}

		Uri uri = new Uri($"{mslAddress}/LogWeb/servlet/FOLogin?pUser=administrator&pPassword=admin");
        AuthResponse authResponse = new AuthResponse();

		try
		{
			HttpResponseMessage response = await client.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				isConnected = true;
				string stringResponse = await response.Content.ReadAsStringAsync();
				authResponse = JsonSerializer.Deserialize<AuthResponse>(stringResponse);
				sessionid = authResponse.sessionid.ToString();
				Console.WriteLine("SessionID: " + sessionid);
			}
		} catch (Exception ex)
		{
			Console.WriteLine(@"\tERROR {0}", ex.Message);
			return "false";
		}
		return authResponse.sessionid.ToString();
    }

	public static async Task<bool> LoginWebmonitor(string email, string password)
	{
		client.Timeout = TimeSpan.FromSeconds(5);
		Uri uri = new Uri("https://api.webmonitor.fw-systeme.de/auth/login");
		LoginData loginData = new LoginData
		{
			email = email,
			password = password
		};
        try
        {
            string json = JsonSerializer.Serialize<LoginData>(loginData, serializerOptions);
            StringContent payload = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            Console.WriteLine(json.ToString());

			HttpResponseMessage response = await client.PostAsync(uri, payload);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                LoginResponse loginResponse = JsonSerializer.Deserialize<LoginResponse>(stringResponse);
				auth_token = loginResponse.access_token;
                Console.WriteLine("Login successfull");
				Console.WriteLine(auth_token);
                return true;
            }
            else
            {
                Console.WriteLine("HTTP Request Failure: " + response.StatusCode);
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(@"\tERROR {0}", ex.Message);
            return false;
        }
    }
}

public class AuthResponse
{
	public bool passwordok { get; set; }
	public double sessionid { get; set; }
	public bool userok { get; set; }
}

public record LoginResponse
{
	public string access_token { get; set; }
}

public class LoginData
{
	public string email { get; set; }
	public string password { get; set; }
}

