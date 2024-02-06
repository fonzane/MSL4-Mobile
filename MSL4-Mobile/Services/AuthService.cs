
using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class AuthService
{
	public static HttpClient client = new HttpClient();
    private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    public static string sessionid;
	public static string ipaddress { get; set; } = "";
	public static string auth_token { get; set; }
	public static bool isConnected = false;
    public static string proxyURL = "http://localhost:3333/?maui=";
    //public static string proxyURL = "https://proxy.webmonitor.fw-systeme.de/?maui=";
    public static string webmonitorURL = "http://localhost:3000";
    //public static string webmonitorURL = "https://api.webmonitor.fw-systeme.de";
    public static string mslAddress;

	static AuthService()
	{
        client.Timeout = TimeSpan.FromSeconds(10);
    }

    public static async Task<string> GetSessionID(string ip)
	{
        ipaddress = ip;

		if (ip.Contains("192") || ip.Contains("10."))
		{
			mslAddress = $"{proxyURL}{ip}";
		} else
		{
			mslAddress = $"{proxyURL}{ip}";
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
		Uri uri = new Uri($"{webmonitorURL}/auth/login");
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
                client.DefaultRequestHeaders.Add("Cookie", $"authorization={auth_token}");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth_token}");
                try
                {
                    SecureStorage.Default.SetAsync("saved_token", auth_token);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

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

	//private static void DecodeJWTAndPrint(string jwt)
	//{

 //       jwt = jwt.Replace('_', '/').Replace('-', '+');
 //       string payload = jwt.Split(".")[1];

 //       switch (payload.Length % 4)
 //       {
 //           case 2: payload += "=="; break;
 //           case 3: payload += "="; break;
 //       }
 //       byte[] decoded = Convert.FromBase64String(payload);
 //       string decodedToken = System.Text.Encoding.Default.GetString(decoded);
 //       Console.WriteLine(decodedToken);

 //       JWTPayload jWTPayload = JsonSerializer.Deserialize<JWTPayload>(decodedToken);
 //       Console.WriteLine("Expiration: " + jWTPayload.exp);
 //       Console.WriteLine("Timestamp" + DateTimeOffset.UtcNow.ToUnixTimeSeconds());
 //   }

    public async static Task<bool> CheckLoggedIn()
    {
        string saved_token = await SecureStorage.Default.GetAsync("saved_token");
        if (saved_token != null)
        {
            Console.WriteLine("Token: " + saved_token);
            if (CheckJWTValidity(saved_token))
            {
                auth_token = saved_token;
                client.DefaultRequestHeaders.Add("Cookie", $"authorization={auth_token}");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth_token}");
                return true;
            } else
            {
                Console.WriteLine("JWT Validity check failed");
                return false;
            }
        } else
        {
            Console.WriteLine("Check Login: no token found.");
            return false;
        }
    }

    private static bool CheckJWTValidity(string jwt)
    {
        jwt = jwt.Replace('_', '/').Replace('-', '+');
        string payload = jwt.Split(".")[1];
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }
        byte[] decoded = Convert.FromBase64String(payload);
        string decodedToken = System.Text.Encoding.Default.GetString(decoded);
        Console.WriteLine(decodedToken);

        JWTPayload jWTPayload = JsonSerializer.Deserialize<JWTPayload>(decodedToken);
        Console.WriteLine("Expiration: " + jWTPayload.exp);
        Console.WriteLine("Timestamp: " + DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > jWTPayload.exp)
        {
            return false;
        }
        else
        {
            return true;
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

public class JWTPayload
{
    public string email { get; set; }
    public string name { get; set; }
    public List<string> roles { get; set; }
    public string division { get; set; }
    public string _id { get; set; }
    public List<string> administratos { get; set; }
    public List<string> mapModes { get; set; }
    public List<string> allowedIPs { get; set; }
    public double iat { get; set; }
    public double exp { get; set; }
}