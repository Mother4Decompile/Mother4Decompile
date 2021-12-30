using System;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using Mother4.GUI.Modifiers;
using SFML.System;

namespace Mother4.Battle.UI.Modifiers
{
	// Token: 0x02000020 RID: 32
	internal class GraphicShielder : IGraphicModifier, IDisposable
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004B3F File Offset: 0x00002D3F
		public bool Done
		{
			get
			{
				return this.isDone;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00004B47 File Offset: 0x00002D47
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004B50 File Offset: 0x00002D50
		public GraphicShielder(RenderPipeline pipeline, Graphic graphic)
		{
			this.pipeline = pipeline;
			this.graphic = graphic;
			this.shieldAnims = new AnimatedRenderable[GraphicShielder.SHIELD_POINTS.Length];
			for (int i = 0; i < this.shieldAnims.Length; i++)
			{
				this.shieldAnims[i] = new IndexedColorGraphic(Paths.PSI_GRAPHICS + "shield.dat", "bubble", this.graphic.Position + GraphicShielder.SHIELD_POINTS[i], this.graphic.Depth + 10);
				this.shieldAnims[i].Visible = false;
				this.shieldAnims[i].SpeedModifier = 0f;
				this.shieldAnims[i].OnAnimationComplete += this.OnAnimationComplete;
				this.pipeline.Add(this.shieldAnims[i]);
			}
			this.timerIndex = TimerManager.Instance.StartTimer(6);
			TimerManager.Instance.OnTimerEnd += this.OnTimerEnd;
			this.nextAnim = true;
			this.animIndex = 0;
			this.isDone = false;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004C74 File Offset: 0x00002E74
		~GraphicShielder()
		{
			this.Dispose(false);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004CA4 File Offset: 0x00002EA4
		public void Update()
		{
			if (!this.isDone && this.nextAnim)
			{
				this.nextAnim = false;
				this.shieldAnims[this.animIndex].SpeedModifier = 1f;
				this.shieldAnims[this.animIndex].Visible = true;
				this.animIndex++;
				if (this.animIndex < this.shieldAnims.Length)
				{
					this.timerIndex = TimerManager.Instance.StartTimer(6);
					return;
				}
				TimerManager.Instance.OnTimerEnd -= this.OnTimerEnd;
				this.isDone = true;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004D42 File Offset: 0x00002F42
		private void OnTimerEnd(int timerIndex)
		{
			if (this.timerIndex == timerIndex)
			{
				this.nextAnim = true;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004D54 File Offset: 0x00002F54
		private void OnAnimationComplete(AnimatedRenderable anim)
		{
			anim.Visible = false;
			anim.OnAnimationComplete -= this.OnAnimationComplete;
			this.pipeline.Remove(anim);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004D7B File Offset: 0x00002F7B
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004D8C File Offset: 0x00002F8C
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					for (int i = 0; i < this.shieldAnims.Length; i++)
					{
						this.shieldAnims[i].Dispose();
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x0400012D RID: 301
		private const int SHIELD_WAIT_FRAMES = 6;

		// Token: 0x0400012E RID: 302
		private static readonly Vector2f[] SHIELD_POINTS = new Vector2f[]
		{
			new Vector2f(16f, 12f),
			new Vector2f(45f, 43f),
			new Vector2f(16f, 35f),
			new Vector2f(45f, 20f)
		};

		// Token: 0x0400012F RID: 303
		private bool disposed;

		// Token: 0x04000130 RID: 304
		private bool isDone;

		// Token: 0x04000131 RID: 305
		private Graphic graphic;

		// Token: 0x04000132 RID: 306
		private RenderPipeline pipeline;

		// Token: 0x04000133 RID: 307
		private AnimatedRenderable[] shieldAnims;

		// Token: 0x04000134 RID: 308
		private int timerIndex;

		// Token: 0x04000135 RID: 309
		private bool nextAnim;

		// Token: 0x04000136 RID: 310
		private int animIndex;
	}
}
