using System;
using Carbine.Graphics;

namespace Mother4.GUI.Modifiers
{
	// Token: 0x0200008F RID: 143
	internal class GraphicBlinker : IGraphicModifier
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00012A52 File Offset: 0x00010C52
		public bool Done
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00012A5A File Offset: 0x00010C5A
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00012A62 File Offset: 0x00010C62
		public GraphicBlinker(Graphic graphic, int duration, int count)
		{
			this.graphic = graphic;
			this.initialVisibility = graphic.Visible;
			this.duration = duration;
			this.total = count * 2;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00012A90 File Offset: 0x00010C90
		public void Update()
		{
			if (this.count >= this.total && this.total >= 0)
			{
				if (!this.done)
				{
					this.graphic.Visible = this.initialVisibility;
					this.done = true;
				}
				return;
			}
			if (this.timer < this.duration)
			{
				this.timer++;
				return;
			}
			this.count++;
			this.graphic.Visible = !this.graphic.Visible;
			this.timer = 0;
		}

		// Token: 0x0400043B RID: 1083
		private Graphic graphic;

		// Token: 0x0400043C RID: 1084
		private bool initialVisibility;

		// Token: 0x0400043D RID: 1085
		private int count;

		// Token: 0x0400043E RID: 1086
		private int total;

		// Token: 0x0400043F RID: 1087
		private int timer;

		// Token: 0x04000440 RID: 1088
		private int duration;

		// Token: 0x04000441 RID: 1089
		private bool done;
	}
}
