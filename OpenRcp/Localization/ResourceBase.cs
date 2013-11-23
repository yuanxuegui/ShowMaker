using System.Globalization;
using System.Resources;

namespace OpenRcp
{
	public class ResourceBase : IResource
	{
		private ResourceManager stringResource;
		private CultureInfo culture = new CultureInfo("en-us");

		public ResourceBase(string stringRes)
		{
            stringResource = new ResourceManager(stringRes, typeof(ResourceBase).Assembly);
		}

		#region IResource Members

		public CultureInfo CurrentCulture 
		{ 
			set
			{
				culture = value;
			}
		}

		public string GetString(string name)
		{
			return stringResource.GetString(name, culture);
		}

		#endregion
	}
}
