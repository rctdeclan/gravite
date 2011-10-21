using PloobsEngine.SceneControl;
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
namespace Gravite
{
    /// <summary>
    /// Basic Deferred Scene
    /// </summary>
    public class GraviteMain : IScene
    {

        CameraStatic cam;
        IObject ball;
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
            TerrainObject to = new TerrainObject(factory, "Tex\\Untitled", Vector3.Zero, Matrix.Identity, new MaterialDescription(1f,2f,0f), 2, 1);
            ///Create the Model using the Terrain Object. Here we pass the textures used, in our case we are using MultiTextured Terrain so we pass lots of textures
            TerrainModel stm = new TerrainModel(factory, to, "TerrainName", "Tex\\Terraingrass", "Tex\\rock", "Tex\\sand", "Tex\\snow");
            ///Create the shader
            ///In this sample we passed lots of textures, each one describe a level in the terrain, the ground is the sand and grass. the hills are rocks and the "mountains" are snow
            ///They are interpolated in the shader, you can control how using the shader parameters exposed in the DeferredTerrainShader
            DeferredTerrainShader shader = new DeferredTerrainShader(TerrainType.MULTITEXTURE);
            ///the classic material
            DeferredMaterial mat = new DeferredMaterial(shader);
            IObject obj3 = new IObject(mat, stm, to);
            this.World.AddObject(obj3);

            ///Create a Simple Model
            SimpleModel model = new SimpleModel(GraphicFactory, "Ball", "Tex\\DebugTexture");///Create a Physic Object
            //model.SetTexture(GraphicFactory.CreateTexture2DColor(1, 1, Color.Pink), TextureType.DIFFUSE);
            IPhysicObject pobj = new SphereObject(new Vector3(0,30,0), 10, 100, 1, new MaterialDescription(1f,2f,0.2f));
            //pobj.isMotionLess = true;
            ///Create a shader   
            IShader nd = new DeferredNormalShader();

            ///Create a Material                
            IMaterial material = new DeferredMaterial(nd);
            ///Create a an Object that englobs everything and add it to the world
            ball = new IObject(material, model, pobj);
            this.World.AddObject(ball);
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
            cam = new CameraStatic(new Vector3(-150, 60, 0),new Vector3(0,30,0));
            
            this.World.CameraManager.AddCamera(cam);
        }

        float yDiff = 0;
        float zDiff = 0;

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //yDiff += Program.receiver.yTilt;
            //zDiff += Program.receiver.zTilt;
            ball.PhysicObject.AngularVelocity = new Vector3(Program.receiver.yTilt/10, 0, Program.receiver.zTilt/10);
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

