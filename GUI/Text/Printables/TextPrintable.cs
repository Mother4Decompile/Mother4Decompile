using System;
using Carbine.Audio;
using Carbine.GUI;
using Carbine.Utility;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.Text.Printables
{
	// Token: 0x0200004C RID: 76
	internal class TextPrintable : Printable
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000B24F File Offset: 0x0000944F
		// (set) Token: 0x060001CA RID: 458 RVA: 0x0000B25C File Offset: 0x0000945C
		public override Vector2f Position
		{
			get
			{
				return this.textRegion.Position;
			}
			set
			{
				this.textRegion.Position = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000B26A File Offset: 0x0000946A
		// (set) Token: 0x060001CC RID: 460 RVA: 0x0000B277 File Offset: 0x00009477
		public override Vector2f Origin
		{
			get
			{
				return this.textRegion.Origin;
			}
			set
			{
				this.textRegion.Origin = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000B285 File Offset: 0x00009485
		public override Vector2f Size
		{
			get
			{
				return this.textRegion.Size + new Vector2f(1f, 0f);
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000B2A8 File Offset: 0x000094A8
		public TextPrintable(FontData font, CarbineSound sound, Color color, string text)
		{
			this.textRegion = new TextRegion(VectorMath.ZERO_VECTOR, 0, font, text);
			this.textRegion.Color = color;
			this.textLength = text.Length;
			this.textRegion.Length = 0;
			this.counter = 0f;
			this.speed = 10f / Math.Max(1f, (float)Settings.TextSpeed);
			this.blepCounter = 0;
			this.sound = sound;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000B32C File Offset: 0x0000952C
		public void TrimStart()
		{
			string text = this.textRegion.Text.TrimStart(new char[0]);
			this.textLength = text.Length;
			this.textRegion.Text = text;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000B368 File Offset: 0x00009568
		public string SplitOffText(int pixelLength)
		{
			int num = 0;
			int i = 0;
			while (i < this.textRegion.Text.Length)
			{
				char c = this.textRegion.Text[i];
				if (char.IsWhiteSpace(c))
				{
					num = i;
				}
				Glyph glyph = this.textRegion.FontData.Font.GetGlyph((uint)c, this.textRegion.FontData.Size, false);
				float num2 = this.textRegion.FindCharacterPos((uint)i).X + glyph.Bounds.Width;
				if (num2 > (float)pixelLength)
				{
					if (num == 0)
					{
						num = i;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			string text = this.textRegion.Text.Substring(0, num);
			string result = this.textRegion.Text.Substring(num + ((num > 0) ? 1 : 0));
			this.textRegion.Reset(text, 0, 0);
			this.textLength = text.Length;
			return result;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000B454 File Offset: 0x00009654
		public override void Update()
		{
			if (this.textRegion.Length + 1 <= this.textLength)
			{
				this.counter += this.speed;
				if ((double)this.textRegion.Length < Math.Floor((double)this.counter))
				{
					this.textRegion.Length += (int)this.counter - this.textRegion.Length;
				}
				this.blepCounter++;
				if (this.blepCounter % 3 == 0 && this.sound != null)
				{
					this.sound.Play();
					return;
				}
			}
			else
			{
				this.textRegion.Length = this.textLength;
				this.isDone = true;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000B50C File Offset: 0x0000970C
		public override void Draw(RenderTarget target)
		{
			this.textRegion.Draw(target);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000B51A File Offset: 0x0000971A
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.textRegion.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400029C RID: 668
		private const float MAX_CHARS_PER_FRAME = 10f;

		// Token: 0x0400029D RID: 669
		private const int FRAMES_PER_BLEP = 3;

		// Token: 0x0400029E RID: 670
		private TextRegion textRegion;

		// Token: 0x0400029F RID: 671
		private int textLength;

		// Token: 0x040002A0 RID: 672
		private float speed;

		// Token: 0x040002A1 RID: 673
		private float counter;

		// Token: 0x040002A2 RID: 674
		private int blepCounter;

		// Token: 0x040002A3 RID: 675
		private CarbineSound sound;
	}
}
