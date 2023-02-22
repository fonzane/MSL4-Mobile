using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Analog;

public partial class AnalogDetailsView : ContentPage
{
	public AnalogInput analogInput { get; set; }

	public List<string> Units { get; } = new List<string>()
    {
        "A", "Bar", "°C", "K", "d", "h", "Hz",
        "kg", "kg/h", "kvar", "kvarh", "kVA",
        "kVAh", "kW/m²", "kW", "kWh", "I",
        "I/h", "kPa", "m³", "m³/h", "MByte",
        "min", "mA", "mK", "Nm³", "Nm³/h",
        "No", "on/off", "Pa", "ppm", "s",
        "Stck", "V", "%"
    };

    public AnalogDetailsView(AnalogInput analogInput)
	{
		this.analogInput = analogInput;
		Console.WriteLine("Received analog input " + this.analogInput.pName);
		InitializeComponent();
    }
    
}
