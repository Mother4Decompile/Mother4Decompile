using System;
using Carbine.Graphics;
using Carbine.Input;
using Mother4.Data;
using Rufini.Strings;
using SFML.System;

namespace Mother4.GUI.OverworldMenu
{
	// Token: 0x02000097 RID: 151
	internal class MainMenu : MenuPanel
	{
		// Token: 0x06000312 RID: 786 RVA: 0x00013BE8 File Offset: 0x00011DE8
		public MainMenu() : base(ViewManager.Instance.FinalTopLeft + MainMenu.PANEL_POSITION, MainMenu.PANEL_SIZE, 0)
		{
			this.mainList = new ScrollingList(new Vector2f(8f, 0f), 1, MainMenu.MAIN_ITEMS, MainMenu.MAIN_ITEMS.Length, 14f, MainMenu.PANEL_SIZE.X - 14f, MainMenu.CURSOR_PATH);
			base.Add(this.mainList);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00013C62 File Offset: 0x00011E62
		public override void AxisPressed(Vector2f axis)
		{
			if (axis.Y < 0f)
			{
				this.mainList.SelectPrevious();
				return;
			}
			if (axis.Y > 0f)
			{
				this.mainList.SelectNext();
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00013C9C File Offset: 0x00011E9C
		public override object ButtonPressed(Button button)
		{
			int? num = null;
			if (button == Button.A)
			{
				num = new int?(this.mainList.SelectedIndex);
			}
			else if (button == Button.B || button == Button.Start)
			{
				num = new int?(-1);
			}
			return num;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00013CDE File Offset: 0x00011EDE
		public override void Focus()
		{
			this.mainList.Focused = true;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00013CEC File Offset: 0x00011EEC
		public override void Unfocus()
		{
			this.mainList.Focused = false;
		}

		// Token: 0x0400048B RID: 1163
		public const int PANEL_DEPTH = 0;

		// Token: 0x0400048C RID: 1164
		private static readonly string CURSOR_PATH = Paths.GRAPHICS + "cursor.dat";

		// Token: 0x0400048D RID: 1165
		public static readonly Vector2f PANEL_POSITION = new Vector2f(4f, 4f);

		// Token: 0x0400048E RID: 1166
		public static readonly Vector2f PANEL_SIZE = new Vector2f(73f, 57f);

		// Token: 0x0400048F RID: 1167
		private static readonly string[] MAIN_ITEMS = new string[]
		{
			StringFile.Instance.Get("menu.goods").Value,
			StringFile.Instance.Get("menu.psi").Value,
			StringFile.Instance.Get("menu.status").Value,
			StringFile.Instance.Get("menu.map").Value
		};

		// Token: 0x04000490 RID: 1168
		private ScrollingList mainList;
	}
}
