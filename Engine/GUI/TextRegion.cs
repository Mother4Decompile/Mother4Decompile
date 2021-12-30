using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Carbine.GUI
{
	// Token: 0x02000035 RID: 53
	public class TextRegion : Renderable
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00009152 File Offset: 0x00007352
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000915C File Offset: 0x0000735C
		public override Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
				this.drawText.Position = new Vector2f(this.position.X + (float)this.font.XCompensation, this.position.Y + (float)this.font.YCompensation);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001ED RID: 493 RVA: 0x000091B0 File Offset: 0x000073B0
		// (set) Token: 0x060001EE RID: 494 RVA: 0x000091B8 File Offset: 0x000073B8
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
				this.dirtyText = true;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001EF RID: 495 RVA: 0x000091C8 File Offset: 0x000073C8
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x000091D0 File Offset: 0x000073D0
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
				this.dirtyText = true;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x000091E0 File Offset: 0x000073E0
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x000091E8 File Offset: 0x000073E8
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				int num = this.length;
				this.length = value;
				this.dirtyText = (this.length != num);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00009215 File Offset: 0x00007415
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x00009222 File Offset: 0x00007422
		public Color Color
		{
			get
			{
				return this.drawText.Color;
			}
			set
			{
				this.drawText.Color = value;
				this.dirtyColor = true;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00009237 File Offset: 0x00007437
		public FontData FontData
		{
			get
			{
				return this.font;
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000923F File Offset: 0x0000743F
		public TextRegion(Vector2f position, int depth, FontData font, string text) : this(position, depth, font, (text != null) ? text : string.Empty, 0, (text != null) ? text.Length : 0)
		{
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00009268 File Offset: 0x00007468
		public TextRegion(Vector2f position, int depth, FontData font, string text, int index, int length)
		{
			this.position = position;
			this.text = text;
			this.index = index;
			this.length = length;
			this.depth = depth;
			this.font = font;
			this.drawText = new Text(string.Empty, this.font.Font, this.font.Size);
			this.drawText.Position = new Vector2f(position.X + (float)this.font.XCompensation, position.Y + (float)this.font.YCompensation);
			this.UpdateText(index, length);
			this.shader = new Shader(EmbeddedResources.GetStream("Carbine.Resources.text.vert"), EmbeddedResources.GetStream("Carbine.Resources.text.frag"));
			this.shader.SetParameter("color", this.drawText.Color);
			this.shader.SetParameter("threshold", font.AlphaThreshold);
			this.renderStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, this.shader);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000937C File Offset: 0x0000757C
		public Vector2f FindCharacterPos(uint index)
		{
			uint num = Math.Max(0U, Math.Min((uint)this.text.Length, index));
			return this.drawText.FindCharacterPos(num);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000093AD File Offset: 0x000075AD
		public void Reset(string text, int index, int length)
		{
			this.text = text;
			this.index = index;
			this.length = length;
			this.UpdateText(index, length);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000093CC File Offset: 0x000075CC
		private void UpdateText(int index, int length)
		{
			this.drawText.DisplayedString = this.text.Substring(index, length);
			FloatRect localBounds = this.drawText.GetLocalBounds();
			this.size = new Vector2f(Math.Max(1f, localBounds.Width), Math.Max(1f, localBounds.Height));
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000942C File Offset: 0x0000762C
		public override void Draw(RenderTarget target)
		{
			if (this.dirtyText)
			{
				this.UpdateText(this.index, Math.Min(this.text.Length, this.length));
				this.dirtyText = false;
			}
			if (this.dirtyColor)
			{
				this.shader.SetParameter("color", this.drawText.Color);
			}
			target.Draw(this.drawText, this.renderStates);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000949F File Offset: 0x0000769F
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.drawText.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x0400011B RID: 283
		private Shader shader;

		// Token: 0x0400011C RID: 284
		private RenderStates renderStates;

		// Token: 0x0400011D RID: 285
		private Text drawText;

		// Token: 0x0400011E RID: 286
		private string text;

		// Token: 0x0400011F RID: 287
		private int index;

		// Token: 0x04000120 RID: 288
		private int length;

		// Token: 0x04000121 RID: 289
		private bool dirtyText;

		// Token: 0x04000122 RID: 290
		private bool dirtyColor;

		// Token: 0x04000123 RID: 291
		private FontData font;
	}
}
