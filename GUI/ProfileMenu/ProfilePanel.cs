using System;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Maps;
using Mother4.Data;
using Rufini.Strings;
using SFML.System;

namespace Mother4.GUI.ProfileMenu
{
	// Token: 0x0200004F RID: 79
	internal class ProfilePanel : MenuPanel
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0000B56C File Offset: 0x0000976C
		public ProfilePanel(Vector2f position, Vector2f size, int index, SaveProfile profile) : base(position, size, 0, WindowBox.Style.Normal, (uint)profile.Flavor)
		{
			TextRegion control = new TextRegion(new Vector2f(1f, -3f), 0, Fonts.Main, string.Format("#{0}", index + 1));
			base.Add(control);
			if (profile.IsValid)
			{
				this.SetupForFile(profile);
				return;
			}
			TextRegion control2 = new TextRegion(new Vector2f(16f, 10f), 0, Fonts.Main, StringFile.Instance.Get("system.noSaveData").Value);
			base.Add(control2);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000B60C File Offset: 0x0000980C
		private void SetupForFile(SaveProfile profile)
		{
			int num = 0;
			int num2 = Math.Min(4, profile.Party.Length);
			for (int i = 0; i < num2; i++)
			{
				Graphic graphic = new IndexedColorGraphic(CharacterGraphics.GetFile(profile.Party[i]), "walk south", new Vector2f((float)(24 + num), this.size.Y - 2f), 0);
				graphic.SpeedModifier = 0f;
				num += 4 + (int)graphic.Size.X;
				base.Add(graphic);
			}
			string[] array = MapLoader.LoadTitle(Paths.MAPS + profile.MapName);
			TextRegion control = new TextRegion(new Vector2f(128f, -3f), 0, Fonts.Main, array[0]);
			base.Add(control);
			TextRegion control2 = new TextRegion(new Vector2f(128f, 12f), 0, Fonts.Main, array[1]);
			base.Add(control2);
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)profile.Time);
			TextRegion textRegion = new TextRegion(default(Vector2f), 0, Fonts.Main, string.Format("{0:00}:{1:00}:{2:00}", (int)timeSpan.TotalHours, (int)timeSpan.TotalMinutes % 60, (int)timeSpan.TotalSeconds % 60));
			textRegion.Position = new Vector2f((float)((int)(this.size.X - textRegion.Size.X) - 2), -3f);
			base.Add(textRegion);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000B792 File Offset: 0x00009992
		public override object ButtonPressed(Button b)
		{
			return null;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000B795 File Offset: 0x00009995
		public override void AxisPressed(Vector2f axis)
		{
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000B797 File Offset: 0x00009997
		public override void Focus()
		{
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000B799 File Offset: 0x00009999
		public override void Unfocus()
		{
		}

		// Token: 0x040002B3 RID: 691
		private const int DEPTH = 0;

		// Token: 0x040002B4 RID: 692
		private const int PLAYER_CHARACTER_COUNT = 4;
	}
}
