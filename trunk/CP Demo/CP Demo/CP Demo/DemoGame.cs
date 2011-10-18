﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Demo
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DemoGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public bool rightIsOn;
        public bool leftIsOn;

        UI.ProgressBar progressBar;
        SpriteFont spriteFont;
        Vector2 textPos;
        Texture2D texture;
        public Vector2 position;
        public DemoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            position = new Vector2(0, 0);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("Arial");

            textPos = new Vector2(10, 10);

            progressBar = new UI.ProgressBar(this, new Rectangle(10,30, 300, 16));
            progressBar.minimum = 0;
            progressBar.maximum = 100;


            texture = Content.Load<Texture2D>("image");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            position.X += Program.receiver.yTilt / 10;
            position.Y += Program.receiver.zTilt / 10;
            leftIsOn = Program.receiver.left;
            rightIsOn = Program.receiver.right;
            progressBar.value = Program.receiver.soundVol;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(spriteFont, "left: " + leftIsOn.ToString() + ", right: " + rightIsOn.ToString(),textPos,Color.Black);
            progressBar.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
