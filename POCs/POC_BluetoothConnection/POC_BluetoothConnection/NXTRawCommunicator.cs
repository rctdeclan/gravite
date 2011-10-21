using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace POC_BluetoothConnection
{
    class NXTRawCommunicator
    {
        SerialPort connection;
        public void Connect(String port)
        {
            try
            {
                connection = new SerialPort();
                connection.PortName = port;
                connection.Open();
                connection.ReadTimeout = 1000;
            }
            catch (ArgumentException e)
            {
                throw (e);
            }
        }

        public void Disconnect()
        {
            if (connection!=null)
            {
                connection.Close();
            }
        }

        public String ReceiveString()
        {
            int length = connection.ReadByte() + 256 * connection.ReadByte();
            Byte[] bytes = new Byte[length];
            connection.Read(bytes, 0, length);
            return Encoding.ASCII.GetString(bytes);
        }

        public void SendString(String message)
        {
            Byte[] Header = { 0x00,0x00};
            Header[0] = (byte)message.Length;
            connection.Write(Header, 0, Header.Length);
            connection.Write(message);
        }

        public int ReceiveInt()
        {
            int output = 0;
            int length = connection.ReadByte() + 256 * connection.ReadByte();
            for (int i = 0; i < length; i++)
            {
                output += connection.ReadByte();
            }
            return output;
        }

        public void SendInt(int message)
        {
            Byte[] Message = { 0x01, 0x00, (byte) message};
            connection.Write(Message, 0, Message.Length);
        }
    }
}
