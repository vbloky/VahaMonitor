using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using HotAvalonia;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using VahaMonitor.Services;
using VahaMonitor.ViewModels;
using VahaMonitor.Views;

namespace VahaMonitor;

public partial class App : Application
{
	public override void Initialize()
	{
		this.EnableHotReload(); // Aktivace
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			// Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
			// More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
			DisableAvaloniaDataAnnotationValidation();


			Ioc.Default.ConfigureServices(
			new ServiceCollection()
				.AddSingleton<MainVM>()
				.AddSingleton<SerialPortService>()
				.AddSingleton<WeightGraphVM>()
				.BuildServiceProvider()
			);

			desktop.MainWindow = new MainWnd
			{
				DataContext = Ioc.Default.GetService<MainVM>(),
			};
		}

		base.OnFrameworkInitializationCompleted();
	}

	private void DisableAvaloniaDataAnnotationValidation()
	{
		// Get an array of plugins to remove
		var dataValidationPluginsToRemove =
			BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

		// remove each entry found
		foreach (var plugin in dataValidationPluginsToRemove)
		{
			BindingPlugins.DataValidators.Remove(plugin);
		}
	}
}