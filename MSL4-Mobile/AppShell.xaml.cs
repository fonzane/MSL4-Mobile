using MSL4_Mobile.Services;

namespace MSL4_Mobile;

public partial class AppShell : Shell
{
	public bool showConnectionTab { get; set; } = true;

	public AppShell()
	{
		InitializeComponent();
		MessagingCenter.Subscribe<MainPage>(this, "isConnected", sender =>
		{
			this.showConnectionTab = false;
			OnPropertyChanged(nameof(showConnectionTab));
		});
	}

}

