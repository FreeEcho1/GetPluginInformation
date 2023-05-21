using System.Windows;

namespace GetPluginInformation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(
        StartupEventArgs e
        )
    {
        base.OnStartup(e);

        Processing.WritePluginsInformation();

        Shutdown();
    }
}
