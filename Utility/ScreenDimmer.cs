using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Utility
{
	// Token: 0x02000177 RID: 375
	internal class ScreenDimmer : IDisposable
	{
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060007EB RID: 2027 RVA: 0x00032F54 File Offset: 0x00031154
		// (remove) Token: 0x060007EC RID: 2028 RVA: 0x00032F8C File Offset: 0x0003118C
		public event ScreenDimmer.OnFadeCompleteHandler OnFadeComplete;

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00032FC1 File Offset: 0x000311C1
		public Color TargetColor
		{
			get
			{
				return this.targetColor;
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00032FCC File Offset: 0x000311CC
		public ScreenDimmer(RenderPipeline pipeline, Color color, int duration, int depth)
		{
			this.pipeline = pipeline;
			this.ChangeColor(Color.Transparent, color, duration);
			this.rect = new RectangleShape(new Vector2f(320f, 180f));
			this.shape = new ShapeGraphic(this.rect, ViewManager.Instance.FinalCenter, new Vector2f((float)((int)(this.rect.Size.X / 2f)), (float)((int)(this.rect.Size.Y / 2f))), this.rect.Size, depth);
			this.rect.FillColor = this.currentColor;
			this.pipeline.Add(this.shape);
			ViewManager.Instance.OnMove += this.OnViewMove;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000330A4 File Offset: 0x000312A4
		~ScreenDimmer()
		{
			this.Dispose(false);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000330D4 File Offset: 0x000312D4
		private void ChangeColor(Color fromColor, Color toColor, int duration)
		{
			if (duration > 0)
			{
				this.currentColor = fromColor;
				this.targetColor = toColor;
				this.speed = 1f / (float)duration;
				this.progress = 0f;
				this.isTransitioning = true;
				return;
			}
			this.currentColor = toColor;
			this.targetColor = toColor;
			this.speed = 0f;
			this.progress = 1f;
			this.isTransitioning = false;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0003313F File Offset: 0x0003133F
		public void ChangeColor(Color color, int duration)
		{
			this.ChangeColor(ColorHelper.BlendAlpha(this.currentColor, this.targetColor, this.progress), color, duration);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00033160 File Offset: 0x00031360
		private void OnViewMove(ViewManager sender, Vector2f newCenter)
		{
			this.rect.Position = sender.FinalCenter;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00033174 File Offset: 0x00031374
		public void Update()
		{
			if (this.progress < 1f)
			{
				this.rect.FillColor = ColorHelper.BlendAlpha(this.currentColor, this.targetColor, this.progress);
				this.progress += this.speed;
				return;
			}
			if (this.isTransitioning)
			{
				this.rect.FillColor = this.targetColor;
				this.isTransitioning = false;
				if (this.OnFadeComplete != null)
				{
					this.OnFadeComplete(this);
				}
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000331F8 File Offset: 0x000313F8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00033207 File Offset: 0x00031407
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.shape.Dispose();
					this.rect.Dispose();
				}
				ViewManager.Instance.OnMove -= this.OnViewMove;
				this.disposed = true;
			}
		}

		// Token: 0x0400098B RID: 2443
		private bool disposed;

		// Token: 0x0400098C RID: 2444
		private RenderPipeline pipeline;

		// Token: 0x0400098D RID: 2445
		private ShapeGraphic shape;

		// Token: 0x0400098E RID: 2446
		private RectangleShape rect;

		// Token: 0x0400098F RID: 2447
		private bool isTransitioning;

		// Token: 0x04000990 RID: 2448
		private float progress;

		// Token: 0x04000991 RID: 2449
		private float speed;

		// Token: 0x04000992 RID: 2450
		private Color currentColor;

		// Token: 0x04000993 RID: 2451
		private Color targetColor;

		// Token: 0x02000178 RID: 376
		// (Invoke) Token: 0x060007F7 RID: 2039
		public delegate void OnFadeCompleteHandler(ScreenDimmer sender);
	}
}
