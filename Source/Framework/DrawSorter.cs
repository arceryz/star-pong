using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

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
		public static DrawSorter Instance;
		Dictionary<int, List<GameObject>> drawLayers;

		public DrawSorter()
		{
			Instance = this;
		}

		public void ResetLayers()
		{
			drawLayers = new();
		}

		public void DrawLayers(SpriteBatch batch)
		{
			foreach (int layer in drawLayers.Keys)
			{
				List<GameObject> objects = drawLayers[layer];
				foreach (GameObject obj in objects) obj.Draw(batch);
			}
		}

		public void DebugDrawLayers(SpriteBatch batch)
		{
			foreach (int layer in drawLayers.Keys)
			{
				List<GameObject> objects = drawLayers[layer];
				foreach (GameObject obj in objects) obj.DebugDraw(batch);
			}
		}

		public void QueueDrawObject(GameObject obj)
		{
			if (!drawLayers.ContainsKey(obj.GlobalDrawLayer))
			{
				drawLayers[obj.GlobalDrawLayer] = new List<GameObject>([obj]);
			}
			else
			{
				drawLayers[obj.GlobalDrawLayer].Add(obj);
			}
		}
	}
}
