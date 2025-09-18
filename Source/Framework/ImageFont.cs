using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Framework
{
	/// <summary>
	/// This class loads a font encoded as a spritesheet and then draws text by
	/// slapping multiple draw calls in a row. 
	/// </summary>
	public class ImageFont
	{
		public int CharacterWidth { get; private set; }
		public int CharacterHeight { get; private set; }
		Dictionary<char, Rectangle> sourceRects = new();
		Texture2D sheet;

		public ImageFont(string assetPath, string characters)
		{
			sheet = Engine.Load<Texture2D>(assetPath);
			CharacterWidth = sheet.Width / characters.Length;
			CharacterHeight = sheet.Height;
			Debug.WriteLine($"Image font size {(float)sheet.Width/ 8}, {CharacterHeight} with {characters.Length}");
			for (int i = 0; i < characters.Length; i++)
			{
				sourceRects[characters[i]] = new Rectangle(i * CharacterWidth, 0, CharacterWidth, CharacterHeight);
			}
		}

		public void DrawString(SpriteBatch batch, Vector2 position, string text, float scale=1, bool centered=true)
		{
			Vector2 pos = position;
			if (centered)
			{
				pos -= 0.5f * new Vector2(CharacterWidth * text.Length, CharacterHeight) * scale;
			}
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (sourceRects.ContainsKey(c))
				{
					Rectangle rect = sourceRects[c];
					batch.Draw(sheet, pos + new Vector2(CharacterWidth * i, 0) * scale, rect, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
				}
			}
		}
	}
}
