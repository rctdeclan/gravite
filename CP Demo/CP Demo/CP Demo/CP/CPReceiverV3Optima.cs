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
    class CPReceiverV3Optima
    {
        protected SByte[] inData;
        protected Byte[] outData;
        Byte[] tempBytes;
        SerialPort connection;
        SerialDataReceivedEventHandler handler;
        UInt64 count = 0;
        public void Connect(String port)
        {
            try
            {
                connection = new SerialPort();
                connection.PortName = port;
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
            inData = new SByte[6];
            tempBytes = new Byte[6];
            outData = new Byte[3];
            outData[0] = 1;
            connection.DataReceived += (handler = new SerialDataReceivedEventHandler(Initialize));
        }

        void Initialize(object sender, SerialDataReceivedEventArgs e)
        {
            connection.DataReceived -= handler;
            connection.DataReceived += handler = new SerialDataReceivedEventHandler(PollData);
            removeStartupData();
        }

        public void Disconnect()
        {
            connection.DataReceived -= PollData;
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

        private void PollData(object sender, SerialDataReceivedEventArgs e)
        {
            count++;
            Thread.Sleep(20);
            messLength = connection.ReadByte();
            connection.ReadByte();//remove header
            try { connection.Read(tempBytes, 0, messLength); }
            catch (TimeoutException) { return; }
            inData = (SByte[])(Array)tempBytes;
            Trace.WriteLine(count.ToString() + ": y:" + yTilt.ToString() + ", z:" + zTilt.ToString() + ", snd:" + soundVol.ToString() + ", left:" + left.ToString() + ", right:" + right.ToString());
            Trace.Flush();
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
    }


}