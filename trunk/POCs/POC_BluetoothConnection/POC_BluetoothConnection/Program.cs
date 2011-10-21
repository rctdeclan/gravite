using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POC_BluetoothConnection
{
    class Program
    {
        static string input = "";
        static void Main(string[] args)
        {
            NXTRawCommunicator rc;
            rc = new NXTRawCommunicator();
            rc.Connect("COM14");
            Console.WriteLine(rc.ReceiveString());
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine(rc.ReceiveString());
            System.Threading.Thread.Sleep(1000);
            rc.Disconnect();
        }
    }
}
