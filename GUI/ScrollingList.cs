using System;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Utility;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	// Token: 0x02000050 RID: 80
	internal class ScrollingList : Renderable
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000B79B File Offset: 0x0000999B
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000B7A3 File Offset: 0x000099A3
		public int SelectedIndex
		{
			get
			{
				return this.selectedIndex;
			}
			set
			{
				this.Select(value);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000B7AC File Offset: 0x000099AC
		public string SelectedItem
		{
			get
			{
				return this.items[this.selectedIndex];
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000B7BB File Offset: 0x000099BB
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x0000B7C3 File Offset: 0x000099C3
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000B7CC File Offset: 0x000099CC
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000B7D4 File Offset: 0x000099D4
		public bool ShowArrows
		{
			get
			{
				return this.showArrows;
			}
			set
			{
				this.showArrows = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000B7DD File Offset: 0x000099DD
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000B7E5 File Offset: 0x000099E5
		public bool ShowSelectionRectangle
		{
			get
			{
				return this.showSelectRect;
			}
			set
			{
				this.showSelectRect = value;
				this.UpdateCursor();
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000B7F4 File Offset: 0x000099F4
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000B7FC File Offset: 0x000099FC
		public bool UseHighlightTextColor
		{
			get
			{
				return this.useHighlightTextColor;
			}
			set
			{
				this.useHighlightTextColor = value;
				this.UpdateCursor();
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000B80B File Offset: 0x00009A0B
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000B813 File Offset: 0x00009A13
		public bool ShowCursor
		{
			get
			{
				return this.showCursor;
			}
			set
			{
				this.showCursor = value;
				this.UpdateCursor();
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000B822 File Offset: 0x00009A22
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000B82A File Offset: 0x00009A2A
		public bool Focused
		{
			get
			{
				return this.focused;
			}
			set
			{
				this.focused = value;
				this.UpdateCursor();
				this.UpdateScrollers();
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000B83F File Offset: 0x00009A3F
		public int Count
		{
			get
			{
				return this.items.Length;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000B849 File Offset: 0x00009A49
		// (set) Token: 0x060001EF RID: 495 RVA: 0x0000B854 File Offset: 0x00009A54
		public override Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
				for (int i = 0; i < this.texts.Length; i++)
				{
					this.texts[i].Position = new Vector2f(this.position.X, this.position.Y + this.lineHeight * (float)i);
				}
				this.UpdateCursor();
				this.upArrow.Position = this.position + new Vector2f(this.width, 0f);
				this.downArrow.Position = this.position + new Vector2f(this.width, this.lineHeight * (float)this.displayCount + 1f);
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000B910 File Offset: 0x00009B10
		public ScrollingList(Vector2f position, int depth, string[] items, int displayCount, float lineHeight, float width, string cursorGraphic)
		{
			if (items == null)
			{
				throw new ArgumentNullException("List item array cannot be null.");
			}
			if (items.Length == 0)
			{
				throw new ArgumentException("List item array cannot be empty.");
			}
			this.position = position;
			this.origin = VectorMath.ZERO_VECTOR;
			this.items = items;
			this.displayCount = displayCount;
			this.lineHeight = lineHeight;
			this.width = width;
			this.size = new Vector2f(this.width, this.lineHeight * (float)this.displayCount);
			this.showArrows = true;
			this.showCursor = true;
			this.enabled = true;
			for (int i = 0; i < this.items.Length; i++)
			{
				if (this.items[i] == null)
				{
					this.items[i] = string.Empty;
				}
			}
			int num = Math.Min(items.Length, displayCount);
			this.texts = new TextRegion[num];
			for (int j = 0; j < num; j++)
			{
				this.texts[j] = new TextRegion(new Vector2f(position.X, position.Y + lineHeight * (float)j), depth, Fonts.Main, items[j]);
			}
			this.cursor = new IndexedColorGraphic(cursorGraphic, "right", this.texts[0].Position, depth);
			this.upArrow = new IndexedColorGraphic(cursorGraphic, "up", position + new Vector2f(width, 0f), depth);
			this.downArrow = new IndexedColorGraphic(cursorGraphic, "down", position + new Vector2f(width, lineHeight * (float)displayCount + 1f), depth);
			RectangleShape rectangleShape = new RectangleShape(new Vector2f(this.width, (float)Fonts.Main.WHeight * 1.3f - ScrollingList.SELECT_RECT_OFFSET.Y * 2f) - ScrollingList.SELECT_RECT_SIZE_OFFSET);
			rectangleShape.FillColor = UIColors.HighlightColor;
			this.selectRectangle = new ShapeGraphic(rectangleShape, this.texts[0].Position + ScrollingList.SELECT_RECT_OFFSET, VectorMath.ZERO_VECTOR, rectangleShape.Size, this.depth - 1);
			this.selectRectangle.Visible = this.showSelectRect;
			this.cursorOffset = (Fonts.Main.WHeight - (int)this.lineHeight) / 2;
			this.UpdateCursor();
			this.UpdateScrollers();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000BB64 File Offset: 0x00009D64
		private void SetVisibility(bool visible)
		{
			for (int i = 0; i < this.texts.Length; i++)
			{
				this.texts[i].Visible = visible;
			}
			this.cursor.Visible = visible;
			this.downArrow.Visible = visible;
			this.upArrow.Visible = visible;
			this.selectRectangle.Visible = (visible && this.showSelectRect);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000BBCD File Offset: 0x00009DCD
		public void Hide()
		{
			this.enabledOnHide = this.enabled;
			this.enabled = false;
			this.SetVisibility(false);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000BBE9 File Offset: 0x00009DE9
		public void Show()
		{
			this.enabled = this.enabledOnHide;
			this.SetVisibility(true);
			this.UpdateScrollers();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000BC04 File Offset: 0x00009E04
		public bool SelectPrevious()
		{
			if (this.enabled && this.selectedIndex - 1 >= 0)
			{
				this.selectedIndex--;
				if (this.selectedIndex < this.topIndex)
				{
					this.topIndex--;
					this.UpdateDisplayTexts();
					this.UpdateScrollers();
				}
				this.UpdateCursor();
				return true;
			}
			return false;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000BC64 File Offset: 0x00009E64
		public bool SelectNext()
		{
			if (this.enabled && this.selectedIndex + 1 < this.items.Length)
			{
				this.selectedIndex++;
				if (this.selectedIndex > this.topIndex + this.displayCount - 1)
				{
					this.topIndex++;
					this.UpdateDisplayTexts();
					this.UpdateScrollers();
				}
				this.UpdateCursor();
				return true;
			}
			return false;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000BCD4 File Offset: 0x00009ED4
		private void Select(int i)
		{
			this.selectedIndex = Math.Min(this.items.Length - 1, Math.Max(0, i));
			this.topIndex = Math.Min(this.selectedIndex, Math.Max(0, this.items.Length - this.displayCount));
			this.UpdateDisplayTexts();
			this.UpdateScrollers();
			this.UpdateCursor();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000BD35 File Offset: 0x00009F35
		public void ChangeItem(int index, string newValue)
		{
			if (index >= 0 && index < this.items.Length)
			{
				this.items[index] = newValue;
				this.UpdateDisplayTexts();
				return;
			}
			throw new ArgumentException("Item index out of range.");
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000BD60 File Offset: 0x00009F60
		private void UpdateDisplayTexts()
		{
			for (int i = 0; i < this.displayCount; i++)
			{
				int num = this.topIndex + i;
				if (num < this.items.Length)
				{
					string text = this.items[num];
					this.texts[i].Reset(text, 0, text.Length);
				}
				else if (i < this.texts.Length)
				{
					this.texts[i].Reset(string.Empty, 0, 0);
				}
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000BDD4 File Offset: 0x00009FD4
		private void UpdateCursor()
		{
			this.cursor.Visible = (this.focused && this.showCursor);
			this.cursor.Position = new Vector2f(this.position.X - 1f, this.position.Y + this.lineHeight * (float)(this.selectedIndex - this.topIndex) + (float)Fonts.Main.WHeight - this.cursor.Size.Y / 2f);
			for (int i = 0; i < this.texts.Length; i++)
			{
				this.texts[i].Color = (this.focused ? ScrollingList.FOCUSED_TEXT_COLOR : ScrollingList.UNFOCUSED_TEXT_COLOR);
			}
			if (this.showSelectRect)
			{
				this.selectRectangle.Position = this.texts[this.selectedIndex - this.topIndex].Position + ScrollingList.SELECT_RECT_OFFSET;
			}
			if (this.useHighlightTextColor)
			{
				this.texts[this.selectedIndex - this.topIndex].Color = Color.Green	;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000BEF4 File Offset: 0x0000A0F4
		private void UpdateScrollers()
		{
			bool visible = this.upArrow.Visible;
			bool visible2 = this.downArrow.Visible;
			this.upArrow.Visible = (this.showArrows && this.focused && this.topIndex > 0);
			this.downArrow.Visible = (this.showArrows && this.focused && this.topIndex < this.items.Length - this.displayCount);
			if (this.upArrow.Visible && !visible && this.downArrow.Visible)
			{
				this.upArrow.Frame = this.downArrow.Frame;
			}
			if (this.downArrow.Visible && !visible2 && this.upArrow.Visible)
			{
				this.downArrow.Frame = this.upArrow.Frame;
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000BFE4 File Offset: 0x0000A1E4
		public override void Draw(RenderTarget target)
		{
			if (this.selectRectangle.Visible && this.showSelectRect)
			{
				this.selectRectangle.Draw(target);
			}
			for (int i = 0; i < this.texts.Length; i++)
			{
				if (this.texts[i].Visible)
				{
					this.texts[i].Draw(target);
				}
			}
			if (this.cursor.Visible)
			{
				this.cursor.Draw(target);
			}
			if (this.downArrow.Visible)
			{
				this.downArrow.Draw(target);
			}
			if (this.upArrow.Visible)
			{
				this.upArrow.Draw(target);
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000C090 File Offset: 0x0000A290
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				for (int i = 0; i < this.texts.Length; i++)
				{
					this.texts[i].Dispose();
				}
				this.cursor.Dispose();
				this.downArrow.Dispose();
				this.upArrow.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x040002B5 RID: 693
		private const int CURSOR_MARGIN = 1;

		// Token: 0x040002B6 RID: 694
		private static readonly Vector2f SELECT_RECT_OFFSET = new Vector2f(-2f, 0f);

		// Token: 0x040002B7 RID: 695
		private static readonly Vector2f SELECT_RECT_SIZE_OFFSET = new Vector2f(-2f, 0f);

		// Token: 0x040002B8 RID: 696
		private static readonly Color FOCUSED_TEXT_COLOR = Color.White;

		// Token: 0x040002B9 RID: 697
		private static readonly Color UNFOCUSED_TEXT_COLOR = new Color(128, 140, 138);

		// Token: 0x040002BA RID: 698
		private string[] items;

		// Token: 0x040002BB RID: 699
		private int displayCount;

		// Token: 0x040002BC RID: 700
		private int selectedIndex;

		// Token: 0x040002BD RID: 701
		private int topIndex;

		// Token: 0x040002BE RID: 702
		private float lineHeight;

		// Token: 0x040002BF RID: 703
		private float width;

		// Token: 0x040002C0 RID: 704
		private TextRegion[] texts;

		// Token: 0x040002C1 RID: 705
		private IndexedColorGraphic cursor;

		// Token: 0x040002C2 RID: 706
		private IndexedColorGraphic upArrow;

		// Token: 0x040002C3 RID: 707
		private IndexedColorGraphic downArrow;

		// Token: 0x040002C4 RID: 708
		private ShapeGraphic selectRectangle;

		// Token: 0x040002C5 RID: 709
		private bool enabled;

		// Token: 0x040002C6 RID: 710
		private bool enabledOnHide;

		// Token: 0x040002C7 RID: 711
		private bool showArrows;

		// Token: 0x040002C8 RID: 712
		private bool showSelectRect = true;

		// Token: 0x040002C9 RID: 713
		private bool showCursor = true;

		// Token: 0x040002CA RID: 714
		private bool useHighlightTextColor = true;

		// Token: 0x040002CB RID: 715
		private bool focused = true;

		// Token: 0x040002CC RID: 716
		private int cursorOffset;
	}
}
