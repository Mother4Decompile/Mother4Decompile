using System;
using Carbine.Utility;
using SFML.Graphics;

namespace Carbine.GUI
{
	// Token: 0x02000034 RID: 52
	public class FontData : IDisposable
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00008FDC File Offset: 0x000071DC
		public Font Font
		{
			get
			{
				return this.font;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00008FE4 File Offset: 0x000071E4
		public int XCompensation
		{
			get
			{
				return this.xComp;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00008FEC File Offset: 0x000071EC
		public int YCompensation
		{
			get
			{
				return this.yComp;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00008FF4 File Offset: 0x000071F4
		public int LineHeight
		{
			get
			{
				return this.lineHeight;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00008FFC File Offset: 0x000071FC
		public int WHeight
		{
			get
			{
				return this.wHeight;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00009004 File Offset: 0x00007204
		public uint Size
		{
			get
			{
				return this.fontSize;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000900C File Offset: 0x0000720C
		public float AlphaThreshold
		{
			get
			{
				return this.alphaThreshold;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009014 File Offset: 0x00007214
		public FontData()
		{
			this.font = new Font(EmbeddedResources.GetStream("Carbine.Resources.openSansPX.ttf"));
			this.fontSize = 16U;
			this.wHeight = (int)this.font.GetGlyph(41U, this.fontSize, false).Bounds.Height;
			this.lineHeight = (int)((float)this.wHeight * 1.2f);
			this.alphaThreshold = 0f;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009088 File Offset: 0x00007288
		public FontData(Font font, uint fontSize, int lineHeight, int xComp, int yComp)
		{
			this.font = font;
			this.fontSize = fontSize;
			this.lineHeight = lineHeight;
			this.xComp = xComp;
			this.yComp = yComp;
			this.wHeight = (int)this.font.GetGlyph(41U, this.fontSize, false).Bounds.Height;
			this.alphaThreshold = 0.8f;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000090F4 File Offset: 0x000072F4
		~FontData()
		{
			this.Dispose(false);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009124 File Offset: 0x00007324
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.font.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009143 File Offset: 0x00007343
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000112 RID: 274
		private const uint W_CODE_POINT = 41U;

		// Token: 0x04000113 RID: 275
		private bool disposed;

		// Token: 0x04000114 RID: 276
		private Font font;

		// Token: 0x04000115 RID: 277
		private int xComp;

		// Token: 0x04000116 RID: 278
		private int yComp;

		// Token: 0x04000117 RID: 279
		private int lineHeight;

		// Token: 0x04000118 RID: 280
		private int wHeight;

		// Token: 0x04000119 RID: 281
		private uint fontSize;

		// Token: 0x0400011A RID: 282
		private float alphaThreshold;
	}
}
