using BaseUtils.Mvvm;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using WeightMonitor.Services;
using WeightMonitor.Views;

namespace WeightMonitor.ViewModels;
public partial class MainVM : BaseViewModel
{
	private readonly SerialPortService _serialService;
	private readonly WeightGraphVM _weightGraphVM;
	private CancellationTokenSource _cts;

	private double _currentWeight;
	public double CurrentWeight
	{
		get => _currentWeight;
		set => SetProperty(ref _currentWeight, value);
	}

	public MainVM(SerialPortService serialService, WeightGraphVM weightGraphVM)
	{
		_serialService = serialService;
		_weightGraphVM = weightGraphVM;
		_cts = new CancellationTokenSource();
		StartReadingDataAsync(_cts.Token);
		serialService.Start(true);
	}

	private async void StartReadingDataAsync(CancellationToken token)
	{
		var reader = _serialService.MessageReader;

		await foreach (var value in reader.ReadAllAsync(token))
		{
			CurrentWeight = value;
			// Můžeš přidat logiku pro graf, historii atd.
		}
	}

	public void Stop()
	{
		_cts.Cancel();
	}

	[RelayCommand]
	private void ResetGraph()
	{
		// Logika pro resetování grafu
		_weightGraphVM.Clear(); // nebo jiná metoda
	}

	[RelayCommand]
	private void ShowOptions()
	{
		var optionsVM = Ioc.Default.GetService<OptionsVM>();
		var optionsWnd = new OptionsWnd
		{
			DataContext = optionsVM
		};
		optionsWnd.Show(); // no modal dialog
	}
}
