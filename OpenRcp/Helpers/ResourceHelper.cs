using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace OpenRcp.Helpers
{
    public class ResourceHelper
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static readonly ResourceHelper Instance = new ResourceHelper();

        /// <summary>
        /// Helper class.
        /// </summary>
        private static readonly IndexerClass IndexerClassInstance = new IndexerClass();

        /// <summary>
        /// Private constructor ensures singleton.
        /// </summary>
        private ResourceHelper() { }

        public IndexerClass Resource
        {
            get
            {
                return IndexerClassInstance;
            }
        }

        public class IndexerClass
        {
            public string this[string index]
            {
                get
                {
                    return IoC.Get<IResourceService>().GetString(index);
                }
            }
        }
    }
}
