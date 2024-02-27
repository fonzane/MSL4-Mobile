using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Chart;

public partial class ChartView : ContentPage
{

	public DBChannelResponse dbChannels;

	private ChartService chartService;

	public ChartView()
	{
		chartService = new ChartService();
		GetChannelData();
		InitializeComponent();
	}

	private async void GetChannelData()
	{
		Console.WriteLine("Getting ChannelData for the Chart");
		dbChannels = await chartService.GetDBChannelData();
		foreach(var channel in dbChannels.items)
		{
			Console.WriteLine("ID: " + channel.id + " - Label: " + channel.pLabel);
		}
	}
}
