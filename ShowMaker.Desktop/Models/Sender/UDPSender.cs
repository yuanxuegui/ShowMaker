using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ShowMaker.Desktop.Sender
{
    public class UDPSender : ISender
    {
        public UDPSender(string ip, string port)
        {
            Debug.WriteLine("IP: {0}, PORT: {1}", ip, port);
        }

        public bool Send(string message)
        {
            Debug.WriteLine("Send Msg: " + message);
            return true;
        }
    }
}
