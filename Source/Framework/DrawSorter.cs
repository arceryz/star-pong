using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;

namespace StarPong.Framework
{
	/// <summary>
	/// This class is responsible for sorting the draw calls made by game objects.
	/// The scene tree collects queues the draw calls, which builds a hierarchy of draw layers
	/// and parent/child relationships in here.
	/// The result is that draw order respects both the draw layer of children as well as
	/// the order in which children we're added if they are on the same layer.
	/// </summary>
	public class DrawSorter
	{
		public const int TopLayer = 32;
		public static DrawSorter Instance;
		List<GameObject>[] drawLayers;

		public DrawSorter()
		{
			Instance = this;
		}

		public void ResetLayers()
		{
			drawLayers = new List<GameObject>[TopLayer+1];
			for (int i = 0; i < drawLayers.Length; i++) drawLayers[i] = new List<GameObject>();
		}
	

		public void DrawLayers(SpriteBatch batch)
		{
			foreach (List<GameObject> layer in drawLayers)
			{
				foreach (GameObject obj in layer) obj.Draw(batch);
			}
		}

		public void DebugDrawLayers(SpriteBatch batch)
		{
			foreach (List<GameObject> layer in drawLayers)
			{
				foreach (GameObject obj in layer) obj.DebugDraw(batch);
			}
		}

		public void QueueDrawObject(GameObject obj)
		{
			drawLayers[Math.Min(TopLayer, obj.GlobalDrawLayer)].Add(obj);
		}
	}
}
