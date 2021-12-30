using System;

namespace Carbine.Graphics
{
	// Token: 0x02000020 RID: 32
	public abstract class AnimatedRenderable : Renderable
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000061CA File Offset: 0x000043CA
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000061D2 File Offset: 0x000043D2
		public int Frames
		{
			get
			{
				return this.frames;
			}
			protected set
			{
				this.frames = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000061DB File Offset: 0x000043DB
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000061E3 File Offset: 0x000043E3
		public float Frame
		{
			get
			{
				return this.frame;
			}
			set
			{
				this.frame = Math.Max(0f, Math.Min((float)this.frames, value));
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00006202 File Offset: 0x00004402
		// (set) Token: 0x06000120 RID: 288 RVA: 0x0000620A File Offset: 0x0000440A
		public float[] Speeds
		{
			get
			{
				return this.speeds;
			}
			protected set
			{
				this.speeds = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00006213 File Offset: 0x00004413
		// (set) Token: 0x06000122 RID: 290 RVA: 0x0000621B File Offset: 0x0000441B
		public float SpeedModifier
		{
			get
			{
				return this.speedModifier;
			}
			set
			{
				this.speedModifier = value;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000123 RID: 291 RVA: 0x00006224 File Offset: 0x00004424
		// (remove) Token: 0x06000124 RID: 292 RVA: 0x0000625C File Offset: 0x0000445C
		public event AnimatedRenderable.AnimationCompleteHandler OnAnimationComplete;

		// Token: 0x06000125 RID: 293 RVA: 0x00006291 File Offset: 0x00004491
		protected void AnimationComplete()
		{
			if (this.OnAnimationComplete != null)
			{
				this.OnAnimationComplete(this);
			}
		}

		// Token: 0x040000A5 RID: 165
		protected int frames;

		// Token: 0x040000A6 RID: 166
		protected float frame;

		// Token: 0x040000A7 RID: 167
		protected float speedIndex;

		// Token: 0x040000A8 RID: 168
		protected float[] speeds;

		// Token: 0x040000A9 RID: 169
		protected float speedModifier;

		// Token: 0x02000021 RID: 33
		// (Invoke) Token: 0x06000128 RID: 296
		public delegate void AnimationCompleteHandler(AnimatedRenderable renderable);
	}
}
