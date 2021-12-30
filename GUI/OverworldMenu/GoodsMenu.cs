using System;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Data;
using Mother4.Items;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.OverworldMenu
{
	// Token: 0x02000096 RID: 150
	internal class GoodsMenu : MenuPanel
	{
		// Token: 0x06000308 RID: 776 RVA: 0x00013470 File Offset: 0x00011670
		public GoodsMenu() : base(ViewManager.Instance.FinalTopLeft + GoodsMenu.PANEL_POSITION, GoodsMenu.PANEL_SIZE, 0)
		{
			this.divider = new ShapeGraphic(new RectangleShape(new Vector2f(1f, 92f))
			{
				FillColor = new Color(128, 140, 138)
			}, new Vector2f((float)((int)(GoodsMenu.PANEL_SIZE.X / 2f)), 4f), VectorMath.ZERO_VECTOR, new Vector2f(1f, 92f), 1);
			base.Add(this.divider);
			CharacterType[] array = PartyManager.Instance.ToArray();
			this.tabs = new IndexedColorGraphic[array.Length + 1];
			this.tabLabels = new TextRegion[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				this.tabs[i] = new IndexedColorGraphic(GoodsMenu.UI_FILE, (i == this.selectedTab) ? "firsttag" : "tag", new Vector2f(-8f, -7f) + new Vector2f(50f * (float)i, 0f), (i == this.selectedTab) ? 1 : -2);
				this.tabs[i].CurrentPalette = this.GetTabPaletteIndex(i);
				base.Add(this.tabs[i]);
				this.tabLabels[i] = new TextRegion(new Vector2f(-4f, -21f) + new Vector2f(50f * (float)i, 0f), (i == this.selectedTab) ? 2 : -1, Fonts.Main, CharacterNames.GetName(array[i]));
				this.tabLabels[i].Color = ((i == this.selectedTab) ? GoodsMenu.ACTIVE_TAB_TEXT_COLOR : GoodsMenu.INACTIVE_TAB_TEXT_COLOR);
				base.Add(this.tabLabels[i]);
			}
			this.tabs[array.Length] = new IndexedColorGraphic(GoodsMenu.UI_FILE, (array.Length < 4) ? "keytagshort" : "keytag", new Vector2f(-8f, -7f) + new Vector2f(50f * (float)array.Length, 0f), -2);
			this.tabs[array.Length].CurrentPalette = this.GetTabPaletteIndex(array.Length);
			base.Add(this.tabs[array.Length]);
			this.SetupItemLists();
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000136D4 File Offset: 0x000118D4
		private uint GetTabPaletteIndex(int tabIndex)
		{
			uint num = Settings.WindowFlavor * 2U;
			if (tabIndex != this.selectedTab)
			{
				return num + 1U;
			}
			return num;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000136F8 File Offset: 0x000118F8
		private string[][] GetItemLists()
		{
			Inventory inventory;
			if (this.selectedTab < this.tabs.Length - 1)
			{
				CharacterType[] array = PartyManager.Instance.ToArray();
				inventory = InventoryManager.Instance.Get(array[this.selectedTab]);
			}
			else
			{
				inventory = InventoryManager.Instance.KeyInventory;
			}
			int[] array2 = new int[2];
			int num = 7;
			array2[0] = Math.Max(0, Math.Min(num, inventory.Count));
			array2[1] = Math.Max(0, Math.Min(num, inventory.Count - 7));
			string[][] array3 = new string[2][];
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i] = new string[array2[i]];
				for (int j = 0; j < array3[i].Length; j++)
				{
					array3[i][j] = inventory[num * i + j].Get<string>("name");
				}
			}
			return array3;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x000137E0 File Offset: 0x000119E0
		private void SetupItemLists()
		{
			if (this.goodsList == null)
			{
				this.goodsList = new ScrollingList[2];
			}
			this.selectedList = 0;
			string[][] itemLists = this.GetItemLists();
			for (int i = 0; i < this.goodsList.Length; i++)
			{
				if (this.goodsList[i] != null)
				{
					base.Remove(this.goodsList[i]);
					this.goodsList[i].Dispose();
					this.goodsList[i] = null;
				}
				if (itemLists[i].Length > 0)
				{
					this.goodsList[i] = new ScrollingList(new Vector2f((float)(6 + ((int)GoodsMenu.PANEL_SIZE.X / 2 + 2) * i), 0f), 1, itemLists[i], 7, 14f, GoodsMenu.PANEL_SIZE.X / 2f - 8f, GoodsMenu.CURSOR_FILE);
					this.goodsList[i].ShowSelectionRectangle = (this.selectedList == i);
					this.goodsList[i].UseHighlightTextColor = (this.selectedList == i);
					this.goodsList[i].ShowCursor = (this.selectedList == i);
					base.Add(this.goodsList[i]);
				}
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00013904 File Offset: 0x00011B04
		private void SelectTab(int index)
		{
			if (index < 0)
			{
				this.selectedTab = this.tabs.Length - 1;
			}
			else if (index >= this.tabs.Length)
			{
				this.selectedTab = 0;
			}
			else
			{
				this.selectedTab = index;
			}
			for (int i = 0; i < this.tabs.Length; i++)
			{
				this.tabs[i].CurrentPalette = this.GetTabPaletteIndex(i);
				this.tabs[i].Depth = ((i == this.selectedTab) ? 1 : -2);
				if (i < this.tabs.Length - 1)
				{
					this.tabLabels[i].Color = ((i == this.selectedTab) ? GoodsMenu.ACTIVE_TAB_TEXT_COLOR : GoodsMenu.INACTIVE_TAB_TEXT_COLOR);
					this.tabLabels[i].Depth = ((i == this.selectedTab) ? 2 : -1);
				}
			}
			this.SetupItemLists();
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000139D8 File Offset: 0x00011BD8
		public override void AxisPressed(Vector2f axis)
		{
			if (axis.Y < 0f)
			{
				this.goodsList[this.selectedList].SelectPrevious();
				return;
			}
			if (axis.Y > 0f)
			{
				this.goodsList[this.selectedList].SelectNext();
				return;
			}
			if (axis.X != 0f)
			{
				int num = (this.selectedList + 1) % 2;
				if (this.goodsList[num] != null)
				{
					int selectedIndex = this.goodsList[this.selectedList].SelectedIndex;
					this.goodsList[this.selectedList].ShowSelectionRectangle = false;
					this.goodsList[this.selectedList].UseHighlightTextColor = false;
					this.goodsList[this.selectedList].ShowCursor = false;
					this.selectedList = num;
					this.goodsList[this.selectedList].SelectedIndex = selectedIndex;
					this.goodsList[this.selectedList].ShowSelectionRectangle = true;
					this.goodsList[this.selectedList].UseHighlightTextColor = true;
					this.goodsList[this.selectedList].ShowCursor = true;
				}
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00013AF4 File Offset: 0x00011CF4
		public override object ButtonPressed(Button button)
		{
			int? num = null;
			if (button == Button.B)
			{
				num = new int?(-1);
			}
			else if (button == Button.L)
			{
				this.SelectTab(this.selectedTab - 1);
			}
			else if (button == Button.R)
			{
				this.SelectTab(this.selectedTab + 1);
			}
			return num;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00013B43 File Offset: 0x00011D43
		public override void Focus()
		{
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00013B45 File Offset: 0x00011D45
		public override void Unfocus()
		{
		}

		// Token: 0x04000478 RID: 1144
		public const int PANEL_DEPTH = 0;

		// Token: 0x04000479 RID: 1145
		public const float TAB_WIDTH = 50f;

		// Token: 0x0400047A RID: 1146
		public const int MAX_SUPPORTED_PARTY_MEMBERS = 4;

		// Token: 0x0400047B RID: 1147
		private const string FRONT_TAG = "firsttag";

		// Token: 0x0400047C RID: 1148
		private const string TAG = "tag";

		// Token: 0x0400047D RID: 1149
		private const string KEY_TAG = "keytag";

		// Token: 0x0400047E RID: 1150
		private const string KEY_TAG_SHORT = "keytagshort";

		// Token: 0x0400047F RID: 1151
		public static readonly Vector2f PANEL_POSITION = MainMenu.PANEL_POSITION + new Vector2f(MainMenu.PANEL_SIZE.X + 20f, 13f);

		// Token: 0x04000480 RID: 1152
		public static readonly Vector2f PANEL_SIZE = new Vector2f(320f - GoodsMenu.PANEL_POSITION.X - 20f, 99f);

		// Token: 0x04000481 RID: 1153
		public static readonly Color ACTIVE_TAB_TEXT_COLOR = Color.Black;

		// Token: 0x04000482 RID: 1154
		public static readonly Color INACTIVE_TAB_TEXT_COLOR = new Color(65, 80, 79);

		// Token: 0x04000483 RID: 1155
		private static readonly string CURSOR_FILE = Paths.GRAPHICS + "cursor.dat";

		// Token: 0x04000484 RID: 1156
		private static readonly string UI_FILE = Paths.GRAPHICS + "pause.dat";

		// Token: 0x04000485 RID: 1157
		private ShapeGraphic divider;

		// Token: 0x04000486 RID: 1158
		private IndexedColorGraphic[] tabs;

		// Token: 0x04000487 RID: 1159
		private TextRegion[] tabLabels;

		// Token: 0x04000488 RID: 1160
		private int selectedTab;

		// Token: 0x04000489 RID: 1161
		private ScrollingList[] goodsList;

		// Token: 0x0400048A RID: 1162
		private int selectedList;
	}
}
