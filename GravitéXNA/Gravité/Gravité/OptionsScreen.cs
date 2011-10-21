using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Management;
using System.Collections;
using System.Runtime.InteropServices;
using CP;

namespace Gravite
{
    public partial class OptionsScreen : Form
    {
        public CPReceiverV4 receiver;
        bool isConnected = false;
        ArrayList ports;
        public delegate void OnClickRun(object sender, EventArgs e);
        public event OnClickRun Exit;
        public OptionsScreen()
        {
            InitializeComponent();

            runBtn.Click += new EventHandler(runButton_Click);
            connectBtn.Click += new EventHandler(connectBtn_Click);
            disconnectBtn.Click += new EventHandler(disconnectBtn_Click);
        }

        public void Initialize()
        {
            GetUSBComDevices();
            SetScreenSizes();
        }
        


        void disconnectBtn_Click(object sender, EventArgs e)
        {
            if (portsBox.SelectedItem != null && receiver != null && isConnected)
            {
                statusLbl.Text = "Disconnecting...";
                receiver.Disconnect();
                isConnected = false;

                statusLbl.Text = "Disconnected.";
            }
        }

        void connectBtn_Click(object sender, EventArgs e)
        {
            if (portsBox.SelectedItem!=null && receiver!=null && !isConnected)
            {
                try
                {
                    statusLbl.Text = "Connecting...";
                    receiver.Connect((portsBox.SelectedItem as ComPort).Id);
                    isConnected = true;
                    statusLbl.Text = "Connected.";
                }
                catch (System.IO.IOException) {
                    statusLbl.Text = "Failed.";
                }
            }
        }

        void runButton_Click(object sender, EventArgs e)
        {
            if (isConnected && sizeCombobox.SelectedIndex!=-1)
            {
                receiver.Start();
                Close();
                Exit(this,null);
            }
        }

        void SetScreenSizes()
        {
            DEVMODE vDevMode = new DEVMODE();
            int i = 0;
            while (EnumDisplaySettings(null, i, ref vDevMode))
            {
                sizeCombobox.Items.Add(new Resolution(vDevMode.dmPelsWidth, vDevMode.dmPelsHeight,1<<vDevMode.dmBitsPerPel,vDevMode.dmDisplayFrequency));
                i++;
            }
            sizeCombobox.DisplayMember = "Description";
        }

        void GetUSBComDevices()
        {
            String[] portNames = SerialPort.GetPortNames();
            ports = new ArrayList(portNames.Length);
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT *  FROM Win32_PnPEntity");

            foreach (ManagementObject mo in searcher.Get())
            {
                string i = mo["Name"].ToString();
                if (i.Contains("(COM")) 
                {
                    ComPort cp = new ComPort();
                    cp.Name = i;
                    int start;
                    String name = i.Substring(start = i.IndexOf("(COM") + 1,i.IndexOf(")",start+3)-start);
                    cp.Id = name;
                    ports.Add(cp);
                }
            }

            portsBox.DataSource = ports;
            portsBox.DisplayMember = "Name";
        }

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        const int ENUM_CURRENT_SETTINGS = -1;
        const int ENUM_REGISTRY_SETTINGS = -2;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {

            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        public Resolution GetRes()
        {
            Resolution selectedRes;
            selectedRes = (Resolution)sizeCombobox.SelectedItem;
            selectedRes.IsFullscreen = fullscreenChk.Checked;
            return selectedRes;
        }

    }



    public struct Resolution
    {
        public Resolution(int width, int height, int colors, int frequency)
        {
            this.Width = width;
            this.Height = height;
            this.Colors = colors;
            this.Frequency = frequency;
            this.IsFullscreen = false;
        }
        public int Width, Height, Colors, Frequency;
        public bool IsFullscreen;

        public string Description
        {
            get { return Width + "x" + Height + ": " + Frequency + "Hz, " + Colors + "Bit"; }
        }
    }


    class ComPort
    {
        private string id;
        private string name;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
