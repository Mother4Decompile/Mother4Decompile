using System;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x02000073 RID: 115
	internal class Groovy : IDisposable
	{
		// Token: 0x06000275 RID: 629 RVA: 0x0000FA98 File Offset: 0x0000DC98
		public Groovy(RenderPipeline pipeline, Vector2f position)
		{
			this.pipeline = pipeline;
			this.groovy = new IndexedColorGraphic(Paths.GRAPHICS + "groovy.dat", "groovy", position, 32767);
			this.groovy.OnAnimationComplete += this.AnimationComplete;
			this.pipeline.Add(this.groovy);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000FB00 File Offset: 0x0000DD00
		~Groovy()
		{
			this.Dispose(false);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000FB30 File Offset: 0x0000DD30
		private void AnimationComplete(AnimatedRenderable graphic)
		{
			this.groovy.SpeedModifier = 0f;
			this.groovy.Frame = (float)(this.groovy.Frames - 1);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000FB5B File Offset: 0x0000DD5B
		public void Update()
		{
			if (!this.done)
			{
				this.timer++;
				if (this.timer > 60)
				{
					this.pipeline.Remove(this.groovy);
					this.done = true;
				}
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000FB95 File Offset: 0x0000DD95
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000FBA4 File Offset: 0x0000DDA4
		protected void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				if (!this.done)
				{
					this.pipeline.Remove(this.groovy);
				}
				this.groovy.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x040003DB RID: 987
		private const int TIME_TO_LIVE = 60;

		// Token: 0x040003DC RID: 988
		private bool disposed;

		// Token: 0x040003DD RID: 989
		private Graphic groovy;

		// Token: 0x040003DE RID: 990
		private RenderPipeline pipeline;

		// Token: 0x040003DF RID: 991
		private int timer;

		// Token: 0x040003E0 RID: 992
		private bool done;
	}
}
