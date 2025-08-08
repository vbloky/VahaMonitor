using Avalonia.Media;
using BaseUtils.Mvvm;
using WeightMonitor.ViewModels;

public partial class OptionsVM : BaseViewModel
{
	public OptionsModel Settings { get; }

	public OptionsVM(OptionsModel settings)
	{
		Settings = settings;
	}

	// 🎨 Barva grafu jako Avalonia Color
	public Color GraphColor
	{
		get => Color.FromUInt32((uint)Settings.GraphColor);
		set => Settings.GraphColor = (int)value.ToUint32();
	}

	public Color GraphBorderColor
	{
		get => Color.FromUInt32((uint)Settings.GraphBorderColor);
		set => Settings.GraphBorderColor = (int)value.ToUint32();
	}

	public Color GraphAxisLabelColor
	{
		get => Color.FromUInt32((uint)Settings.GraphAxisLabelColor);
		set => Settings.GraphAxisLabelColor = (int)value.ToUint32();
	}

	// 🖌️ Pokud preferujete Brush pro UI binding
	public IBrush GraphColorBrush
	{
		get => new SolidColorBrush(GraphColor);
		set => GraphColor = (value as SolidColorBrush)?.Color ?? GraphColor;
	}

	public IBrush GraphBorderBrush
	{
		get => new SolidColorBrush(GraphBorderColor);
		set => GraphBorderColor = (value as SolidColorBrush)?.Color ?? GraphBorderColor;
	}

	public IBrush GraphAxisLabelBrush
	{
		get => new SolidColorBrush(GraphAxisLabelColor);
		set => GraphAxisLabelColor = (value as SolidColorBrush)?.Color ?? GraphAxisLabelColor;
	}
}