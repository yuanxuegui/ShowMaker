using System.Reflection;
using Caliburn.Micro;

namespace OpenRcp
{
	public static class ExtensionMethods
	{
		public static string GetExecutingAssemblyName()
		{
			return Assembly.GetExecutingAssembly().GetAssemblyName();
		}
	}
}