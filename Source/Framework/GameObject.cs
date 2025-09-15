using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Source.Framework
{
	public class GameObject
	{
		public GameObject Parent { get; private set; }
		public List<GameObject> Children { get; private set; } = new();

		public Vector2 Position = Vector2.Zero;
		public int GlobalDrawLayer { get; private set; } = 0;
		public int DrawLayer = 0;

		public virtual void Initialize() { }
		public virtual void Update(float delta) { }
		public virtual void DebugDraw(SpriteBatch spriteBatch) { }
		public virtual void Draw(SpriteBatch batch) {}

		public void UpdateHierarchy(float delta)
		{
			Update(delta);
			foreach (GameObject child in Children)
			{
				child.UpdateHierarchy(delta);
			}
		}

		public void DrawHierarchy(SpriteBatch batch)
		{
			if (Parent != null)
			{
				GlobalDrawLayer = DrawLayer + Parent.GlobalDrawLayer;
			}
			else
			{
				GlobalDrawLayer = DrawLayer;
			}

			Draw(batch);
			DebugDraw(batch);
			foreach (GameObject child in Children)
			{
				child.DrawHierarchy(batch);
			}
		}

		public void QueueFree()
		{
			SceneTree.Instance.QueueFree(this);
		}

		public void AddChild(GameObject child)
		{
			Children.Add(child);
			child.Parent = this;
			child.Initialize();
		}

		public void DrawTexture(SpriteBatch batch, Texture2D texture, Vector2 position, Color color)
		{
			batch.Draw(texture, position, null, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, GlobalDrawLayer);
		}

		public void DrawString(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color, bool centered=true)
		{
			Vector2 offset = centered ? 0.5f * font.MeasureString(text) : Vector2.Zero;
			batch.DrawString(font, text, Position - offset, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, GlobalDrawLayer);
		}
	}
}
