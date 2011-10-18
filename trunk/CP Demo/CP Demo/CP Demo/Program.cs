using System;
using CP;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna;
using System.IO;

namespace Demo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static DemoGame game;
        public static CPReceiverV3Optima receiver;
        static void Main(string[] args)
        {
            //EngineSetup game = new EngineSetup();
            receiver = new CPReceiverV3Optima();
            try
            {
                receiver.Connect("COM14");
            }
            catch (IOException)
            {
                return;
            }
            game = new DemoGame();
            game.Exiting += new EventHandler<EventArgs>(game_Exiting);
            game.Run();
        }

        static void game_Exiting(object sender, EventArgs e)
        {
            receiver.StartMotor();
            Thread.Sleep(1000);
            receiver.StopMotor();
            receiver.Disconnect();
            Environment.Exit(1);
        }
    }
#endif
}

