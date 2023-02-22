using System;
using System.Text.Json;

namespace MSL4_Mobile.Services;

public class ChannelService
{
	private static HttpClient client = new HttpClient();

	public async static Task<List<DigitalInput>> GetDigitalInputs(string ip, string sessionid)
	{
		Uri uri = new Uri($"http://{ip}/RestDigitalIn/{sessionid}/");
		List<DigitalInput> digitalInputChannelData = new List<DigitalInput>();

		try
		{
			HttpResponseMessage response = await client.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				string stringResponse = await response.Content.ReadAsStringAsync();
				digitalInputChannelData = JsonSerializer.Deserialize<List<DigitalInput>>(stringResponse);
			} else
			{
				Console.WriteLine("HTTP Request Failure: " + response.StatusCode);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(@"\tERROR {0}", ex.Message);
		}
		return digitalInputChannelData;
	}

	public async static Task<List<DigitalOutput>> GetDigitalOutputs(string ip, string sessionid)
	{
		Uri uri = new Uri($"http://{ip}/RestDigitalOut/{sessionid}/");
		List<DigitalOutput> digitalOutputChannelData = new List<DigitalOutput>();

		try
		{
			HttpResponseMessage response = await client.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				string stringResponse = await response.Content.ReadAsStringAsync();
				digitalOutputChannelData = JsonSerializer.Deserialize<List<DigitalOutput>>(stringResponse);
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
		return digitalOutputChannelData;
	}

    public async static Task<List<AnalogInput>> GetAnalogInputs(string ip, string sessionid)
    {
        Uri uri = new Uri($"http://{ip}/RestAnalog/{sessionid}/");
        List<AnalogInput> analogInputs = new List<AnalogInput>();

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                analogInputs = JsonSerializer.Deserialize<List<AnalogInput>>(stringResponse);
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
        return analogInputs;
    }

	public async static Task<List<AnalogOutput>> GetAnalogOutputs(string ip, string sessionid)
	{
        Uri uri = new Uri($"http://{ip}/RestAnalogOut/{sessionid}/");
        List<AnalogOutput> analogOutputs = new List<AnalogOutput>();

        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                analogOutputs = JsonSerializer.Deserialize<List<AnalogOutput>>(stringResponse);
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
        return analogOutputs;
    }
}


public class DigitalInput
{
	public int pDBChannelNr { get; set; }
	public string pName { get; set; }
	public bool pValue { get; set; }
	public string pValueText { get; set; }
	public string pUnit { get; set; }
	public string pLimesMin { get; set; }
	public string pLimesMax { get; set; }
	public string pDigitalType { get; set; }
	public string pAlertMessage { get; set; }
	public int pDBVisualTypeID { get; set; }
	public int pChannelAddress { get; set; }
	public string pDigitalModus { get; set; }
	public bool pActive { get; set; }
	public int pJobPeriod { get; set; }
	public int id { get; set; }
	public string pDescription { get; set; }
}

public class DigitalOutput
{
	public int pDBChannelNr { get; set; }
	public string pName { get; set; }
	public bool pActive { get; set; }
	public bool pValue { get; set; }
	public int id { get; set; }
	public string pDescription { get; set; }
}

public class AnalogInput
{
    public int pDBChannelNr { get; set; }
	public string pUnit { get; set; }
	public string pValue { get; set; }
	public string pMinPower { get; set; }
	public string pFactor { get; set; }
	public string pValueCalc { get; set; }
	public string pName { get; set; }
	public int pDBVisualTypeID { get; set; }
	public int pAnalogType { get; set; }
    public int pChannelAddress { get; set; }
    public bool pActive { get; set; }
	public string pMaxPower { get; set; }
	public int pJobPeriod { get; set; }
    public int id { get; set; }
}

public class AnalogAlert
{
	public int pDBChannelNr { get; set; }
	public string pLimesDelayIn { get; set; }
	public string pValueCalc { get; set; }
	public string pAlertMessage { get; set; }
	public string pName { get; set; }
	public string pUnit { get; set; }
	public int pAnalogType { get; set; }
	public int pChannelAddress { get; set; }
	public string pLimesMin { get; set; }
	public string pLimesMax { get; set; }
	public string pLimesDelayOut { get; set; }
	public int id { get; set; }
}

public class AnalogOutput
{
    public int pDBChannelNr { get; set; }
    public string pValuePower { get; set; }
    public string pMinPower { get; set; }
    public string pMaxPower { get; set; }
    public string pFactor { get; set; }
    public string pValueAnalog { get; set; }
    public string pName { get; set; }
    public int pAnalogType { get; set; }
    public bool pActive { get; set; }
    public int id { get; set; }
}

public enum AnalogType
{
	TYPE_0_10_V = 1,
	TYPE_0_20_MA = 2,
	TYPE_SENSOR_TEMPERATUR = 3,
	TYPE_FUELLSTAND = 4,
    TYPE_SENSOR_REL_FEUCHTE = 5,
    TYPE_WASSER = 6,
	TYPE_NTC_5_K = 7,
    TYPE_4_20_MA = 8,
    TYPE_0K4_2_V = 9
}

/*public class UnitList
{
	private static readonly List<string> units = new()
	{
		"A", "Bar", "°C", "K", "d", "h", "Hz",
		"kg", "kg/h", "kvar", "kvarh", "kVA",
		"kVAh", "kW/m²", "kW", "kWh", "I",
		"I/h", "kPa", "m³", "m³/h", "MByte",
		"min", "mA", "mK", "Nm³", "Nm³/h",
		"No", "on/off", "Pa", "ppm", "s",
		"Stck", "V", "%"
    };
	public static List<string> Units { get => units; }

	public UnitList() { }
}*/