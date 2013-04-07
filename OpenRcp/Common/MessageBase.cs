using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRcp
{
    public class MessageBase : IMessage
    {
        private string content;

        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }
    }
}
