using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using VahaMonitor.Services;


namespace VahaMonitor.ViewModels;
public class WeightGraphVM : ObservableObject
{
	private readonly ObservableCollection<DateTimePoint> _points = new();
	private readonly LineSeries<DateTimePoint> _series;
	private readonly SerialPortService _serialService;
	private readonly DispatcherTimer _updateTimer;

	private TimeSpan _selectedRange = TimeSpan.FromMinutes(1);
	private double _yMarginRatio = 0.05;

	public ISeries[] Series { get; set; } =
	{
		new LineSeries<double>
		{
			Values = new ObservableCollection<double>(),
			Fill = null
		}
	};


	public Axis[] XAxes { get; set; } =
	{
		new Axis
		{
			LabelsRotation = 15,
			Labeler = value => DateTime.Now.AddSeconds(value).ToString("HH:mm:ss"),
			MinLimit = 0
		}
	};

	public Axis[] YAxes { get; set; } =
	{
		new Axis
		{
			MinLimit = 0,
			MaxLimit = 10000
		}
	};


	public WeightGraphVM(SerialPortService serialService)
	{
		_serialService = serialService;

		_series = new LineSeries<DateTimePoint>
		{
			Values = _points,
			Fill = null,
			GeometrySize = 4
		};

		XAxes = new[]
		{
			new Axis
			{
				Labeler = value => new DateTime((long)value).ToString("HH:mm:ss"),
				UnitWidth = TimeSpan.FromSeconds(1).Ticks,
				MinLimit = double.NaN,
				MaxLimit = double.NaN
			}
		};

		YAxes = new[]
		{
			new Axis
			{
				MinLimit = double.NaN,
				MaxLimit = double.NaN
			}
		};

		_updateTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(333) };
		_updateTimer.Tick += (_, _) => UpdateGraph();
		_updateTimer.Start();
	}

	private void UpdateGraph()
	{
		while (_serialService.MessageReader.TryRead(out var value))
		{
			_points.Add(new DateTimePoint(DateTime.Now, value));
		}

		var cutoff = DateTime.Now - _selectedRange;
		while (_points.Count > 0 && _points[0].DateTime < cutoff)
		{
			_points.RemoveAt(0);
		}

		if (_points.Count > 0)
		{
			var minY = _points.Min(p => p.Value);
			var maxY = _points.Max(p => p.Value);
			var margin = (maxY - minY) * _yMarginRatio;

			YAxes[0].MinLimit = minY - margin;
			YAxes[0].MaxLimit = maxY + margin;

			XAxes[0].MinLimit = _points[0].DateTime.Ticks;
			XAxes[0].MaxLimit = _points[^1].DateTime.Ticks;
		}
	}
}
