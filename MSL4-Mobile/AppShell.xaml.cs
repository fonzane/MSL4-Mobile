using MSL4_Mobile.Services;
using MSL4_Mobile.Views.Connection;

namespace MSL4_Mobile;

public partial class AppShell : Shell
{
	public bool showLoginTab { get; set; } = true;
	public bool showNav { get; set; } = false;

	public AppShell()
	{
		InitializeComponent();

		MessagingCenter.Subscribe<MainPage>(this, "isLoggedIn", sender =>
		{
			Console.WriteLine("Removing login tab?");
			this.showLoginTab = false;
			OnPropertyChanged(nameof(showLoginTab));
		});

		MessagingCenter.Subscribe<ConnectionView>(this, "isConnected", sender =>
		{
			Console.WriteLine("Showing Nav");
			showNav = true;
			OnPropertyChanged(nameof(showNav));
		});
	}

}

