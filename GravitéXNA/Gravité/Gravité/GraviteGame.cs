using PloobsEngine.Engine;
using PloobsEngine.SceneControl;
using PloobsEngine.Engine.Logger;
using System;
using Microsoft.Xna.Framework;


namespace Gravite
{
    /// <summary>
    /// Engine entry point
    /// </summary>
    public class GraviteGame
    {
        public GraviteGame(Resolution resolution)
        {
            InitialEngineDescription desc = InitialEngineDescription.Default();
            desc.BackBufferWidth = resolution.Width;
            desc.BackBufferHeight = resolution.Height;
            desc.isFullScreen = resolution.IsFullscreen;
            desc.isFixedGameTime = true;
            desc.useMipMapWhenPossible = true;
            desc.UnhandledException_Handler = UnhandledException;
            System.Threading.Thread gameThread = new System.Threading.Thread(() =>
            {
                EngineStuff engine = new EngineStuff(ref desc, LoadScreen);
                engine.Run();
            });
            gameThread.Start();
            gameThread.Join();
            
        }

        static void LoadScreen(ScreenManager manager)
        {
            manager.AddScreen(new GraviteMain());
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}




