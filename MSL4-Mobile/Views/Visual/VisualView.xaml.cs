using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Maui.Controls.PlatformConfiguration;
using MSL4_Mobile.Services;
#if IOS || MACCATALYST
using Foundation;
#endif

namespace MSL4_Mobile.Views.Visual;

public partial class VisualView : ContentPage
{
	//public string url { get; set; } = System.Web.HttpUtility.UrlEncode(AuthService.mslAddress + "/visual/");
	public Uri url { get; set; } = new Uri(AuthService.mslAddress + "/visual");
	private Uri uri = new Uri(AuthService.mslAddress + "/visual/", UriKind.RelativeOrAbsolute);
	//private WebView visualWebView { get; init; }

    public VisualView()
	{
        InitializeComponent();
#if MACCATALYST
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("Inspect", (handler, view) =>
        {
            if (OperatingSystem.IsMacCatalystVersionAtLeast(16, 4))
                handler.PlatformView.Inspectable = true;
        });
#endif

        CookieContainer cookieContainer = new CookieContainer();
        Cookie cookie = new Cookie
        {
            Name = "authorization",
            Expires = DateTime.Now.AddDays(1),
            Value = $"{AuthService.auth_token}",
            Domain = uri.Host,
            Path = "/"
        };
        cookieContainer.Add(uri, cookie);
        visualWebView.Cookies = cookieContainer;
        visualWebView.Source = new UrlWebViewSource { Url = uri.ToString() };

    }

	private async Task LoadWebViewContent()
	{
        try
        {
            // Make the HTTP request and get the content as a string
            string content = await AuthService.client.GetStringAsync(uri);

            var scriptMatches = Regex.Matches(content, @"<script[^>]*?src=[""'](.*?)[""'][^>]*?>");
            var styleMatches = Regex.Matches(content, @"<style.*?>(.*?)</style>", RegexOptions.Singleline);
            var backGroundMatches = Regex.Matches(content, @"background-image\s*:\s*url\s*\(\s*['""]?(.*?)['""]?\s*\)");

            foreach (Match bgMatch in backGroundMatches)
            {
                string absoluteUrl = AuthService.mslAddress + bgMatch.Groups[1].Value;
                content = content.Replace(bgMatch.Groups[0].Value, $"background-image: url('{absoluteUrl}')");
            }

            foreach (Match scriptMatch in scriptMatches)
            {
                string scriptUrl = AuthService.mslAddress + scriptMatch.Groups[1].Value;
                Console.WriteLine("Fetching Script: " + scriptUrl);
                try
                {
                    // Fetch the content of the script
                    string scriptContent = await AuthService.client.GetStringAsync(scriptUrl);

                    // Replace the script tag with the actual content
                    content = Regex.Replace(content, @"<script[^>]*?src=[""'](.*?)[""'][^>]*?>", $"<script>{HttpUtility.HtmlEncode(scriptContent)}</script>");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching script content: {ex.Message}");
                }
                
            }

            foreach (Match styleMatch in styleMatches)
            {
                string styleContent = styleMatch.Groups[1].Value;

                styleContent = Regex.Replace(styleContent, @"@import\s+(url\()?\s*['""]?([^;]+)['""]?\s*\)?;", match =>
                {
                    string importUrl = match.Groups[2].Value;

                    Console.WriteLine("Style relative URL: " + importUrl);

                    // Convert the relative URL to absolute URL
                    string absoluteImportUrl = HttpUtility.HtmlEncode(AuthService.mslAddress + importUrl);

                    return $"@import url('{absoluteImportUrl}');";
                });

                // Encode the embedded style content before replacing
                string encodedStyleContent = HttpUtility.HtmlEncode(styleContent);

                // Replace the style tag with the encoded content
                content = content.Replace(styleMatch.Value, $"<style>{encodedStyleContent}</style>");

            }


            // Load the content into the WebView
            visualWebView.Source = new HtmlWebViewSource { Html = content };
            //visualWebView.Reload();
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., network issues, request errors)
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async void OpenBrowser()
	{
		try
		{
			await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
		} catch (Exception ex)
		{
			Console.WriteLine("Failed to open browser for msl visual page");
			Console.WriteLine(ex);
		}
	}
}
