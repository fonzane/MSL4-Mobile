using MSL4_Mobile.Services;

namespace MSL4_Mobile;

public partial class MainPage : ContentPage
{

	public string ip { get; set; }

	public MainPage()
	{
		InitializeComponent();
		Shell.SetTabBarIsVisible(Application.Current.MainPage, false);
	}

    async void OnConnectToMSL(System.Object sender, System.EventArgs e)
    {
		Console.WriteLine(ip);
		await AuthService.GetSessionID(ip);
		await Shell.Current.GoToAsync("//infoView");
		//Navigation.PushAsync(new Views.Home.HomeView());
	}

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    Shell.SetTabBarIsVisible(Application.Current.MainPage, false);
    //}

}