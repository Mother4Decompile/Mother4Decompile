using System;
using Carbine.GUI;

namespace Mother4.Data
{
	// Token: 0x020000F3 RID: 243
	internal class Settings
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x000219C9 File Offset: 0x0001FBC9
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x000219D0 File Offset: 0x0001FBD0
		public static string Locale
		{
			get
			{
				return Settings.language;
			}
			set
			{
				Settings.language = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x000219D8 File Offset: 0x0001FBD8
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x000219DF File Offset: 0x0001FBDF
		public static uint WindowFlavor
		{
			get
			{
				return Settings.windowFlavor;
			}
			set
			{
				Settings.windowFlavor = value;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x000219E7 File Offset: 0x0001FBE7
		public static WindowBox.Style WindowStyle
		{
			get
			{
				return WindowBox.Style.Normal;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x000219EA File Offset: 0x0001FBEA
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x000219F1 File Offset: 0x0001FBF1
		public static float EffectsVolume
		{
			get
			{
				return Settings.sfxVolume;
			}
			set
			{
				Settings.sfxVolume = value;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x000219F9 File Offset: 0x0001FBF9
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00021A00 File Offset: 0x0001FC00
		public static float MusicVolume
		{
			get
			{
				return Settings.bgmVolume;
			}
			set
			{
				Settings.bgmVolume = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00021A08 File Offset: 0x0001FC08
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x00021A0F File Offset: 0x0001FC0F
		public static int TextSpeed
		{
			get
			{
				return Settings.textSpeed;
			}
			set
			{
				Settings.textSpeed = value;
			}
		}

		// Token: 0x04000760 RID: 1888
		private static string language = "en_US";

		// Token: 0x04000761 RID: 1889
		private static uint windowFlavor = 0U;

		// Token: 0x04000762 RID: 1890
		private static float bgmVolume = 0f;

		// Token: 0x04000763 RID: 1891
		private static float sfxVolume = 0.9f;

		// Token: 0x04000764 RID: 1892
		private static int textSpeed = 10;
	}
}
