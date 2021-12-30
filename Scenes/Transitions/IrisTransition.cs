using System;
using Carbine;
using Carbine.Graphics;
using Carbine.Scenes.Transitions;
using Mother4.GUI;
using SFML.System;

namespace Mother4.Scenes.Transitions
{
	// Token: 0x02000117 RID: 279
	internal class IrisTransition : ITransition
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0002ADB1 File Offset: 0x00028FB1
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0002ADB9 File Offset: 0x00028FB9
		public float Progress
		{
			get
			{
				return this.progress;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0002ADC1 File Offset: 0x00028FC1
		public bool ShowNewScene
		{
			get
			{
				return this.progress > 0.5f;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0002ADD0 File Offset: 0x00028FD0
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0002ADD8 File Offset: 0x00028FD8
		public bool Blocking { get; set; }

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002ADE4 File Offset: 0x00028FE4
		public IrisTransition(float duration)
		{
			Vector2f origin = new Vector2f(160f, 90f);
			this.overlay = new IrisOverlay(ViewManager.Instance.FinalCenter, origin, 0f);
			float num = 60f * duration;
			this.speed = 1f / num;
			this.holdFrames = 30;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0002AE40 File Offset: 0x00029040
		public void Update()
		{
			if (!this.isComplete)
			{
				if (this.progress < 0.5f)
				{
					float num = 1f - this.progress / 0.5f;
					this.overlay.Progress = (float)(-(float)Math.Cos((double)num * 3.141592653589793) + 1.0) / 2f;
				}
				else if (this.holdTimer < this.holdFrames)
				{
					this.holdTimer++;
				}
				else
				{
					float num2 = (this.progress - 0.5f) / 0.5f;
					this.overlay.Progress = (float)(-(float)Math.Cos((double)num2 * 3.141592653589793) + 1.0) / 2f;
				}
				if (this.holdTimer == 0 || this.holdTimer >= this.holdFrames)
				{
					if (this.progress < 1f)
					{
						this.progress += this.speed;
						return;
					}
					this.isComplete = true;
				}
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002AF45 File Offset: 0x00029145
		public void Draw()
		{
			this.overlay.Draw(Engine.FrameBuffer);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002AF57 File Offset: 0x00029157
		public void Reset()
		{
			this.isComplete = false;
			this.progress = 0f;
			this.overlay.Progress = 1f;
		}

		// Token: 0x040008B9 RID: 2233
		private bool isComplete;

		// Token: 0x040008BA RID: 2234
		private float progress;

		// Token: 0x040008BB RID: 2235
		private float speed;

		// Token: 0x040008BC RID: 2236
		private int holdTimer;

		// Token: 0x040008BD RID: 2237
		private int holdFrames;

		// Token: 0x040008BE RID: 2238
		private IrisOverlay overlay;
	}
}
