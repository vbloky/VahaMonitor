using Avalonia.Controls;
using Avalonia.Controls.Templates;
using BaseUtils.Mvvm;
using System;

namespace VahaMonitor;

public class ViewLocator : IDataTemplate
{

	public Control? Build(object? param)
	{
		if (param is null)
			return null;

		var name = param.GetType().FullName!.Replace("ViewModel", "VM", StringComparison.Ordinal);
		var type = Type.GetType(name);

		if (type != null)
		{
			return (Control)Activator.CreateInstance(type)!;
		}

		return new TextBlock { Text = "Not Found: " + name };
	}

	public bool Match(object? data) => data is BaseViewModel;
}
