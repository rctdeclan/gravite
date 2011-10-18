﻿using PloobsEngine.SceneControl;
using PloobsEngine;
using System.Collections.Generic;
using PloobsEngine.Physics;
using PloobsEngine.Input;
using PloobsEngine.Modelo;
using Microsoft.Xna.Framework;
using PloobsEngine.Material;
using PloobsEngine.Commands;
using PloobsEngine.Cameras;
using PloobsEngine.Light;
using Microsoft.Xna.Framework.Graphics;
using PloobsEngine.Utils;
using PloobsEngine.Physics.Bepu;
using PloobsEngine.Engine;
namespace ProjectTemplate
{
    /// <summary>
    /// Basic Deferred Scene
    /// </summary>
    public class GraviteMain : IScene
    {
        /// <summary>
        /// Sets the world and render technich.
        /// </summary>
        /// <param name="renderTech">The render tech.</param>
        /// <param name="world">The world.</param>
        protected override void SetWorldAndRenderTechnich(out IRenderTechnic renderTech, out IWorld world)
        {
            ///create the world using bepu as physic api and a simple culler implementation
            ///IT DOES NOT USE PARTICLE SYSTEMS (see the complete constructor, see the ParticleDemo to know how to add particle support)
            world = new IWorld(new BepuPhysicWorld(), new SimpleCuller());

            ///Create the deferred description
            DeferredRenderTechnicInitDescription desc = DeferredRenderTechnicInitDescription.Default();
            ///Some custom parameter, this one allow light saturation. (and also is a pre requisite to use hdr)
            desc.UseFloatingBufferForLightMap = true;
            ///set background color, default is black
            desc.BackGroundColor = Color.CornflowerBlue;
            ///create the deferred technich
            renderTech = new DeferredRenderTechnic(desc);
        }

        /// <summary>
        /// Load content for the screen.
        /// </summary>
        /// <param name="GraphicInfo"></param>
        /// <param name="factory"></param>
        /// <param name="contentManager"></param>
        protected override void LoadContent(PloobsEngine.Engine.GraphicInfo GraphicInfo, PloobsEngine.Engine.GraphicFactory factory, IContentManager contentManager)
        {
            ///must be called before all
            base.LoadContent(GraphicInfo, factory, contentManager);

            ///Create a Simple Model
            SimpleModel model = new SimpleModel(GraphicFactory, "ball");
            model.SetTexture(GraphicFactory.CreateTexture2DColor(1, 1, Color.Purple), TextureType.DIFFUSE);

            ///Create a Physic Object
            IPhysicObject pobj = new SphereObject(Vector3.Zero, 1, 10, 1, MaterialDescription.DefaultBepuMaterial());
            pobj.isMotionLess = true;
            ///Create a shader   
            IShader nd = new DeferredNormalShader();

            ///Create a Material                
            IMaterial material = new DeferredMaterial(nd);
            ///Create a an Object that englobs everything and add it to the world
            IObject obj = new IObject(material, model, pobj);
            this.World.AddObject(obj);
            #region Lights
            DirectionalLightPE ld1 = new DirectionalLightPE(Vector3.Left, Color.White);
            DirectionalLightPE ld2 = new DirectionalLightPE(Vector3.Right, Color.White);
            DirectionalLightPE ld3 = new DirectionalLightPE(Vector3.Backward, Color.White);
            DirectionalLightPE ld4 = new DirectionalLightPE(Vector3.Forward, Color.White);
            DirectionalLightPE ld5 = new DirectionalLightPE(Vector3.Down, Color.White);
            float li = 0.4f;
            ld1.LightIntensity = li;
            ld2.LightIntensity = li;
            ld3.LightIntensity = li;
            ld4.LightIntensity = li;
            ld5.LightIntensity = li;
            this.World.AddLight(ld1);
            this.World.AddLight(ld2);
            this.World.AddLight(ld3);
            this.World.AddLight(ld4);
            this.World.AddLight(ld5);
            #endregion
            ///Add a AA post effect
            this.RenderTechnic.AddPostEffect(new AntiAliasingPostEffect());

            CameraStatic cam = new CameraStatic();
            this.World.CameraManager.AddCamera(cam);
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="render"></param>
        protected override void Draw(GameTime gameTime, RenderHelper render)
        {
            ///must be called before
            base.Draw(gameTime, render);
            ///Draw some text to the screen
            render.RenderTextComplete("Gravite Tilt Test", new Vector2(GraphicInfo.Viewport.Width - 315, 15), Color.White, Matrix.Identity);
        }
    }
}

