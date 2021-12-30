using System;
using System.Text;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Battle;
using Mother4.Data;
using Mother4.Data.Character;
using Mother4.Data.Psi;
using Rufini.Strings;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.OverworldMenu
{
	// Token: 0x02000099 RID: 153
	internal class PsiMenu : MenuPanel
	{
		// Token: 0x0600031F RID: 799 RVA: 0x00013F28 File Offset: 0x00012128
		public PsiMenu() : base(ViewManager.Instance.FinalTopLeft + PsiMenu.PANEL_POSITION, PsiMenu.PANEL_SIZE, 0)
		{
			RectangleShape rectangleShape = new RectangleShape(new Vector2f(1f, PsiMenu.PANEL_SIZE.Y * 0.6f));
			rectangleShape.FillColor = PsiMenu.DIVIDER_COLOR;
			this.vertDivider = new ShapeGraphic(rectangleShape, new Vector2f(PsiMenu.PANEL_SIZE.X * 0.33f, PsiMenu.PANEL_SIZE.Y * 0.3f), VectorMath.Truncate(rectangleShape.Size / 2f), rectangleShape.Size, 1);
			base.Add(this.vertDivider);
			RectangleShape rectangleShape2 = new RectangleShape(new Vector2f(PsiMenu.PANEL_SIZE.X, 1f));
			rectangleShape2.FillColor = PsiMenu.DIVIDER_COLOR;
			this.horizDivider = new ShapeGraphic(rectangleShape2, new Vector2f(PsiMenu.PANEL_SIZE.X * 0.5f, PsiMenu.PANEL_SIZE.Y * 0.66f), VectorMath.Truncate(rectangleShape2.Size / 2f), rectangleShape2.Size, 1);
			base.Add(this.horizDivider);
			CharacterType[] array = PartyManager.Instance.ToArray();
			this.tabs = new IndexedColorGraphic[array.Length];
			this.tabLabels = new TextRegion[array.Length];
			this.charactersWithPsi = new CharacterType[array.Length];
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				StatSet stats = CharacterStats.GetStats(array[i]);
				CharacterData data = CharacterFile.Instance.GetData(array[i]);
				if (data.HasPsi(stats.Level))
				{
					this.tabs[num] = new IndexedColorGraphic(Paths.GRAPHICS + "pause.dat", (num == this.selectedTab) ? "firsttag" : "tag", new Vector2f(-8f, -7f) + new Vector2f(50f * (float)num, 0f), (num == this.selectedTab) ? 1 : -2);
					this.tabs[num].CurrentPalette = this.GetTabPaletteIndex(num);
					base.Add(this.tabs[num]);
					this.tabLabels[num] = new TextRegion(new Vector2f(-4f, -21f) + new Vector2f(50f * (float)num, 0f), (num == this.selectedTab) ? 2 : -1, Fonts.Main, CharacterNames.GetName(array[i]));
					this.tabLabels[num].Color = ((num == this.selectedTab) ? PsiMenu.ACTIVE_TAB_TEXT_COLOR : PsiMenu.INACTIVE_TAB_TEXT_COLOR);
					base.Add(this.tabLabels[num]);
					this.charactersWithPsi[num] = array[i];
					num++;
				}
			}
			Array.Resize<IndexedColorGraphic>(ref this.tabs, num);
			Array.Resize<TextRegion>(ref this.tabLabels, num);
			Array.Resize<CharacterType>(ref this.charactersWithPsi, num);
			this.psiList = new PsiList[this.charactersWithPsi.Length];
			for (int j = 0; j < this.charactersWithPsi.Length; j++)
			{
				this.psiList[j] = new PsiList(PsiMenu.PSI_LIST_POSITION, this.charactersWithPsi[j], PsiMenu.PSI_LIST_WIDTH, 4, this.depth);
				this.psiList[j].Visible = false;
				if (j == this.selectedTab)
				{
					this.psiList[j].Show();
				}
				base.Add(this.psiList[j]);
			}
			this.descriptionText = new TextRegion(new Vector2f(8f, (float)((int)(PsiMenu.PANEL_SIZE.Y * 0.66f) + 4)), 0, Fonts.Main, this.GetDescription());
			base.Add(this.descriptionText);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0001430C File Offset: 0x0001250C
		private uint GetTabPaletteIndex(int tabIndex)
		{
			uint num = Settings.WindowFlavor * 2U;
			if (tabIndex != this.selectedTab)
			{
				return num + 1U;
			}
			return num;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00014330 File Offset: 0x00012530
		private string GetDescription()
		{
			string text = null;
			if (this.psiList[this.selectedTab].SelectedPsiLevel != null)
			{
				PsiLevel value = this.psiList[this.selectedTab].SelectedPsiLevel.Value;
				PsiData data = PsiFile.Instance.GetData(value.PsiType);
				StringBuilder stringBuilder = new StringBuilder(data.Key.Length + 4);
				stringBuilder.Append(data.Key);
				stringBuilder.Replace("psi", "psiDesc");
				stringBuilder.Append(value.Level + 1);
				string qualifiedName = stringBuilder.ToString();
				text = StringFile.Instance.Get(qualifiedName).Value;
			}
			return text ?? string.Empty;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000143F8 File Offset: 0x000125F8
		private void UpdateDescription()
		{
			string description = this.GetDescription();
			this.descriptionText.Reset(description, 0, description.Length);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00014420 File Offset: 0x00012620
		public override void AxisPressed(Vector2f axis)
		{
			if (axis.Y < 0f)
			{
				this.psiList[this.selectedTab].SelectUp();
			}
			else if (axis.Y > 0f)
			{
				this.psiList[this.selectedTab].SelectDown();
			}
			if (axis.X < 0f)
			{
				this.psiList[this.selectedTab].SelectLeft();
			}
			else if (axis.X > 0f)
			{
				this.psiList[this.selectedTab].SelectRight();
			}
			this.UpdateDescription();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000144B8 File Offset: 0x000126B8
		private void SelectTab(int index)
		{
			this.psiList[this.selectedTab].Hide();
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
				this.tabLabels[i].Color = ((i == this.selectedTab) ? PsiMenu.ACTIVE_TAB_TEXT_COLOR : PsiMenu.INACTIVE_TAB_TEXT_COLOR);
				this.tabLabels[i].Depth = ((i == this.selectedTab) ? 2 : -1);
			}
			this.psiList[this.selectedTab].Show();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00014598 File Offset: 0x00012798
		public override object ButtonPressed(Button button)
		{
			object result = null;
			if (button == Button.A)
			{
				if (this.psiList[this.selectedTab].SelectedPanelType == PsiList.PanelType.PsiTypePanel && this.psiList[this.selectedTab].SelectedPsiLevel != null)
				{
					result = this.psiList[this.selectedTab].SelectedPsiLevel.Value;
				}
				else
				{
					this.psiList[this.selectedTab].Accept();
				}
			}
			else if (button == Button.B)
			{
				if (this.psiList[this.selectedTab].SelectedPanelType == PsiList.PanelType.PsiGroupPanel)
				{
					result = -1;
				}
				else
				{
					this.psiList[this.selectedTab].Cancel();
				}
			}
			else if (button == Button.L)
			{
				this.SelectTab(this.selectedTab - 1);
			}
			else if (button == Button.R)
			{
				this.SelectTab(this.selectedTab + 1);
			}
			return result;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0001466D File Offset: 0x0001286D
		public override void Focus()
		{
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0001466F File Offset: 0x0001286F
		public override void Unfocus()
		{
		}

		// Token: 0x04000496 RID: 1174
		private const int PSI_LIST_ROWS = 4;

		// Token: 0x04000497 RID: 1175
		private const int PANEL_DEPTH = 0;

		// Token: 0x04000498 RID: 1176
		private const float TAB_WIDTH = 50f;

		// Token: 0x04000499 RID: 1177
		private const int MAX_SUPPORTED_PARTY_MEMBERS = 4;

		// Token: 0x0400049A RID: 1178
		private const string FILE = "pause.dat";

		// Token: 0x0400049B RID: 1179
		private const string FRONT_TAG = "firsttag";

		// Token: 0x0400049C RID: 1180
		private const string TAG = "tag";

		// Token: 0x0400049D RID: 1181
		private static readonly Vector2f PANEL_POSITION = MainMenu.PANEL_POSITION + new Vector2f(MainMenu.PANEL_SIZE.X + 20f, 13f);

		// Token: 0x0400049E RID: 1182
		private static readonly Vector2f PANEL_SIZE = new Vector2f(320f - PsiMenu.PANEL_POSITION.X - 20f, 99f);

		// Token: 0x0400049F RID: 1183
		private static readonly Vector2f PSI_LIST_POSITION = new Vector2f(8f, 2f);

		// Token: 0x040004A0 RID: 1184
		private static readonly int PSI_LIST_WIDTH = (int)(PsiMenu.PANEL_SIZE.X - PsiMenu.PSI_LIST_POSITION.X);

		// Token: 0x040004A1 RID: 1185
		private static readonly Color ACTIVE_TAB_TEXT_COLOR = Color.Black;

		// Token: 0x040004A2 RID: 1186
		private static readonly Color INACTIVE_TAB_TEXT_COLOR = new Color(65, 80, 79);

		// Token: 0x040004A3 RID: 1187
		private static readonly Color DIVIDER_COLOR = new Color(128, 140, 138);

		// Token: 0x040004A4 RID: 1188
		private static readonly string CURSOR_FILE = Paths.GRAPHICS + "cursor.dat";

		// Token: 0x040004A5 RID: 1189
		private ShapeGraphic horizDivider;

		// Token: 0x040004A6 RID: 1190
		private ShapeGraphic vertDivider;

		// Token: 0x040004A7 RID: 1191
		private IndexedColorGraphic[] tabs;

		// Token: 0x040004A8 RID: 1192
		private TextRegion[] tabLabels;

		// Token: 0x040004A9 RID: 1193
		private int selectedTab;

		// Token: 0x040004AA RID: 1194
		private PsiList[] psiList;

		// Token: 0x040004AB RID: 1195
		private CharacterType[] charactersWithPsi;

		// Token: 0x040004AC RID: 1196
		private TextRegion descriptionText;
	}
}
