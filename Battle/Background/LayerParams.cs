using System;

namespace Mother4.Battle.Background
{
	// Token: 0x020000C1 RID: 193
	public class LayerParams
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x0001A2EC File Offset: 0x000184EC
		public LayerParams()
		{
			this.Variation = new LayerVariation[8];
			for (int i = 0; i < 8; i++)
			{
				this.Variation[i].Mode = 0;
				this.Variation[i].A = 1f;
				this.Variation[i].B = 1f;
				this.Variation[i].C = 1f;
				this.Variation[i].D = 1f;
				this.Variation[i].E = 1f;
			}
			this.File = string.Empty;
			this.Name = string.Empty;
			this.Amplitude = 0f;
			this.Scale = 0f;
			this.Frequency = 0f;
			this.Compression = 0f;
			this.Speed = 0f;
			this.Opacity = 0f;
			this.Xtrans = 0f;
			this.Ytrans = 0f;
			this.Palette = new PaletteChange[0];
		}

		// Token: 0x040005F4 RID: 1524
		private const int LAYER_VARIATION_TYPE_COUNT = 8;

		// Token: 0x040005F5 RID: 1525
		public string File;

		// Token: 0x040005F6 RID: 1526
		public string Name;

		// Token: 0x040005F7 RID: 1527
		public int Mode;

		// Token: 0x040005F8 RID: 1528
		public int Blend;

		// Token: 0x040005F9 RID: 1529
		public float Amplitude;

		// Token: 0x040005FA RID: 1530
		public float Scale;

		// Token: 0x040005FB RID: 1531
		public float Frequency;

		// Token: 0x040005FC RID: 1532
		public float Compression;

		// Token: 0x040005FD RID: 1533
		public float Speed;

		// Token: 0x040005FE RID: 1534
		public float Opacity;

		// Token: 0x040005FF RID: 1535
		public float Xtrans;

		// Token: 0x04000600 RID: 1536
		public float Ytrans;

		// Token: 0x04000601 RID: 1537
		public LayerVariation[] Variation;

		// Token: 0x04000602 RID: 1538
		public PaletteChange[] Palette;
	}
}
