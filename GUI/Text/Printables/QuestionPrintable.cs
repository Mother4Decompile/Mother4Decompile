using System;
using Carbine.Audio;
using Carbine.Flags;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.Text.Printables
{
	// Token: 0x0200004B RID: 75
	internal class QuestionPrintable : Printable
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000AD05 File Offset: 0x00008F05
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000AD0D File Offset: 0x00008F0D
		public override Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.SetPosition(value);
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000AD18 File Offset: 0x00008F18
		public QuestionPrintable(FontData font, float width, string[] options)
		{
			this.size = new Vector2f(width, (float)font.LineHeight);
			this.isRemovable = true;
			this.texts = new TextRegion[options.Length];
			for (int i = 0; i < this.texts.Length; i++)
			{
				this.texts[i] = new TextRegion(VectorMath.ZERO_VECTOR, 0, font, options[i]);
			}
			this.selectedIndex = 0;
			this.cursor = new IndexedColorGraphic(Paths.GRAPHICS + "cursor.dat", "right", VectorMath.ZERO_VECTOR, 0);
			this.selectRect = new RectangleShape();
			this.selectRect.Origin = new Vector2f(1f, 0f);
			this.selectRect.FillColor = UIColors.HighlightColor;
			this.selectRectStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, null);
			this.cursorOffsetY = (float)(font.LineHeight / 2);
			this.cellSize = (int)(width - this.cursor.Size.X * 2f) / Math.Max(1, this.texts.Length);
			this.moveSound = AudioManager.Instance.Use(Paths.AUDIO + "cursorx.wav", AudioType.Sound);
			this.selectSound = AudioManager.Instance.Use(Paths.AUDIO + "confirm.wav", AudioType.Sound);
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			InputManager.Instance.AxisPressed += this.AxisPressed;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000AE9E File Offset: 0x0000909E
		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			if (axis.X < 0f)
			{
				this.selectLeft = true;
				return;
			}
			if (axis.X > 0f)
			{
				this.selectRight = true;
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000AECB File Offset: 0x000090CB
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.A)
			{
				this.select = true;
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000AED8 File Offset: 0x000090D8
		private void UpdateOptionPositions()
		{
			for (int i = 0; i < this.texts.Length; i++)
			{
				this.texts[i].Position = this.position + new Vector2f((float)((int)this.cursor.Size.X + this.cellSize * i), 0f);
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000AF38 File Offset: 0x00009138
		private void UpdateSelection()
		{
			this.cursor.Position = this.texts[this.selectedIndex].Position + new Vector2f(0f, this.cursorOffsetY);
			for (int i = 0; i < this.texts.Length; i++)
			{
				if (this.selectedIndex == i)
				{
					this.texts[i].Color = Color.Black;
					this.selectRect.Position = this.texts[i].Position;
					this.selectRect.Size = new Vector2f(this.texts[i].Size.X + 2f, (float)this.texts[i].FontData.LineHeight);
				}
				else
				{
					this.texts[i].Color = Color.White;
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000B012 File Offset: 0x00009212
		private void SetPosition(Vector2f newPosition)
		{
			this.position = newPosition;
			this.UpdateOptionPositions();
			this.UpdateSelection();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000B028 File Offset: 0x00009228
		public override void Update()
		{
			if (this.selectLeft || this.selectRight)
			{
				this.moveSound.Play();
			}
			if (this.selectLeft)
			{
				this.selectedIndex = (this.selectedIndex + this.texts.Length - 1) % this.texts.Length;
				this.UpdateSelection();
				this.selectLeft = false;
			}
			if (this.selectRight)
			{
				this.selectedIndex = (this.selectedIndex + 1) % this.texts.Length;
				this.UpdateSelection();
				this.selectRight = false;
			}
			if (this.select && !this.hasSelected)
			{
				this.selectSound.Play();
				this.selectSound.OnComplete += this.selectSound_OnComplete;
				FlagManager.Instance[2] = (this.selectedIndex == 0);
				ValueManager.Instance[0] = this.selectedIndex;
				this.cursor.Visible = false;
				this.UnregisterInputDelegates();
				this.hasSelected = true;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000B121 File Offset: 0x00009321
		private void selectSound_OnComplete(CarbineSound sender)
		{
			this.isDone = true;
			sender.OnComplete -= this.selectSound_OnComplete;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000B13C File Offset: 0x0000933C
		public override void Draw(RenderTarget target)
		{
			this.selectRect.Draw(target, this.selectRectStates);
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
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000B1A4 File Offset: 0x000093A4
		private void UnregisterInputDelegates()
		{
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			InputManager.Instance.AxisPressed -= this.AxisPressed;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000B1D4 File Offset: 0x000093D4
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.selectRect.Dispose();
					this.cursor.Dispose();
					for (int i = 0; i < this.texts.Length; i++)
					{
						this.texts[i].Dispose();
					}
				}
				AudioManager.Instance.Unuse(this.moveSound);
				AudioManager.Instance.Unuse(this.selectSound);
				this.UnregisterInputDelegates();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400028E RID: 654
		private const string SUBSPRITE_NAME = "right";

		// Token: 0x0400028F RID: 655
		private IndexedColorGraphic cursor;

		// Token: 0x04000290 RID: 656
		private RectangleShape selectRect;

		// Token: 0x04000291 RID: 657
		private RenderStates selectRectStates;

		// Token: 0x04000292 RID: 658
		private float cursorOffsetY;

		// Token: 0x04000293 RID: 659
		private TextRegion[] texts;

		// Token: 0x04000294 RID: 660
		private int cellSize;

		// Token: 0x04000295 RID: 661
		private bool select;

		// Token: 0x04000296 RID: 662
		private bool selectLeft;

		// Token: 0x04000297 RID: 663
		private bool selectRight;

		// Token: 0x04000298 RID: 664
		private bool hasSelected;

		// Token: 0x04000299 RID: 665
		private int selectedIndex;

		// Token: 0x0400029A RID: 666
		private CarbineSound moveSound;

		// Token: 0x0400029B RID: 667
		private CarbineSound selectSound;
	}
}
