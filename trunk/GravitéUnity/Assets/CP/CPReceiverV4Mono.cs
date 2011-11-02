﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace CP
{
    public class CPReceiverV4Mono
    {
        //protected SByte[] inData;
        protected Byte[] outData;
		protected Byte[] inData;
        Byte[] tempBytes;
        SerialPort connection;
        //SerialDataReceivedEventHandler handler;
        public void Connect(String port)
        {
            try
            {
                connection = new SerialPort();
                connection.PortName = port;
				connection.RtsEnable = true;
                try
                {
                    connection.Open();
                    Trace.WriteLine("Connected");
                }
                catch (IOException e)
                {
                    Trace.WriteLine("IOException: Is the NXT switched on and Running CPSender?");
                    throw e;
                }
                connection.ReadTimeout = 1000;
            }
            catch (ArgumentException e)
            {
                throw (e);
            }
            inData = new Byte[6];
            tempBytes = new Byte[6];
            outData = new Byte[3];
            outData[0] = 1;
        }

        public void Disconnect()
        {
            if (connection != null)
                connection.Close();
            Trace.WriteLine("Disconnected");
        }

        public void removeStartupData()
        {
            Byte[] shit = new Byte[8];
            connection.Read(shit,0,8);
        }

        int messLength;

        public void PollData()
        {
            messLength = connection.ReadByte();
            connection.ReadByte();//remove header
            try { connection.Read(tempBytes, 0, messLength); }
            catch (TimeoutException) { return; }
            inData = tempBytes;
        }

        protected void SendData()
        {
            connection.Write(outData, 0, 3);
        }

        public int zTilt
        {
            get { return inData[0]; }
        }

        public int yTilt
        {
            get { return inData[1]; }
        }

        public int soundVol
        {
            get { return inData[2]; }
        }

        public bool left
        {
            get { return inData[3] == 1 || inData[3] == 3; }
        }

        public bool right
        {
            get { return inData[3] == 2 || inData[3] == 3; }
        }

        public void StartMotor()
        {
            outData[2] = 1;
            SendData();
        }

        public void StopMotor()
        {
            outData[2] = 2;
            SendData();
        }

        public void Start()
        {
            outData[2] = 4;
            SendData();
        }

        public void ExitNXT()
        {
            outData[2] = 3;
            SendData();
        }
    }


}
