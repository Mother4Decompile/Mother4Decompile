using System;
using System.Linq;
using Carbine.Actors;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x020000DE RID: 222
	internal class ButtonBar : Actor
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001FAAD File Offset: 0x0001DCAD
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x0001FAB5 File Offset: 0x0001DCB5
		public int SelectedIndex
		{
			get
			{
				return this.selIndex;
			}
			set
			{
				this.lastSelIndex = this.selIndex;
				this.selIndex = Math.Max(0, Math.Min(this.buttons.Length - 1, value));
				this.SelectedIndexChanged();
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x0001FAE5 File Offset: 0x0001DCE5
		public ButtonBar.Action SelectedAction
		{
			get
			{
				return this.buttonActions[this.selIndex];
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x0001FAF4 File Offset: 0x0001DCF4
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x0001FAFC File Offset: 0x0001DCFC
		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				this.visible = value;
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001FB05 File Offset: 0x0001DD05
		public ButtonBar(RenderPipeline pipeline)
		{
			this.pipeline = pipeline;
			this.buttonActions = new ButtonBar.Action[0];
			this.buttons = new Graphic[0];
			this.selIndex = 0;
			this.visible = false;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001FB3C File Offset: 0x0001DD3C
		private void SetUpButtons(ButtonBar.Action[] newActions)
		{
			for (int i = 0; i < this.buttons.Length; i++)
			{
				if (this.buttons[i] != null)
				{
					this.pipeline.Remove(this.buttons[i]);
				}
			}
			this.buttonActions = new ButtonBar.Action[newActions.Length];
			this.buttons = new Graphic[newActions.Length];
			this.buttonWidths = new int[this.buttons.Length];
			int num = 0;
			for (int j = 0; j < newActions.Length; j++)
			{
				string spriteName;
				switch (newActions[j])
				{
				case ButtonBar.Action.Bash:
					spriteName = "bash";
					break;
				case ButtonBar.Action.Psi:
					spriteName = "psi";
					break;
				case ButtonBar.Action.Items:
					spriteName = "goods";
					break;
				case ButtonBar.Action.Talk:
					spriteName = "talk";
					break;
				case ButtonBar.Action.Guard:
					spriteName = "guard";
					break;
				case ButtonBar.Action.Run:
					spriteName = "run";
					break;
				default:
					throw new NotImplementedException("Unimplemented button action type.");
				}
				this.buttonActions[j] = newActions[j];
				this.buttons[j] = new IndexedColorGraphic(ButtonBar.GRAPHIC_FILE, spriteName, default(Vector2f), 32757);
				((IndexedColorGraphic)this.buttons[j]).CurrentPalette = Settings.WindowFlavor;
				this.buttonWidths[j] = this.buttons[j].TextureRect.Width;
				num += this.buttonWidths[j] + 2;
			}
			num -= 2;
			this.buttonYs = new int[this.buttons.Length];
			this.buttonHeights = new int[this.buttons.Length];
			int num2 = 160 - num / 2;
			for (int k = 0; k < this.buttons.Length; k++)
			{
				this.buttonHeights[k] = -24;
				this.buttonYs[k] = this.buttonHeights[k];
				this.buttons[k].Position = new Vector2f((float)num2, (float)this.buttonYs[k]);
				num2 += this.buttonWidths[k] + 2;
				this.pipeline.Add(this.buttons[k]);
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001FD39 File Offset: 0x0001DF39
		public void SetActions(ButtonBar.Action[] newActions)
		{
			if (!this.buttonActions.SequenceEqual(newActions))
			{
				this.SetUpButtons(newActions);
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001FD50 File Offset: 0x0001DF50
		private void SelectedIndexChanged()
		{
			for (int i = 0; i < this.buttons.Length; i++)
			{
				this.buttonHeights[i] = 4;
				this.buttons[i].Frame = 0f;
			}
			this.buttonHeights[this.SelectedIndex] = 6;
			this.buttons[this.SelectedIndex].Frame = 1f;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001FDB0 File Offset: 0x0001DFB0
		public void SelectRight()
		{
			if (this.selIndex + 1 < this.buttons.Length)
			{
				this.SelectedIndex = this.selIndex + 1;
				return;
			}
			this.SelectedIndex = 0;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001FDDA File Offset: 0x0001DFDA
		public void SelectLeft()
		{
			if (this.selIndex - 1 >= 0)
			{
				this.SelectedIndex = this.selIndex - 1;
				return;
			}
			this.SelectedIndex = this.buttons.Length - 1;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001FE06 File Offset: 0x0001E006
		public void Show(int index)
		{
			this.SelectedIndex = index;
			this.visible = true;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001FE16 File Offset: 0x0001E016
		public void Show()
		{
			this.Show(this.selIndex);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001FE24 File Offset: 0x0001E024
		public void Hide()
		{
			for (int i = 0; i < this.buttons.Length; i++)
			{
				this.buttonHeights[i] = -this.buttons[i].TextureRect.Height - 1;
			}
			this.visible = false;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001FE68 File Offset: 0x0001E068
		public override void Input()
		{
			base.Input();
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001FE70 File Offset: 0x0001E070
		public override void Update()
		{
			base.Update();
			for (int i = 0; i < this.buttons.Length; i++)
			{
				if (this.buttonYs[i] < this.buttonHeights[i])
				{
					this.buttonYs[i] += (int)Math.Ceiling((double)((float)(this.buttonHeights[i] - this.buttonYs[i]) / 2f));
					this.buttons[i].Position = new Vector2f(this.buttons[i].Position.X, (float)this.buttonYs[i]);
				}
				else if (this.buttonYs[i] > this.buttonHeights[i])
				{
					this.buttonYs[i] += (int)Math.Floor((double)((float)(this.buttonHeights[i] - this.buttonYs[i]) / 2f));
					this.buttons[i].Position = new Vector2f(this.buttons[i].Position.X, (float)this.buttonYs[i]);
				}
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001FF8C File Offset: 0x0001E18C
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			for (int i = 0; i < this.buttons.Length; i++)
			{
				this.buttons[i].Dispose();
			}
		}

		// Token: 0x040006E4 RID: 1764
		private const int BUTTON_MARGIN = 2;

		// Token: 0x040006E5 RID: 1765
		private const int BUTTON_MIN_Y = 4;

		// Token: 0x040006E6 RID: 1766
		private const int BUTTON_MAX_Y = 6;

		// Token: 0x040006E7 RID: 1767
		private const int BUTTON_HEIGHT = 24;

		// Token: 0x040006E8 RID: 1768
		private static readonly string GRAPHIC_FILE = Paths.GRAPHICS + "battleui.dat";

		// Token: 0x040006E9 RID: 1769
		private int[] buttonYs;

		// Token: 0x040006EA RID: 1770
		private int[] buttonHeights;

		// Token: 0x040006EB RID: 1771
		private RenderPipeline pipeline;

		// Token: 0x040006EC RID: 1772
		private int selIndex;

		// Token: 0x040006ED RID: 1773
		private int lastSelIndex;

		// Token: 0x040006EE RID: 1774
		private int[] buttonWidths;

		// Token: 0x040006EF RID: 1775
		private Graphic[] buttons;

		// Token: 0x040006F0 RID: 1776
		private ButtonBar.Action[] buttonActions;

		// Token: 0x040006F1 RID: 1777
		private bool visible;

		// Token: 0x020000DF RID: 223
		public enum Action
		{
			// Token: 0x040006F3 RID: 1779
			None,
			// Token: 0x040006F4 RID: 1780
			Bash,
			// Token: 0x040006F5 RID: 1781
			Psi,
			// Token: 0x040006F6 RID: 1782
			Items,
			// Token: 0x040006F7 RID: 1783
			Talk,
			// Token: 0x040006F8 RID: 1784
			Guard,
			// Token: 0x040006F9 RID: 1785
			Run
		}
	}
}
