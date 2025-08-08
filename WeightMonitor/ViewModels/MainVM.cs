using BaseUtils.Mvvm;
using System.Threading;
using WeightMonitor.Services;

namespace WeightMonitor.ViewModels;
public partial class MainVM : BaseViewModel
{
	private readonly SerialPortService _serialService;
	private CancellationTokenSource _cts;

	private double _currentWeight;
	public double CurrentWeight
	{
		get => _currentWeight;
		set => SetProperty(ref _currentWeight, value);
	}

	public MainVM(SerialPortService serialService)
	{
		_serialService = serialService;
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
}
