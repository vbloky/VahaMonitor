using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;

namespace BaseUtils.Mvvm
{
	/// <summary>
	/// Base class for all ViewModels in the application.
	/// </summary>
	public abstract class BaseViewModel : ObservableObject
	{
		//property as PUBLIC to enable call from outside of the class (in base is defined as PROTECTED)
		public new void OnPropertyChanged([CallerMemberName] string? propertyName = null) => base.OnPropertyChanged(propertyName);
		public void OnAllPropertiesChanged() => base.OnPropertyChanged(string.Empty);
	}
}
