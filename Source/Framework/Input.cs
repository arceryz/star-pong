using Microsoft.Xna.Framework.Input;

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
	}
}
