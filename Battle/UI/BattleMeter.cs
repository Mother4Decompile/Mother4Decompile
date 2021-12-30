using System;
using Carbine.Graphics;
using Mother4.Data;
using Mother4.GUI.Modifiers;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x020000DB RID: 219
	internal class BattleMeter : IDisposable
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0001F5A3 File Offset: 0x0001D7A3
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x0001F5AB File Offset: 0x0001D7AB
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001F5B4 File Offset: 0x0001D7B4
		public BattleMeter(RenderPipeline pipeline, Vector2f position, float initialFill, int depth)
		{
			this.targetFill = initialFill;
			this.fill = this.targetFill;
			this.meter = new IndexedColorGraphic(Paths.GRAPHICS + "battleui.dat", "meter", default(Vector2f), depth);
			this.meter.CurrentPalette = Settings.WindowFlavor;
			this.initialTextureRect = this.meter.TextureRect;
			this.hOffset = this.initialTextureRect.Height - (int)((float)this.initialTextureRect.Height * this.fill);
			this.position = position;
			this.meter.Position = new Vector2f(position.X, position.Y + (float)this.hOffset);
			this.meter.TextureRect = new IntRect(this.initialTextureRect.Left, this.initialTextureRect.Top + this.hOffset, this.initialTextureRect.Width, this.initialTextureRect.Height - this.hOffset);
			pipeline.Add(this.meter);
			this.fillMaxThreshold = 1f - 1f / (float)this.initialTextureRect.Height / 2f;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001F6F4 File Offset: 0x0001D8F4
		~BattleMeter()
		{
			this.Dispose(false);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001F724 File Offset: 0x0001D924
		public void SetFill(float newFill)
		{
			this.targetFill = Math.Max(0f, Math.Min(1f, newFill));
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001F744 File Offset: 0x0001D944
		public void SetGroovy(bool groovy)
		{
			bool flag = this.isGroovy;
			this.isGroovy = groovy;
			if (flag != this.isGroovy)
			{
				if (this.isGroovy)
				{
					this.flashFlag = false;
					this.fader = new GraphicFader(this.meter, BattleMeter.INITIAL_GLOW_COLOR, ColorBlendMode.Multiply, 6, 1);
					return;
				}
				this.fader = null;
				this.meter.Color = Color.White;
				this.meter.ColorBlendMode = ColorBlendMode.Multiply;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001F7B4 File Offset: 0x0001D9B4
		public void Update()
		{
			if (!this.isGroovy)
			{
				if (this.fill != this.targetFill)
				{
					this.fill += Math.Max(-1f, Math.Min(1f, this.targetFill - this.fill)) / 10f;
					this.hOffset = ((this.fill > this.fillMaxThreshold) ? 0 : (this.initialTextureRect.Height - (int)((float)this.initialTextureRect.Height * this.fill)));
					this.meter.TextureRect = new IntRect(this.initialTextureRect.Left, this.initialTextureRect.Top + this.hOffset, this.initialTextureRect.Width, this.initialTextureRect.Height - this.hOffset);
				}
				if (this.targetFill >= 0.999999f && this.fill > this.fillMaxThreshold)
				{
					this.SetGroovy(true);
				}
			}
			else if (this.fader != null)
			{
				this.fader.Update();
				if (this.fader.Done && !this.flashFlag)
				{
					this.flashFlag = true;
					this.fader = new GraphicFader(this.meter, BattleMeter.GLOW_COLOR, ColorBlendMode.Multiply, 6, -1);
				}
			}
			this.meter.Position = new Vector2f(this.position.X, this.position.Y + (float)this.hOffset);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001F92C File Offset: 0x0001DB2C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001F93B File Offset: 0x0001DB3B
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.meter.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x040006CF RID: 1743
		private const int HEIGHT = 55;

		// Token: 0x040006D0 RID: 1744
		private const float FILL_FACTOR = 10f;

		// Token: 0x040006D1 RID: 1745
		private const int INITIAL_GLOW_FRAMES = 6;

		// Token: 0x040006D2 RID: 1746
		private const int GLOW_FRAMES = 6;

		// Token: 0x040006D3 RID: 1747
		private static Color INITIAL_GLOW_COLOR = new Color(100, 100, 100);

		// Token: 0x040006D4 RID: 1748
		private static Color GLOW_COLOR = new Color(128, 128, 128);

		// Token: 0x040006D5 RID: 1749
		private bool disposed;

		// Token: 0x040006D6 RID: 1750
		private IntRect initialTextureRect;

		// Token: 0x040006D7 RID: 1751
		private IndexedColorGraphic meter;

		// Token: 0x040006D8 RID: 1752
		private float fill;

		// Token: 0x040006D9 RID: 1753
		private float targetFill;

		// Token: 0x040006DA RID: 1754
		private float fillMaxThreshold;

		// Token: 0x040006DB RID: 1755
		private int hOffset;

		// Token: 0x040006DC RID: 1756
		private Vector2f position;

		// Token: 0x040006DD RID: 1757
		private bool isGroovy;

		// Token: 0x040006DE RID: 1758
		private GraphicFader fader;

		// Token: 0x040006DF RID: 1759
		private bool flashFlag;
	}
}
