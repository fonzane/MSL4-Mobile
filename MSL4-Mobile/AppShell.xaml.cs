using MSL4_Mobile.Services;

namespace MSL4_Mobile;

public partial class AppShell : Shell
{
	public AuthService AuthService { get; set; }

	public AppShell()
	{
		Console.WriteLine(AuthService.isConnected.ToString());
		InitializeComponent();
	}
}

