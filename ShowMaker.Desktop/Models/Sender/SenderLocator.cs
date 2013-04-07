using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowMaker.Desktop.Sender
{
    public class SenderLocator
    {
        private static ISender sender = null;

        public static void Register(ISender sender)
        {
            SenderLocator.sender = sender;
        }

        public static ISender Lookup()
        {
            return SenderLocator.sender;
        }
    }
}
