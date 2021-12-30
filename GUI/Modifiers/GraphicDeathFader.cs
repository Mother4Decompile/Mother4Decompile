using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;

namespace Mother4.GUI.Modifiers
{
	// Token: 0x02000093 RID: 147
	internal class GraphicDeathFader : IGraphicModifier
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00013055 File Offset: 0x00011255
		public bool Done
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0001305D File Offset: 0x0001125D
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00013065 File Offset: 0x00011265
		public GraphicDeathFader(IndexedColorGraphic graphic, int frames)
		{
			this.graphic = graphic;
			this.graphic.Color = Color.Black;
			this.graphic.ColorBlendMode = ColorBlendMode.Screen;
			this.speed = 2f / (float)frames;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000130A0 File Offset: 0x000112A0
		public void Update()
		{
			if (!this.done)
			{
				if (this.progress < 1f)
				{
					this.graphic.Color = ColorHelper.Blend(Color.Black, Color.White, this.progress);
				}
				else if (this.progress < 2f)
				{
					if (this.progress < 1f + this.speed)
					{
						this.graphic.ColorBlendMode = ColorBlendMode.Replace;
						this.graphic.Color = Color.White;
					}
					this.graphic.Color = ColorHelper.Blend(Color.White, Color.Black, this.progress - 1f);
				}
				else
				{
					this.done = true;
				}
				this.progress += this.speed;
			}
		}

		// Token: 0x04000466 RID: 1126
		private bool done;

		// Token: 0x04000467 RID: 1127
		private IndexedColorGraphic graphic;

		// Token: 0x04000468 RID: 1128
		private float progress;

		// Token: 0x04000469 RID: 1129
		private float speed;
	}
}
