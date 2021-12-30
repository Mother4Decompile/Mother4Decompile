using System;
using Carbine.Graphics;
using SFML.System;

namespace Mother4.GUI.Modifiers
{
	// Token: 0x02000095 RID: 149
	internal class GraphicTranslator : IGraphicModifier
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0001336C File Offset: 0x0001156C
		public bool Done
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00013374 File Offset: 0x00011574
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0001337C File Offset: 0x0001157C
		public GraphicTranslator(Graphic graphic, Vector2f target, int frames)
		{
			this.graphic = graphic;
			this.target = target;
			this.moveVector = (this.target - this.graphic.Position) / (float)frames;
			this.done = false;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x000133BC File Offset: 0x000115BC
		public void Update()
		{
			float value = this.target.X - this.graphic.Position.X;
			float value2 = this.target.Y - this.graphic.Position.Y;
			this.done = (Math.Abs(value) <= 1f && Math.Abs(value2) <= 1f);
			if (!this.done)
			{
				this.graphic.Position += this.moveVector;
				return;
			}
			if (!this.cleanupFlag)
			{
				this.graphic.Position = this.target;
				this.cleanupFlag = true;
			}
		}

		// Token: 0x04000473 RID: 1139
		private Graphic graphic;

		// Token: 0x04000474 RID: 1140
		private Vector2f target;

		// Token: 0x04000475 RID: 1141
		private Vector2f moveVector;

		// Token: 0x04000476 RID: 1142
		private bool done;

		// Token: 0x04000477 RID: 1143
		private bool cleanupFlag;
	}
}
