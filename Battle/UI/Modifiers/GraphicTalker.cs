using System;
using Carbine.Graphics;
using Mother4.Data;
using Mother4.GUI.Modifiers;
using SFML.System;

namespace Mother4.Battle.UI.Modifiers
{
	// Token: 0x02000094 RID: 148
	internal class GraphicTalker : IGraphicModifier, IDisposable
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00013166 File Offset: 0x00011366
		public bool Done
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0001316E File Offset: 0x0001136E
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00013178 File Offset: 0x00011378
		public GraphicTalker(RenderPipeline pipeline, Graphic graphic)
		{
			this.pipeline = pipeline;
			this.graphic = graphic;
			this.done = false;
			this.rightOffset = new Vector2f((float)this.graphic.TextureRect.Width, 0f);
			this.talker = new IndexedColorGraphic(Paths.GRAPHICS + "chat.dat", "left", this.graphic.Position - this.graphic.Origin, this.graphic.Depth + 1);
			this.talker.OnAnimationComplete += this.AnimationComplete;
			this.pipeline.Add(this.talker);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00013230 File Offset: 0x00011430
		~GraphicTalker()
		{
			this.Dispose(false);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00013260 File Offset: 0x00011460
		private void AnimationComplete(AnimatedRenderable graphic)
		{
			this.count++;
			if (this.count > 2)
			{
				this.right = !this.right;
				this.talker.SetSprite(this.right ? "right" : "left", true);
				this.count = 0;
				this.Update();
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000132C0 File Offset: 0x000114C0
		public void Update()
		{
			if (this.right)
			{
				this.talker.Position = this.graphic.Position - this.graphic.Origin + this.rightOffset;
				return;
			}
			this.talker.Position = this.graphic.Position - this.graphic.Origin;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0001332D File Offset: 0x0001152D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0001333C File Offset: 0x0001153C
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.pipeline.Remove(this.talker);
					this.talker.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x0400046A RID: 1130
		private const int LOOP_TOTAL = 2;

		// Token: 0x0400046B RID: 1131
		private bool disposed;

		// Token: 0x0400046C RID: 1132
		private RenderPipeline pipeline;

		// Token: 0x0400046D RID: 1133
		private Graphic graphic;

		// Token: 0x0400046E RID: 1134
		private IndexedColorGraphic talker;

		// Token: 0x0400046F RID: 1135
		private Vector2f rightOffset;

		// Token: 0x04000470 RID: 1136
		private int count;

		// Token: 0x04000471 RID: 1137
		private bool right;

		// Token: 0x04000472 RID: 1138
		private bool done;
	}
}
