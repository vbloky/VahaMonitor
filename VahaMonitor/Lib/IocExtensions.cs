using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


namespace BaseUtils.Mvvm
{
	public static class IocExtensions
	{
		public static Ioc Touch<T>(this Ioc ioc)
			where T : class
		{
			var res = ioc.GetService<T>();
			return ioc;
		}

		public static IServiceCollection AddMySingleton<TInterface, TImplementation>(this IServiceCollection services)
			where TInterface : class
			where TImplementation : class, TInterface
		{
			// Registrace implementace
			services.AddSingleton<TImplementation>();
			// Registrace rozhraní odkazující na implementaci
			services.AddSingleton<TInterface>(sp => sp.GetRequiredService<TImplementation>());
			//if (instantiateClass)
			//	;
			return services;
		}
	}
}
