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
		bool shiftModifier;

		public IEKey(Keys key, bool shiftModifier=false)
		{
			this.key = key;
			this.shiftModifier = shiftModifier;
		}

		public override bool IsHeld()
		{
			return Input.IsKeyHeld(key) && (!shiftModifier || Input.IsKeyHeld(Keys.LeftShift));
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

	public class IEMouseButton : InputEvent
	{
		MouseButton button;
		public IEMouseButton(MouseButton button)
		{
			this.button = button;
		}

		public override bool IsHeld()
		{
			return Input.IsMouseButtonHeld(button);
		}
	}
}
