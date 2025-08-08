using Avalonia.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using VahaMonitor.ViewModels;

namespace VahaMonitor.Views;

public partial class WeightGraphUC : UserControl
{
	public WeightGraphUC()
	{
		InitializeComponent();

		if (!Design.IsDesignMode)
		{
			DataContext = Ioc.Default.GetService<WeightGraphVM>();
		}
	}
}
