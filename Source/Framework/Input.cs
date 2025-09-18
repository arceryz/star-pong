using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace StarPong.Framework
{
	public class Input
	{
		static Input Instance;

		KeyboardState prevState = new();
		KeyboardState currentState = new();

		public Input() 
		{
			Instance = this;
		}

		public void Update()
		{
			prevState = currentState;
			currentState = Keyboard.GetState();
		}

		public static bool IsKeyPressed(Keys key)
		{
			return !Instance.prevState.IsKeyDown(key) && Instance.currentState.IsKeyDown(key);
		}
		
		public static bool IsKeyHeld(Keys key)
		{
			return Instance.currentState.IsKeyDown(key);
		}

		public static Vector2 GetMousePosition()
		{
			MouseState ms = Mouse.GetState();
			Vector2 pos = new Vector2(ms.Position.X, ms.Position.Y);
			Vector2 orig = Engine.Viewport.GetTL();

			// Take relative coordinates to the true game window, then rescale to game width.
			Vector2 mpos = Vector2.Zero;
			mpos.X = (pos.X - orig.X) / Engine.Viewport.Width * Engine.GameWidth;
			mpos.Y = (pos.Y - orig.Y) / Engine.Viewport.Height * Engine.GameHeight;

			return mpos;
		}
	}
}
