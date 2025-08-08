using BaseUtils.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using WeightMonitor.Services;

namespace WeightMonitor.ViewModels;

public partial class WeightGraphVM : BaseViewModel
{
	private readonly List<double> _values = new();
	private readonly LineSeries<double> _lineSeries;
	private readonly SerialPortService _serialService;

	public ISeries[] Series { get; private set; }
	public Axis[] XAxes { get; private set; }
	public Axis[] YAxes { get; private set; }

	[ObservableProperty] public partial double MainWindowWidth { get; set; } = 800;

	public WeightGraphVM(SerialPortService serialService)
	{
		_serialService = serialService;

		_lineSeries = new LineSeries<double>
		{
			Values = _values,
			Fill = null
		};

		Series = new ISeries[] { _lineSeries };

		XAxes = new Axis[]
		{
			new Axis
			{
				Labeler = value => $"{value:F0}s",
				MinLimit = 0,
				MaxLimit = 60,
				UnitWidth = 1
			}
		};

		YAxes = new Axis[]
		{
			new Axis
			{
				MinLimit = 0,
				MaxLimit = 10000,
				Labeler = value => $"{value:F0} kg"
			}
		};

		ListenToSerialData();
	}

	private async void ListenToSerialData()
	{
		await foreach (var value in _serialService.MessageReader.ReadAllAsync())
		{
			AddValue(value);
		}
	}

	public void AddValue(double value)
	{
		_values.Add(value);

		if (_values.Count > 1800)
			_values.RemoveAt(0);

		_lineSeries.Values = null;
		_lineSeries.Values = _values;

		UpdateAxes();
	}

	private void UpdateAxes()
	{
		if (_values.Count == 0) return;

		double minY = double.MaxValue;
		double maxY = double.MinValue;

		foreach (var v in _values)
		{
			if (v < minY) minY = v;
			if (v > maxY) maxY = v;
		}

		double margin = (maxY - minY) * 0.1;
		if (margin < 100) margin = 100;

		YAxes[0].MinLimit = minY - margin;
		YAxes[0].MaxLimit = maxY + margin;

		XAxes[0].MaxLimit = _values.Count;
		XAxes[0].MinLimit = Math.Max(0, _values.Count - 1800);
	}
}
