using Microsoft.Xna.Framework.Input;

namespace StarPong.Framework
{
	public abstract class InputEvent
	{
		public abstract bool IsHeld();
	}

	public class IEKey : InputEvent
	{
		Keys key;
		public IEKey(Keys key)
		{
			this.key = key;
		}

		public override bool IsHeld()
		{
			return Input.IsKeyHeld(key);
		}
	}

	public class IEButton : InputEvent
	{
		Buttons button;
		int gamepadIndex;

		public IEButton(Buttons button, int gamepadIndex)
		{
			this.button = button;
			this.gamepadIndex = gamepadIndex;
		}

		public override bool IsHeld()
		{
			return Input.IsGamepadButtonHeld(button, gamepadIndex);
		}
	}
}
