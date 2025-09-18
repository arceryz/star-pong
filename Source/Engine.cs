using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Framework;
using StarPong.Scenes;
using StarPong.Source.Framework;

namespace StarPong
{
	public enum SceneName
	{
		MenuScene,
		PlayingScene,
		EndScene,
	}

	public class Engine : Microsoft.Xna.Framework.Game
    {
        public static Engine Instance;
		public static int GameWidth = 1280;
        public static int GameHeight = 720;
        public static float Time = 0;
        public static bool EnableDebugDraw = false;
        public static bool EnableTreeDraw = false;
        public static SceneName ActiveScene { get; private set; }
        public static SpriteFont DebugFont;
		public static Dictionary<string, Object> CustomAssets = new();
        public static Rect2 Viewport = Rect2.Zero;

		// Internal.
		GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Input input = new();
        SceneTree sceneTree = new();
        Physics physics = new();
        DrawSorter drawSorter = new();
        RenderTarget2D renderTarget;

        #region Core
        public Engine()
        {
            Instance = this;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameWidth;
            graphics.PreferredBackBufferHeight = GameHeight;
            Window.AllowUserResizing = true;

			Content.RootDirectory = "Content";
            IsMouseVisible = true;
		}

        protected override void Initialize()
        {
			base.Initialize();
			ChangeScene(SceneName.MenuScene);
		}

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
			renderTarget = new RenderTarget2D(GraphicsDevice, GameWidth, GameHeight, false, SurfaceFormat.Color, DepthFormat.None);

			DebugFont = Load<SpriteFont>(AssetPaths.Font.Debug_Kobe_TTF);
            CustomAssets[AssetPaths.Font.Gyruss_Gold] = new ImageFont(AssetPaths.Font.Gyruss_Gold, "abcdefghijklmnopqrstuvwxyz0123456789-");
            CustomAssets[AssetPaths.Font.Gyruss_Grey] = new ImageFont(AssetPaths.Font.Gyruss_Grey, "abcdefghijklmnopqrstuvwxyz0123456789-");
			CustomAssets[AssetPaths.Font.Gyruss_Bronze] = new ImageFont(AssetPaths.Font.Gyruss_Bronze, "abcdefghijklmnopqrstuvwxyz0123456789-");
		}

        protected override void Update(GameTime gameTime)
        {
			Time = (float)gameTime.TotalGameTime.TotalSeconds;
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			input.Update();
            if (Input.IsKeyPressed(Keys.Z)) EnableDebugDraw = !EnableDebugDraw;
            if (Input.IsKeyHeld(Keys.Escape)) Exit();
            if (Input.IsKeyPressed(Keys.T)) EnableTreeDraw = !EnableTreeDraw;
            if (Input.IsKeyPressed(Keys.F)) graphics.ToggleFullScreen();

			physics.Update(delta);
			sceneTree.Update(delta);
			sceneTree.UpdateTransform();
		}

        protected override void Draw(GameTime gameTime)
        {
			// First we render to the render target we created,
			// then we scale this to the screen properly later.
			ComputeViewport();
			GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);

            // The scene tree will update the draw layers in a hierarchical fashion.
            // This way, objects will be sorted by both global draw layer and child index.
			drawSorter.ResetLayers();
            sceneTree.QueueDrawObjects();
			drawSorter.DrawLayers(spriteBatch);
            if (EnableDebugDraw) drawSorter.DebugDrawLayers(spriteBatch);
            if (EnableTreeDraw) sceneTree.DebugDrawTree(spriteBatch);
            spriteBatch.DrawString(DebugFont, "Press (Z) to toggle debug shapes | Press (T) to toggle tree view | Press (Esc) to quit",
                GetAnchor(-1, 1, 10, -23), Color.DarkGray with { A = 50 });
            spriteBatch.End();

			// Rescale the render target to the desired screen size.
			// This will maintain aspect ratio and add black bars if needed.
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
			spriteBatch.Draw(renderTarget, Viewport.ToRectangle(), Color.White);
			spriteBatch.End();
		}
		#endregion

		#region Utility

		public static void DebugDrawRect(Rect2 rect, Color color)
        {
            Primitives2D.DrawRectangle(Instance.spriteBatch, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), color);
        }

		public static void ChangeScene(SceneName scene)
		{
            if (SceneTree.Instance.Root != null)
            {
                SceneTree.Instance.Root.QueueFree();
            }
			ActiveScene = scene;

            GameObject obj = null;
            if (scene == SceneName.MenuScene) obj = new MenuScene();
            else if (scene == SceneName.PlayingScene) obj = new PlayingScene();
            Instance.sceneTree.SetRoot(obj);
		}

        public static Vector2 GetAnchor(float xs, float ys, float xo = 0, float yo = 0)
        {
            return new Vector2(Engine.GameWidth * (xs + 1) / 2 + xo, Engine.GameHeight * (ys + 1) / 2 + yo);
        }

        /// <summary>
        /// Load an asset from the content pipeline or a custom asset loaded by us.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path)
        {
            if (CustomAssets.ContainsKey(path))
            {
                return (T)CustomAssets[path];
			}
			return Instance.Content.Load<T>(path);
        }
        #endregion

        #region Viewport
        public void ComputeViewport()
        {
			// Calculate scaling rectangle to maintain aspect ratio.
			// By setting the scale to the smallest of the two ratios, we ensure the game fits on the screen.
			// Black bars will be present on the sides that don't fit.
			float windowWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
			float windowHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
			float scale = Math.Min(windowWidth / GameWidth, windowHeight / GameHeight);

			// The viewport is centered on the screen.
			int vpWidth = (int)(GameWidth * scale);
			int vpHeight = (int)(GameHeight * scale);
			int vpX = (int)((windowWidth - vpWidth) / 2);
			int vpY = (int)((windowHeight - vpHeight) / 2);
			Viewport = new Rect2(vpX, vpY, vpWidth, vpHeight);
		}
        #endregion
    }
}
