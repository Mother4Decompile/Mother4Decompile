using System;
using System.Collections.Generic;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Utility;
using Mother4.Data;
using Mother4.GUI.Text.Printables;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.Text
{
	// Token: 0x0200009C RID: 156
	internal class Typewriter : Renderable
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00015048 File Offset: 0x00013248
		// (set) Token: 0x06000334 RID: 820 RVA: 0x00015050 File Offset: 0x00013250
		public override Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.Reposition(value, this.origin);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0001505F File Offset: 0x0001325F
		// (set) Token: 0x06000336 RID: 822 RVA: 0x00015067 File Offset: 0x00013267
		public override Vector2f Origin
		{
			get
			{
				return this.origin;
			}
			set
			{
				this.Reposition(this.position, value);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00015076 File Offset: 0x00013276
		// (set) Token: 0x06000338 RID: 824 RVA: 0x0001507E File Offset: 0x0001327E
		public override Vector2f Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00015087 File Offset: 0x00013287
		public bool ShowBullets
		{
			get
			{
				return this.showBullets;
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600033A RID: 826 RVA: 0x00015090 File Offset: 0x00013290
		// (remove) Token: 0x0600033B RID: 827 RVA: 0x000150C8 File Offset: 0x000132C8
		public event Typewriter.TypewriterCompleteHandler OnTypewriterComplete;

		// Token: 0x0600033C RID: 828 RVA: 0x00015100 File Offset: 0x00013300
		public Typewriter(FontData font, Vector2f position, Vector2f origin, Vector2f size, bool showBullets)
		{
			this.depth = 2147450880;
			this.fontData = font;
			this.position = position;
			this.origin = origin;
			this.size = size;
			this.textColor = Color.White;
			this.currentLine = 0;
			int num = (int)(this.size.Y / (float)this.fontData.LineHeight);
			this.lines = new List<Printable>[num];
			for (int i = 0; i < this.lines.Length; i++)
			{
				this.lines[i] = new List<Printable>();
			}
			this.showBullets = showBullets;
			this.bullets = new Renderable[this.lines.Length];
			float num2 = 0f;
			for (int j = 0; j < this.bullets.Length; j++)
			{
				this.bullets[j] = new IndexedColorGraphic(Paths.GRAPHICS + "bullet.dat", "bullet", this.position - this.origin + new Vector2f(0f, (float)(this.fontData.LineHeight * j + this.fontData.LineHeight / 2)), 0);
				this.bullets[j].Visible = false;
				num2 = Math.Max(num2, this.bullets[j].Size.X);
			}
			this.bulletWidth = (this.showBullets ? num2 : 0f);
			this.SetTextSound(Typewriter.BlipSound.Narration, false);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00015270 File Offset: 0x00013470
		private void Reposition(Vector2f newPosition, Vector2f newOrigin)
		{
			Vector2f v = new Vector2f(newPosition.X - newOrigin.X - (this.position.X - this.origin.X), newPosition.Y - newOrigin.Y - (this.position.Y - this.origin.Y));
			for (int i = 0; i < this.lines.Length; i++)
			{
				for (int j = 0; j < this.lines[i].Count; j++)
				{
					this.lines[i][j].Position += v;
				}
				this.bullets[i].Position += v;
			}
			this.position = newPosition;
			this.origin = newOrigin;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00015344 File Offset: 0x00013544
		private int CalculateLineWidth(int line)
		{
			int num = Math.Max(0, Math.Min(this.lines.Length - 1, line));
			int num2 = (int)this.bulletWidth;
			for (int i = 0; i < this.lines[num].Count; i++)
			{
				num2 += (int)this.lines[num][i].Size.X;
			}
			return num2;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000153A8 File Offset: 0x000135A8
		private bool PrintableFitsInCurrentLine(Printable printable)
		{
			int num = this.CalculateLineWidth(this.currentLine);
			return num + (int)printable.Size.X <= (int)this.size.X;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000153E4 File Offset: 0x000135E4
		public void Clear()
		{
			for (int i = 0; i < this.lines.Length; i++)
			{
				for (int j = 0; j < this.lines[i].Count; j++)
				{
					this.lines[i][j].Dispose();
				}
				this.lines[i].Clear();
				this.bullets[i].Visible = false;
			}
			this.currentLine = 0;
			this.currentPrintable = null;
			this.splitTextForLater = null;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00015460 File Offset: 0x00013660
		private void ClearTopLine()
		{
			for (int i = 0; i < this.lines[0].Count; i++)
			{
				this.lines[0][i].Dispose();
			}
			this.lines[0].Clear();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x000154A8 File Offset: 0x000136A8
		private void ShiftLinesUp()
		{
			this.ClearTopLine();
			List<Printable> list = this.lines[0];
			for (int i = 1; i < this.lines.Length; i++)
			{
				this.bullets[i - 1].Visible = this.bullets[i].Visible;
				this.lines[i - 1] = this.lines[i];
				for (int j = 0; j < this.lines[i].Count; j++)
				{
					this.lines[i][j].Position -= new Vector2f(0f, (float)this.fontData.LineHeight);
				}
			}
			this.bullets[this.bullets.Length - 1].Visible = false;
			this.lines[this.lines.Length - 1] = list;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0001557D File Offset: 0x0001377D
		private void CompletePrintAction()
		{
			if (this.OnTypewriterComplete != null)
			{
				this.OnTypewriterComplete(this, new EventArgs());
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00015598 File Offset: 0x00013798
		private void AdvanceLine()
		{
			if (this.currentLine + 1 >= this.lines.Length)
			{
				this.ShiftLinesUp();
				return;
			}
			this.currentLine++;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000155C1 File Offset: 0x000137C1
		public void PrintNewLine()
		{
			this.AdvanceLine();
			this.CompletePrintAction();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000155CF File Offset: 0x000137CF
		public void SetTextColor(Color color, bool isPrintAction)
		{
			this.textColor = color;
			if (isPrintAction)
			{
				this.CompletePrintAction();
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000155E4 File Offset: 0x000137E4
		public void SetTextSound(Typewriter.BlipSound soundType, bool isPrintAction)
		{
			if (soundType != this.textSoundType)
			{
				this.textSoundType = soundType;
				if (this.textSoundType != Typewriter.BlipSound.None)
				{
					this.textSound = AudioManager.Instance.Use(Paths.AUDIO + Typewriter.BLIP_SOUNDS[this.textSoundType], AudioType.Sound);
				}
				else
				{
					if (this.textSound != null)
					{
						AudioManager.Instance.Unuse(this.textSound);
					}
					this.textSound = null;
				}
			}
			if (isPrintAction)
			{
				this.CompletePrintAction();
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00015660 File Offset: 0x00013860
		private void Print(Printable printable)
		{
			List<Printable> list = this.lines[this.currentLine];
			float num = this.position.X - this.origin.X + this.bulletWidth;
			float num2 = 0f;
			if (list.Count > 0)
			{
				Renderable renderable = list[list.Count - 1];
				num = renderable.Position.X - renderable.Origin.X;
				num2 = renderable.Size.X;
			}
			printable.Position = new Vector2f((float)((int)(num + num2)), (float)((int)(this.position.Y - this.origin.Y + (float)(this.currentLine * this.fontData.LineHeight))));
			this.lines[this.currentLine].Add(printable);
			this.currentPrintable = printable;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00015734 File Offset: 0x00013934
		public void PrintText(string text)
		{
			string text2 = text;
			if (text2.Length > 0 && text2[0] == '@')
			{
				this.bullets[this.currentLine].Visible = this.showBullets;
				text2 = text2.Substring(1);
			}
			TextPrintable textPrintable = new TextPrintable(this.fontData, this.textSound, this.textColor, text2);
			if (!this.PrintableFitsInCurrentLine(textPrintable))
			{
				int pixelLength = (int)this.size.X - this.CalculateLineWidth(this.currentLine);
				this.splitTextForLater = textPrintable.SplitOffText(pixelLength);
			}
			if (textPrintable.Size.X > 0f)
			{
				this.Print(textPrintable);
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000157DC File Offset: 0x000139DC
		public void PrintGraphic(string subsprite)
		{
			GraphicPrintable graphicPrintable = new GraphicPrintable(subsprite);
			graphicPrintable.Origin = VectorMath.ZERO_VECTOR;
			if (!this.PrintableFitsInCurrentLine(graphicPrintable))
			{
				this.PrintNewLine();
			}
			this.Print(graphicPrintable);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00015814 File Offset: 0x00013A14
		public void PrintQuestion(string[] options)
		{
			QuestionPrintable printable = new QuestionPrintable(this.fontData, this.size.X, options);
			if (this.lines[this.currentLine].Count > 0)
			{
				this.PrintNewLine();
			}
			this.Print(printable);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0001585C File Offset: 0x00013A5C
		public void Update()
		{
			if (this.currentPrintable != null)
			{
				if (!this.currentPrintable.Complete)
				{
					this.currentPrintable.Update();
					return;
				}
				if (this.splitTextForLater != null)
				{
					this.AdvanceLine();
					string text = this.splitTextForLater;
					this.splitTextForLater = null;
					this.PrintText(text);
					this.currentPrintable.Update();
					return;
				}
				if (this.currentPrintable.Removable)
				{
					this.lines[this.currentLine].Remove(this.currentPrintable);
					this.currentPrintable.Dispose();
				}
				this.currentPrintable = null;
				this.CompletePrintAction();
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000158FC File Offset: 0x00013AFC
		public override void Draw(RenderTarget target)
		{
			for (int i = 0; i < this.lines.Length; i++)
			{
				if (this.bullets[i].Visible)
				{
					this.bullets[i].Draw(target);
				}
				for (int j = 0; j < this.lines[i].Count; j++)
				{
					this.lines[i][j].Draw(target);
				}
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00015968 File Offset: 0x00013B68
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					for (int i = 0; i < this.lines.Length; i++)
					{
						for (int j = 0; j < this.lines[i].Count; j++)
						{
							this.lines[i][j].Dispose();
						}
					}
				}
				AudioManager.Instance.Unuse(this.textSound);
				this.textSound = null;
				this.OnTypewriterComplete = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x040004D6 RID: 1238
		private const int DEPTH = 2147450880;

		// Token: 0x040004D7 RID: 1239
		private const char BULLET_CHAR = '@';

		// Token: 0x040004D8 RID: 1240
		private static readonly Dictionary<Typewriter.BlipSound, string> BLIP_SOUNDS = new Dictionary<Typewriter.BlipSound, string>
		{
			{
				Typewriter.BlipSound.Narration,
				"text1.wav"
			},
			{
				Typewriter.BlipSound.Male,
				"text2.wav"
			},
			{
				Typewriter.BlipSound.Female,
				"text3.wav"
			},
			{
				Typewriter.BlipSound.Awkward,
				"text4.wav"
			},
			{
				Typewriter.BlipSound.Robot,
				"text5.wav"
			}
		};

		// Token: 0x040004D9 RID: 1241
		private FontData fontData;

		// Token: 0x040004DA RID: 1242
		private Color textColor;

		// Token: 0x040004DB RID: 1243
		private List<Printable>[] lines;

		// Token: 0x040004DC RID: 1244
		private int currentLine;

		// Token: 0x040004DD RID: 1245
		private Printable currentPrintable;

		// Token: 0x040004DE RID: 1246
		private string splitTextForLater;

		// Token: 0x040004DF RID: 1247
		private Renderable[] bullets;

		// Token: 0x040004E0 RID: 1248
		private float bulletWidth;

		// Token: 0x040004E1 RID: 1249
		private bool showBullets;

		// Token: 0x040004E2 RID: 1250
		private Typewriter.BlipSound textSoundType;

		// Token: 0x040004E3 RID: 1251
		private CarbineSound textSound;

		// Token: 0x0200009D RID: 157
		public enum BlipSound
		{
			// Token: 0x040004E6 RID: 1254
			None,
			// Token: 0x040004E7 RID: 1255
			Narration,
			// Token: 0x040004E8 RID: 1256
			Male,
			// Token: 0x040004E9 RID: 1257
			Female,
			// Token: 0x040004EA RID: 1258
			Awkward,
			// Token: 0x040004EB RID: 1259
			Robot
		}

		// Token: 0x0200009E RID: 158
		// (Invoke) Token: 0x06000351 RID: 849
		public delegate void TypewriterCompleteHandler(object sender, EventArgs e);
	}
}
