using System;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x020000E6 RID: 230
	internal class OdometerRoller : IDisposable
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000550 RID: 1360 RVA: 0x00020D00 File Offset: 0x0001EF00
		// (remove) Token: 0x06000551 RID: 1361 RVA: 0x00020D38 File Offset: 0x0001EF38
		public event OdometerRoller.RollCompleteHandler OnRollComplete;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000552 RID: 1362 RVA: 0x00020D70 File Offset: 0x0001EF70
		// (remove) Token: 0x06000553 RID: 1363 RVA: 0x00020DA8 File Offset: 0x0001EFA8
		public event OdometerRoller.RollOverHandler OnRollover;

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00020DDD File Offset: 0x0001EFDD
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x00020DEA File Offset: 0x0001EFEA
		public Vector2f Position
		{
			get
			{
				return this.roller.Position;
			}
			set
			{
				this.roller.Position = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00020DF8 File Offset: 0x0001EFF8
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x00020E03 File Offset: 0x0001F003
		public int Number
		{
			get
			{
				return this.targetFrame / 9;
			}
			set
			{
				this.targetFrame = value * 9;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00020E0F File Offset: 0x0001F00F
		public int CurrentNumber
		{
			get
			{
				return this.frame / 9;
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00020E1C File Offset: 0x0001F01C
		public OdometerRoller(RenderPipeline pipeline, int initialNumber, Vector2f position, int depth)
		{
			this.pipeline = pipeline;
			this.frame = initialNumber * 9;
			this.targetFrame = this.frame;
			this.roller = new IndexedColorGraphic(Paths.GRAPHICS + "odometer.dat", "odometer", position, depth);
			this.roller.SpeedModifier = 0f;
			this.roller.Frame = (float)(this.frame % 90);
			pipeline.Add(this.roller);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00020EA0 File Offset: 0x0001F0A0
		~OdometerRoller()
		{
			this.Dispose(false);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00020ED0 File Offset: 0x0001F0D0
		public void Hide()
		{
			if (!this.hidden)
			{
				this.roller.Visible = false;
				this.hidden = true;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00020EED File Offset: 0x0001F0ED
		public void Show()
		{
			if (this.hidden)
			{
				this.roller.Visible = true;
				this.hidden = false;
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00020F0A File Offset: 0x0001F10A
		public void DoEntireRoll()
		{
			this.rolling = true;
			this.targetStepFrame = -1;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00020F1C File Offset: 0x0001F11C
		public void StepRoll()
		{
			this.rolling = true;
			int num = Math.Max(-1, Math.Min(1, this.targetFrame - this.frame));
			this.targetStepFrame = this.frame + 9 * num;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00020F5C File Offset: 0x0001F15C
		public void Update()
		{
			if (this.rolling)
			{
				int num = Math.Max(-1, Math.Min(1, this.targetFrame - this.frame));
				this.frame += num;
				this.roller.Frame = (float)(this.frame % 90);
				if ((num > 0 && this.frame % 90 == 82) || (num < 0 && this.frame % 90 == 89))
				{
					this.rollCount += num;
					if (this.OnRollover != null)
					{
						this.OnRollover();
					}
				}
				if (this.OnRollComplete != null && this.frame == this.targetFrame)
				{
					this.OnRollComplete();
					this.rolling = false;
				}
				if (this.frame == this.targetStepFrame)
				{
					this.rolling = false;
				}
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00021031 File Offset: 0x0001F231
		public void ForceNumber(int number)
		{
			this.roller.Frame = (float)(number * 9);
			this.targetFrame = (int)this.roller.Frame;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00021055 File Offset: 0x0001F255
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00021064 File Offset: 0x0001F264
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.pipeline.Remove(this.roller);
					this.roller.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x04000723 RID: 1827
		private const int FRAMES_PER_NUMBER = 9;

		// Token: 0x04000724 RID: 1828
		private const int TOTAL_FRAMES = 90;

		// Token: 0x04000725 RID: 1829
		private bool disposed;

		// Token: 0x04000726 RID: 1830
		private RenderPipeline pipeline;

		// Token: 0x04000727 RID: 1831
		private Graphic roller;

		// Token: 0x04000728 RID: 1832
		private int frame;

		// Token: 0x04000729 RID: 1833
		private int targetFrame;

		// Token: 0x0400072A RID: 1834
		private int targetStepFrame;

		// Token: 0x0400072B RID: 1835
		private int rollCount;

		// Token: 0x0400072C RID: 1836
		private bool rolling;

		// Token: 0x0400072D RID: 1837
		private bool hidden;

		// Token: 0x020000E7 RID: 231
		// (Invoke) Token: 0x06000564 RID: 1380
		public delegate void RollCompleteHandler();

		// Token: 0x020000E8 RID: 232
		// (Invoke) Token: 0x06000568 RID: 1384
		public delegate void RollOverHandler();
	}
}
