using BaseUtils.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WeightMonitor.ViewModels;
public partial class OptionsModel : BaseViewModel
{
	#region --- Graph options ---
	[ObservableProperty] public partial int GraphTimeRangeInitial { get; set; } = 30;
	[ObservableProperty] public partial int GraphTimeRangeMax { get; set; } = 120;
	[ObservableProperty] public partial double GraphYRangeInitial { get; set; } = -10;
	[ObservableProperty] public partial double GraphYRangeMax { get; set; } = 100;
	[ObservableProperty] public partial bool ShowGraphDots { get; set; } = true;
	[ObservableProperty] public partial double GraphDotSize { get; set; } = 4;
	[ObservableProperty] public partial int GraphColor { get; set; } = 0xFFA500; // orange
	[ObservableProperty] public partial int GraphBorderColor { get; set; } = 0xCCCCCC;
	[ObservableProperty] public partial int GraphAxisLabelColor { get; set; } = 0x888888;
	#endregion

	#region --- Main window options ---
	[ObservableProperty] public partial double MainWindowWidth { get; set; } = 800;
	[ObservableProperty] public partial double MainWindowHeight { get; set; } = 600;
	[ObservableProperty] public partial double MainWindowLeft { get; set; } = 100;
	[ObservableProperty] public partial double MainWindowTop { get; set; } = 100;
	#endregion

	#region --- Serial port options ---
	[ObservableProperty] public partial bool UseTestData { get; set; } = false;
	[ObservableProperty] public partial string SerialPortName { get; set; } = "COM3";
	[ObservableProperty] public partial int SerialBaudRate { get; set; } = 9600;
	[ObservableProperty] public partial bool AutoConnectSerialPort { get; set; } = true;
	#endregion
}