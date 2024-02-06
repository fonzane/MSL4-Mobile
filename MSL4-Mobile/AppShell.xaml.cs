using System.Reflection;
using MSL4_Mobile.Services;
using MSL4_Mobile.Views.Connection;

namespace MSL4_Mobile;

public partial class AppShell : Shell
{
	public bool showLoginTab { get; set; } = true;
	public bool showNav { get; set; } = false;
	public bool showConnectionTab { get; set; } = true;

	public AppShell()
	{
		InitializeComponent();

		MessagingCenter.Subscribe<MainPage>(this, "isLoggedIn", sender =>
		{
			Console.WriteLine("Removing login tab?");
			this.showLoginTab = false;
			OnPropertyChanged(nameof(showLoginTab));
		});

		MessagingCenter.Subscribe<ConnectionView>(this, "isConnected", async sender =>
		{
			Console.WriteLine("Showing Nav");
			showNav = true;
			OnPropertyChanged(nameof(showNav));
			await Task.Delay(200);
			showConnectionTab = false;
			OnPropertyChanged(nameof(showConnectionTab));
		});
	}

    //ShellContent _previousShellContent;

    //protected override void OnNavigated(ShellNavigatedEventArgs args)
    //{
    //    base.OnNavigated(args);
    //    if (CurrentItem?.CurrentItem?.CurrentItem is not null &&
    //        _previousShellContent is not null)
    //    {
    //        var property = typeof(ShellContent)
    //            .GetProperty("ContentCache", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

    //        property.SetValue(_previousShellContent, null);
    //    }

    //    _previousShellContent = CurrentItem?.CurrentItem?.CurrentItem;
    //}

}

