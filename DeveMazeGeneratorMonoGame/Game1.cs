#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using DeveMazeGenerator;
using DeveMazeGenerator.Generators;
#endregion

namespace DeveMazeGeneratorMonoGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera camera;

        private BasicEffect effect;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        private int curMazeWidth = 32;
        private int curMazeHeight = 32;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            if (!true)
            {
                graphics.PreferredBackBufferWidth = 1700;
                graphics.PreferredBackBufferHeight = 900;
            }
            else
            {
                graphics.PreferredBackBufferWidth = 2560;
                graphics.PreferredBackBufferHeight = 1440;
                graphics.IsFullScreen = true;
            }

            GenerateMaze();

            Content.RootDirectory = "Content";

            camera = new Camera(this);

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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);

            ContentDing.GoLoadContent(GraphicsDevice, Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void GenerateMaze()
        {
            if (indexBuffer != null)
                indexBuffer.Dispose();
            if (vertexBuffer != null)
                vertexBuffer.Dispose();



            var alg = new AlgorithmBacktrack();
            var maze = alg.Generate(curMazeWidth, curMazeHeight, InnerMapType.BitArreintjeFast, null);
            var walls = maze.GenerateListOfMazeWalls();


            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[walls.Count * 8];
            int[] indices = new int[walls.Count * 12];

            int curVertice = 0;
            int curIndice = 0;


            foreach (var wall in walls)
            {
                //int factorHeight = 10;
                //int factorWidth = 10;

                WallModel model = new WallModel(this, wall);

                model.GoGenerateVertices(vertices, indices, ref curVertice, ref curIndice);

            }

            vertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);

            vertexBuffer.SetData(vertices);
            indexBuffer.SetData(indices);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputDing.PreUpdate();

            if (InputDing.CurKey.IsKeyDown(Keys.Escape))
                Exit();


            camera.Update(gameTime);


            if (InputDing.KeyDownUp(Keys.Up))
            {
                curMazeWidth *= 2;
                curMazeHeight *= 2;
                GenerateMaze();
            }

            if (InputDing.KeyDownUp(Keys.Down))
            {
                curMazeWidth /= 2;
                curMazeHeight /= 2;
                GenerateMaze();
            }

            if (InputDing.KeyDownUp(Keys.R))
            {
                GenerateMaze();
            }



            InputDing.AfterUpdate();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);




            //GraphicsDevice.BlendState = BlendState.Opaque;

            //DepthStencilState d = new DepthStencilState();
            //d.DepthBufferEnable = true;
            //GraphicsDevice.DepthStencilState = d;

            Matrix worldMatrix = Matrix.Identity;
            effect.World = worldMatrix;
            effect.View = camera.viewMatrix;
            effect.Projection = camera.projectionMatrix;

            //effect.EnableDefaultLighting();
            effect.LightingEnabled = true;
            effect.EmissiveColor = new Vector3(0.25f, 0.25f, 0.25f);
            effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            effect.SpecularPower = 0.1f;

            effect.AmbientLightColor = new Vector3(0.25f, 0.25f, 0.25f);
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = new Vector3(1, -1, -1);
            effect.DirectionalLight0.DiffuseColor = new Vector3(0.75f, 0.75f, 0.75f);
            effect.DirectionalLight0.SpecularColor = new Vector3(0.1f, 0.1f, 0.1f);

            effect.World = Matrix.Identity;
            effect.TextureEnabled = true;


            //Skybox
            effect.LightingEnabled = false;
            effect.Texture = ContentDing.skyTexture1;
            int skyboxSize = 1000000;
            CubeModelInvertedForSkybox skybox = new CubeModelInvertedForSkybox(this, skyboxSize, skyboxSize, skyboxSize, TexturePosInfoGenerator.FullImage);
            Matrix skyboxMatrix = Matrix.CreateTranslation(camera.cameraPosition) * Matrix.CreateTranslation(new Vector3(-skyboxSize / 2, -skyboxSize / 2, -skyboxSize / 2));
            skybox.Draw(skyboxMatrix, effect);
            effect.LightingEnabled = true;




            //effect.Texture = ContentDing.wallTexture;


            //SamplerState newSamplerState = new SamplerState()
            //{
            //    AddressU = TextureAddressMode.Wrap,
            //    AddressV = TextureAddressMode.Wrap,
            //    Filter = TextureFilter.Point
            //};
            //GraphicsDevice.SamplerStates[0] = newSamplerState;

            //foreach (WallModel wallModel in wallModels)
            //{
            //    GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            //    wallModel.Draw(Matrix.Identity, effect);
            //}


            //Ground


            int mazeScale = 10;

            effect.World = Matrix.CreateScale(mazeScale);

            //effect.Texture = ContentDing.grasTexture;

            //SamplerState newSamplerState2 = new SamplerState()
            //{
            //    AddressU = TextureAddressMode.Wrap,
            //    AddressV = TextureAddressMode.Wrap,
            //    Filter = TextureFilter.Point
            //};
            //GraphicsDevice.SamplerStates[0] = newSamplerState2;


            //int curmazeheight = 100;

            //WallModel wallmodel = new WallModel(this, curmazeheight * 10, curmazeheight * 10, TexturePosInfoGenerator.FullImageWithSize(15), Matrix.Identity);
            //wallmodel.Draw(Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateTranslation(0, 0, curmazeheight * 10), effect);

            effect.Texture = ContentDing.grasTexture;

            CubeModel ground = new CubeModel(this, curMazeWidth, 1, curMazeHeight, TexturePosInfoGenerator.FullImage);
            ground.Draw(Matrix.CreateTranslation(0, -1, 0) * effect.World, effect);

            effect.World = Matrix.CreateScale(mazeScale);

            if (vertexBuffer != null && indexBuffer != null)
            {
                GraphicsDevice.RasterizerState = RasterizerState.CullNone;

                GraphicsDevice.Indices = indexBuffer;
                GraphicsDevice.SetVertexBuffer(vertexBuffer);

                effect.Texture = ContentDing.wallTexture;



                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
                }

            }





            //CubeModel redCube = new CubeModel(this, 5, 5, 5, TexturePosInfoGenerator.FullImage);
            //effect.Texture = textureRed;

            //foreach (var node in currentPath)
            //{
            //    Matrix m = Matrix.CreateTranslation(2.5f + 10f * node.X, 2.5f, 2.5f + 10f * node.Y);
            //    redCube.Draw(m, effect);
            //}


            //spriteBatch.Begin();

            //spriteBatch.DrawString(ContentDing.spriteFont, "Size: " + curMazeWidth, new Vector2(10, 10), Color.White);

            //spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
