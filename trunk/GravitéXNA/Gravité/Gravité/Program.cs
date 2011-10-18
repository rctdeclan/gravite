using System;
using CP;
using Microsoft.Xna;
namespace Gravite
{
    static class Program
    {
        public static CPReceiverV3Optima receiver;
        static void Main(string[] args)
        {
            receiver = new CPReceiverV3Optima();
            try
            {
                receiver.Connect("COM21");
            }
            catch (System.IO.IOException) { return; }
            EngineSetup engine = new EngineSetup();          
        }
    }
}

