using System.Collections.Generic;
using System.Diagnostics;

namespace StarPong.Framework
{
	/// <summary>
	/// Applies collision and overlap detection to objects in the scene tree.
	/// </summary>
	public class Physics
	{
		public static Physics Instance;

		public Physics()
		{
			Instance = this;
		}

		public void Update(float delta)
		{
			List<GameObject> objects = SceneTree.Instance.GetObjectsInGroup("physics");
			foreach (GameObject obj in objects)
			{
				Debug.WriteLine(obj);
			}
			Debug.WriteLine($"{objects.Count} objects in physics group");
		}
	}
}
