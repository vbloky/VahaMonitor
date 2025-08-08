using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Channels;
using System.Timers;

namespace VahaMonitor.Services
{
	public class SerialPortService
	{
		private readonly SerialPort _serialPort;
		private readonly Channel<double> _messageChannel;
		private StringBuilder _buffer;
		private readonly Timer _simulationTimer;
		private readonly Random _random;
		private int _sampleIndex;
		private const int SamplesPerSinusoid = 45; // 15 seconds * 3 samples per second

		public SerialPortService()
		{
			_serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
			_messageChannel = Channel.CreateUnbounded<double>();
			_buffer = new StringBuilder();
			_random = new Random();
			_sampleIndex = 0;

			_serialPort.DataReceived += OnDataReceived;

			// Nastavení simulace
			_simulationTimer = new Timer(333); // 3x za sekundu
			_simulationTimer.Elapsed += (sender, e) => SimulateDataGeneration();
		}

		public ChannelReader<double> MessageReader => _messageChannel.Reader;

		private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			try
			{
				var data = _serialPort.ReadExisting();
				_buffer.Append(data);

				if (IsPacketComplete(_buffer.ToString()))
				{
					var packet = _buffer.ToString();
					_buffer.Clear();

					if (IsPacketValid(packet))
					{
						var value = ExtractValue(packet);
						_messageChannel.Writer.TryWrite(value);
					}
				}
			}
			catch (Exception ex)
			{
				// Logika pro zpracování chyb
			}
		}

		private bool IsPacketComplete(string data)
		{
			// Logika pro kontrolu, zda je paket kompletní
			return data.EndsWith("\n");

		}

		private bool IsPacketValid(string packet)
		{
			// Logika pro kontrolu konzistence dat
			return true;
		}

		private double ExtractValue(string packet)
		{
			// Logika pro extrakci hodnoty z paketu
			return double.Parse(packet);
		}

		private void SimulateDataGeneration()
		{
			double sinusoidalValue = 4000 * (1 + Math.Sin(2 * Math.PI * _sampleIndex / SamplesPerSinusoid));
			double noisyValue = sinusoidalValue + _random.Next(-50, 51);
			_messageChannel.Writer.TryWrite(noisyValue);
			_sampleIndex = (_sampleIndex + 1) % SamplesPerSinusoid;
		}

		public void Start(bool isSimulation)
		{
			if (!isSimulation)
				_serialPort.Open();
			if (isSimulation)
				_simulationTimer.Start();
		}

		public void Stop()
		{
			_serialPort.Close();
			_simulationTimer.Stop();
		}

		public void Dispose()
		{
			_serialPort?.Dispose();
			_simulationTimer?.Dispose();
		}
	}
}
