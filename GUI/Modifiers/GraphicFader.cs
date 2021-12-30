using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;

namespace Mother4.GUI.Modifiers
{
	// Token: 0x02000092 RID: 146
	internal class GraphicFader : IGraphicModifier
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00012E8A File Offset: 0x0001108A
		public bool Done
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00012E92 File Offset: 0x00011092
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00012E9C File Offset: 0x0001109C
		public GraphicFader(IndexedColorGraphic graphic, Color color, ColorBlendMode blendMode, int duration, int count)
		{
			this.graphic = graphic;
			this.blendMode = blendMode;
			this.baseBlendMode = this.graphic.ColorBlendMode;
			this.baseColor = ((blendMode == ColorBlendMode.Screen) ? this.graphic.Color.Invert() : this.graphic.Color);
			this.color = color;
			this.duration = duration;
			this.total = count * 2;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00012F10 File Offset: 0x00011110
		public void Update()
		{
			if (!this.firstStep)
			{
				this.graphic.ColorBlendMode = this.blendMode;
				this.graphic.Color = this.baseColor;
				this.firstStep = true;
			}
			if (this.count >= this.total && this.total >= 0)
			{
				if (!this.done)
				{
					this.graphic.Color = ((this.blendMode == ColorBlendMode.Screen) ? this.baseColor.Invert() : this.baseColor);
					this.graphic.ColorBlendMode = this.baseBlendMode;
					this.done = true;
				}
				return;
			}
			if (this.timer < this.duration)
			{
				this.graphic.Color = ColorHelper.Blend((this.count % 2 == 0) ? this.baseColor : this.color, (this.count % 2 == 1) ? this.baseColor : this.color, (float)this.timer / (float)this.duration);
				this.timer++;
				return;
			}
			this.count++;
			this.graphic.Color = ((this.count % 2 == 0) ? this.baseColor : this.color);
			this.timer = 0;
		}

		// Token: 0x0400045B RID: 1115
		private IndexedColorGraphic graphic;

		// Token: 0x0400045C RID: 1116
		private Color baseColor;

		// Token: 0x0400045D RID: 1117
		private Color color;

		// Token: 0x0400045E RID: 1118
		private ColorBlendMode baseBlendMode;

		// Token: 0x0400045F RID: 1119
		private ColorBlendMode blendMode;

		// Token: 0x04000460 RID: 1120
		private int count;

		// Token: 0x04000461 RID: 1121
		private int total;

		// Token: 0x04000462 RID: 1122
		private int timer;

		// Token: 0x04000463 RID: 1123
		private int duration;

		// Token: 0x04000464 RID: 1124
		private bool done;

		// Token: 0x04000465 RID: 1125
		private bool firstStep;
	}
}
