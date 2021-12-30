using System;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Data;
using Rufini.Strings;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.NamingMenu
{
	// Token: 0x02000043 RID: 67
	internal class NamingPanel : MenuPanel
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000963A File Offset: 0x0000783A
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00009647 File Offset: 0x00007847
		public string Description
		{
			get
			{
				return this.descriptionText.Text;
			}
			set
			{
				this.SetDescription(value);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00009650 File Offset: 0x00007850
		// (set) Token: 0x0600016A RID: 362 RVA: 0x0000965D File Offset: 0x0000785D
		public string Name
		{
			get
			{
				return this.nameText.Text;
			}
			set
			{
				this.SetName(value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00009666 File Offset: 0x00007866
		public int NameWidth
		{
			get
			{
				return (int)this.nameText.Size.X;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000967C File Offset: 0x0000787C
		public NamingPanel(Vector2f position, Vector2f size) : base(position, size, 1, WindowBox.Style.Normal, 0U)
		{
			this.descriptionText = new TextRegion(new Vector2f(2f, 0f), 1, Fonts.Main, string.Empty);
			base.Add(this.descriptionText);
			string value = StringFile.Instance.Get("naming.prompt").Value;
			this.promptText = new TextRegion(new Vector2f(2f, (float)Fonts.Main.LineHeight), 1, Fonts.Main, value);
			base.Add(this.promptText);
			RectangleShape rectangleShape = new RectangleShape(new Vector2f(52f, (float)(Fonts.Main.LineHeight - 4)));
			rectangleShape.FillColor = UIColors.HighlightColor;
			this.textbox1 = new ShapeGraphic(rectangleShape, new Vector2f(4f + this.promptText.Size.X, (float)(Fonts.Main.LineHeight + 1)), VectorMath.ZERO_VECTOR, rectangleShape.Size, 1);
			base.Add(this.textbox1);
			RectangleShape rectangleShape2 = new RectangleShape(new Vector2f(50f, (float)(Fonts.Main.LineHeight - 2)));
			rectangleShape2.FillColor = UIColors.HighlightColor;
			this.textbox2 = new ShapeGraphic(rectangleShape2, new Vector2f(5f + this.promptText.Size.X, (float)Fonts.Main.LineHeight), VectorMath.ZERO_VECTOR, rectangleShape2.Size, 1);
			base.Add(this.textbox2);
			RectangleShape rectangleShape3 = new RectangleShape(new Vector2f(1f, (float)(Fonts.Main.LineHeight - 4)));
			rectangleShape3.FillColor = Color.Black;
			this.cursor = new ShapeGraphic(rectangleShape3, new Vector2f(8f + this.promptText.Size.X, (float)(Fonts.Main.LineHeight + 1)), VectorMath.ZERO_VECTOR, rectangleShape3.Size, 4);
			base.Add(this.cursor);
			this.nameText = new TextRegion(new Vector2f(6f + this.promptText.Size.X, (float)Fonts.Main.LineHeight), 2, Fonts.Main, string.Empty);
			this.nameText.Color = Color.Black;
			base.Add(this.nameText);
			this.cursorTimerIndex = TimerManager.Instance.StartTimer(30);
			TimerManager.Instance.OnTimerEnd += this.CursorTimerEnd;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000098EB File Offset: 0x00007AEB
		private void CursorTimerEnd(int timerIndex)
		{
			if (timerIndex == this.cursorTimerIndex)
			{
				this.cursor.Visible = !this.cursor.Visible;
				this.cursorTimerIndex = TimerManager.Instance.StartTimer(30);
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00009921 File Offset: 0x00007B21
		private void SetDescription(string description)
		{
			this.descriptionText.Reset(description, 0, description.Length);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00009938 File Offset: 0x00007B38
		private void SetName(string name)
		{
			this.nameText.Reset(name, 0, name.Length);
			this.cursor.Position = this.nameText.Position + new Vector2f(this.nameText.Size.X + 1f, 1f);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00009993 File Offset: 0x00007B93
		public override object ButtonPressed(Button button)
		{
			return null;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00009996 File Offset: 0x00007B96
		public override void AxisPressed(Vector2f axis)
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00009998 File Offset: 0x00007B98
		public override void Focus()
		{
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000999A File Offset: 0x00007B9A
		public override void Unfocus()
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000999C File Offset: 0x00007B9C
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.descriptionText.Dispose();
				this.promptText.Dispose();
				this.nameText.Dispose();
				this.cursor.Dispose();
				this.textbox1.Dispose();
				this.textbox2.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400025C RID: 604
		private const string PROMPT_STRING = "naming.prompt";

		// Token: 0x0400025D RID: 605
		private const int FLAVOR = 0;

		// Token: 0x0400025E RID: 606
		private const int DEPTH = 1;

		// Token: 0x0400025F RID: 607
		private TextRegion descriptionText;

		// Token: 0x04000260 RID: 608
		private TextRegion promptText;

		// Token: 0x04000261 RID: 609
		private TextRegion nameText;

		// Token: 0x04000262 RID: 610
		private ShapeGraphic cursor;

		// Token: 0x04000263 RID: 611
		private ShapeGraphic textbox1;

		// Token: 0x04000264 RID: 612
		private ShapeGraphic textbox2;

		// Token: 0x04000265 RID: 613
		private int cursorTimerIndex;
	}
}
