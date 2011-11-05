using System;
using CP;
using System.Windows.Forms;
using System.IO.Ports;

namespace Gravite
{
    static class Program
    {
        public static CPReceiverV4 receiver;
        public static OptionsScreen optionsScreen;
        public static GraviteGame game;

        public static Resolution selectedRes;

        [STAThread]
        static void Main(string[] args)
        {
            optionsScreen = new OptionsScreen();
            optionsScreen.Exit += OnClickRun;
            receiver = new CPReceiverV4();
            optionsScreen.receiver = receiver;
            optionsScreen.Initialize();
            //System.Threading.Thread gameThread = new System.Threading.Thread(() =>
            //{
                Application.Run(optionsScreen);
            //});
        }

        static void OnClickRun(object sender, EventArgs e)
        {
            selectedRes = optionsScreen.GetRes();
            game = new GraviteGame(selectedRes);
        }

        
    }
}

