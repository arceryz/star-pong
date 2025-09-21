namespace StarPong
{
	/// <summary>
	/// The only purpose of this class is to assign file paths to variables,
	/// in that way, file changes can be made easily by refactoring the variable name in the IDE.
	/// As a bonus, there is autocomplete for all the asset categories!
	/// </summary>
	public static class Assets
	{
		public static class Textures
		{
			public static string BG_Planets = "Background/Planets";
			public static string BG_Asteroids = "Background/Asteroids";
			public static string BG_Stars = "Background/Stars";

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

			public static string Cat = "Debug/Cat";
		}

		public static class Fonts
		{
			public static string Pixel = "UI/Pixel";
			public static string Gyruss_Grey = "UI/Gyruss_Grey";
			public static string Gyruss_Gold = "UI/Gyruss_Gold";
			public static string Gyruss_Bronze = "UI/Gyruss_Bronze";
			public static string Debug_Kobe_TTF = "Debug/Kobe";
		}

		public static class Songs
		{
			public static string Battle_Normal = "Music/Battle_Normal";
			public static string Battle_Critical = "Music/Battle_Critical";
			public static string Menu1 = "Music/Menu1";
			public static string Menu2 = "Music/Menu2";
		}

		public static class Sounds
		{
			public static string Bullet_Fired = "SFX/Bullet_Fired";
			public static string Bullet_Impact = "SFX/Bullet_Impact";
			public static string Explosion = "SFX/Explosion";
			public static string Shield_Activate = "SFX/Shield_Activate";
			public static string Shield_Deactivate = "SFX/Shield_Deactivate";
			public static string Shield_Deflect = "SFX/Shield_Deflect";
			public static string Shield_Loop = "SFX/Shield_Loop";

			public static string UI_Button_Click = "SFX/Button_Click";
			public static string UI_Button_Hover = "SFX/Button_Hover";
			public static string UI_Scene_Transition = "SFX/Scene_Transition";
			public static string Mother_Alarm = "SFX/Mother_Alarm";
		}

		public static class Shaders
		{
			public static string CRT = "Shaders/CRT";
		}
	}
}
