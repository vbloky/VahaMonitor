using Avalonia.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using WeightMonitor.ViewModels;

namespace WeightMonitor.Views;

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
