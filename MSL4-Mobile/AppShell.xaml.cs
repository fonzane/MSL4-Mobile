using MSL4_Mobile.Services;

namespace MSL4_Mobile;

public partial class AppShell : Shell
{
	public bool isConnected = false;

	public AppShell()
	{
		Console.WriteLine(AuthService.isConnected.ToString());
		InitializeComponent();
		isConnected = true;
		OnPropertyChanged(nameof(isConnected));
	}
}

