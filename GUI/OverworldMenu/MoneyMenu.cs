using System;
using Carbine.Flags;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Rufini.Strings;
using SFML.System;

namespace Mother4.GUI.OverworldMenu
{
	// Token: 0x02000098 RID: 152
	internal class MoneyMenu : MenuPanel
	{
		// Token: 0x06000318 RID: 792 RVA: 0x00013DBC File Offset: 0x00011FBC
		public MoneyMenu() : base(ViewManager.Instance.FinalTopLeft + MoneyMenu.PANEL_POSITION, MoneyMenu.PANEL_SIZE, 0)
		{
			this.dollarText = new TextRegion(new Vector2f(1f, -1f), 1, Fonts.Main, StringFile.Instance.Get("system.currency").Value);
			base.Add(this.dollarText);
			this.RefreshValue();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00013E34 File Offset: 0x00012034
		public void RefreshValue()
		{
			int num = ValueManager.Instance[1];
			string text = string.Format("{0:0.00}", num);
			if (this.moneyText == null)
			{
				this.moneyText = new TextRegion(new Vector2f(MoneyMenu.PANEL_SIZE.X - 2f, -1f), 1, Fonts.Main, text);
				base.Add(this.moneyText);
			}
			this.moneyText.Position -= new Vector2f(this.moneyText.Size.X, 0f);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00013ECE File Offset: 0x000120CE
		public override void AxisPressed(Vector2f axis)
		{
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00013ED0 File Offset: 0x000120D0
		public override object ButtonPressed(Button button)
		{
			return null;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00013ED3 File Offset: 0x000120D3
		public override void Focus()
		{
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00013ED5 File Offset: 0x000120D5
		public override void Unfocus()
		{
		}

		// Token: 0x04000491 RID: 1169
		public const int PANEL_DEPTH = 0;

		// Token: 0x04000492 RID: 1170
		public static readonly Vector2f PANEL_POSITION = MainMenu.PANEL_POSITION + new Vector2f(0f, MainMenu.PANEL_SIZE.Y + 19f);

		// Token: 0x04000493 RID: 1171
		public static readonly Vector2f PANEL_SIZE = new Vector2f(MainMenu.PANEL_SIZE.X, 10f);

		// Token: 0x04000494 RID: 1172
		private TextRegion dollarText;

		// Token: 0x04000495 RID: 1173
		private TextRegion moneyText;
	}
}
