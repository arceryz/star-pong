namespace StarPong
{
	/// <summary>
	/// The only purpose of this class is to assign file paths to variables,
	/// in that way, file changes can be made easily by refactoring the variable name in the IDE.
	/// As a bonus, there is autocomplete for all the asset categories!
	/// </summary>
	public static class AssetPaths
	{
		public static class Texture
		{
			public static string BG_Asteroids_Mid = "Background/BG_Asteroids_Mid";
			public static string BG_Asteroids_Close = "Background/BG_Asteroids_Close";
			public static string BG_Stars = "Background/BG_Stars";

			public static string Blue_Mothership = "Mothership/Blue_Mother";
			public static string Red_Mothership = "Mothership/Red_Mother";

			public static string Bomb = "Bomb/Bomb";

			public static string MuzzleFlash = "Player/Muzzle_Flash";
			public static string Blue_Player = "Player/Blue_Ship";
			public static string Blue_Shield = "Player/Blue_Shield";
			public static string Blue_Bullet = "Player/Blue_Bullet";

			public static string Red_Player = "Player/Red_Ship";
			public static string Red_Shield = "Player/Red_Shield";
			public static string Red_Bullet = "Player/Red_Bullet";

			public static string UI_SelectionArrows = "UI/Selection_Arrows";
			public static string UI_Red_Portrait = "UI/Red_Portrait";
			public static string UI_Blue_Portrait = "UI/Blue_Portrait";
			public static string UI_HealthPip = "UI/HealthPip";
			public static string UI_EnergyPip = "UI/EnergyPip";

			public static string ExplosionFX_Big = "FX/Explosion_Big";
			public static string ExplosionFX_Small = "FX/Explosion_Small";
			public static string FireFX_Big = "FX/Fire_Big";
			public static string FireFX_Small = "FX/Fire_Small";
		}

		public static class Font
		{
			public static string Pixel = "UI/Pixel";
			public static string Gyruss_Grey = "UI/Gyruss_Grey";
			public static string Gyruss_Gold = "UI/Gyruss_Gold";
			public static string Gyruss_Bronze = "UI/Gyruss_Bronze";
			public static string Debug_Kobe_TTF = "Debug/Kobe";
		}

		public static class Song
		{
			public static string Battle1_Normal = "Music/Battle1_Normal";
		}
	}
}
