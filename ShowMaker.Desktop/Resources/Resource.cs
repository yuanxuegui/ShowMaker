using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRcp;
using System.Resources;
using System.Globalization;

namespace ShowMaker.Desktop.Resources
{
    [ExportResource]
    public class Resource : IResource
    {
        private ResourceManager stringResource;
        private CultureInfo culture = new CultureInfo("zh-cn");

        public Resource()
        {
            stringResource = new ResourceManager("ShowMaker.Desktop.Resources.StringResource", typeof(Resource).Assembly);
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
