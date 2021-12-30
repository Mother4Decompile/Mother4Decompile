using System;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x020000E2 RID: 226
	internal class DamageNumber : IDisposable
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000532 RID: 1330 RVA: 0x0002047C File Offset: 0x0001E67C
		// (remove) Token: 0x06000533 RID: 1331 RVA: 0x000204B4 File Offset: 0x0001E6B4
		public event DamageNumber.CompletionHandler OnComplete;

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x000204E9 File Offset: 0x0001E6E9
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x000204F1 File Offset: 0x0001E6F1
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x000204FA File Offset: 0x0001E6FA
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x00020502 File Offset: 0x0001E702
		public Vector2f Goal
		{
			get
			{
				return this.goal;
			}
			set
			{
				this.goal = value;
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0002050C File Offset: 0x0001E70C
		public DamageNumber(RenderPipeline pipeline, Vector2f position, Vector2f offset, int hangTime, int number)
		{
			this.pipeline = pipeline;
			this.position = position;
			this.goal = position + offset;
			this.hangTime = hangTime;
			this.number = number;
			this.Reset(this.position, number);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00020558 File Offset: 0x0001E758
		~DamageNumber()
		{
			this.Dispose(false);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00020588 File Offset: 0x0001E788
		public void AddToNumber(int add)
		{
			this.number += add;
			this.Reset(this.position, this.number);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000205AC File Offset: 0x0001E7AC
		public void Reset(Vector2f position, int number)
		{
			this.position = position;
			this.number = number;
			if (this.numbers != null && this.numbers.Length > 0)
			{
				for (int i = 0; i < this.numbers.Length; i++)
				{
					this.pipeline.Remove(this.numbers[i]);
					this.numbers[i].Dispose();
				}
			}
			int num = Digits.CountDigits(number);
			this.numbers = new Graphic[num];
			int num2 = 0;
			int num3 = 0;
			for (int j = 0; j < this.numbers.Length; j++)
			{
				this.numbers[j] = new IndexedColorGraphic(DamageNumber.RESOURCE, "numbers", default(Vector2f), 32767);
				this.numbers[j].Frame = (float)Digits.Get(number, this.numbers.Length - j);
				this.numbers[j].Visible = false;
				num2 += this.numbers[j].TextureRect.Width + -1;
				num3 = Math.Max(num3, this.numbers[j].TextureRect.Height);
				this.pipeline.Add(this.numbers[j]);
			}
			num2 -= -1;
			int num4 = num2 / 2;
			int num5 = num3 / 2;
			for (int k = 0; k < this.numbers.Length; k++)
			{
				this.numbers[k].Position = this.position - new Vector2f((float)num4, (float)num5);
				num4 -= this.numbers[k].TextureRect.Width + -1;
			}
			this.state = DamageNumber.State.Moving;
			this.timer = 0;
			this.paused = true;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00020758 File Offset: 0x0001E958
		public void SetVisibility(bool visible)
		{
			for (int i = 0; i < this.numbers.Length; i++)
			{
				this.numbers[i].Visible = visible;
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00020786 File Offset: 0x0001E986
		public void Pause()
		{
			this.paused = true;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0002078F File Offset: 0x0001E98F
		public void Start()
		{
			this.paused = false;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00020798 File Offset: 0x0001E998
		public virtual void Update()
		{
			if (!this.paused)
			{
				if (this.state == DamageNumber.State.Moving)
				{
					this.translation = new Vector2f(Math.Min(25f, this.goal.X - this.position.X), Math.Min(25f, this.goal.Y - this.position.Y));
					if (this.translation.X > 1f || this.translation.X < -1f || this.translation.Y > 1f || this.translation.Y < -1f)
					{
						this.position += this.translation * 0.25f;
						for (int i = 0; i < this.numbers.Length; i++)
						{
							this.numbers[i].Position += this.translation * 0.25f;
						}
						return;
					}
					this.state = DamageNumber.State.Waiting;
					return;
				}
				else if (this.state == DamageNumber.State.Waiting)
				{
					this.timer++;
					if (this.timer > this.hangTime)
					{
						this.state = DamageNumber.State.CleanUp;
						return;
					}
				}
				else if (this.state == DamageNumber.State.CleanUp)
				{
					for (int j = 0; j < this.numbers.Length; j++)
					{
						this.pipeline.Remove(this.numbers[j]);
					}
					if (this.OnComplete != null)
					{
						this.OnComplete(this);
					}
					this.state = DamageNumber.State.Done;
				}
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0002092C File Offset: 0x0001EB2C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0002093C File Offset: 0x0001EB3C
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				for (int i = 0; i < this.numbers.Length; i++)
				{
					this.numbers[i].Dispose();
				}
			}
		}

		// Token: 0x04000708 RID: 1800
		private const int PADDING = -1;

		// Token: 0x04000709 RID: 1801
		private static readonly string RESOURCE = Paths.GRAPHICS + "numberset1.dat";

		// Token: 0x0400070A RID: 1802
		private bool disposed;

		// Token: 0x0400070B RID: 1803
		private int number;

		// Token: 0x0400070C RID: 1804
		private int timer;

		// Token: 0x0400070D RID: 1805
		private int hangTime;

		// Token: 0x0400070E RID: 1806
		private Vector2f position;

		// Token: 0x0400070F RID: 1807
		private Vector2f goal;

		// Token: 0x04000710 RID: 1808
		private Vector2f translation;

		// Token: 0x04000711 RID: 1809
		private Graphic[] numbers;

		// Token: 0x04000712 RID: 1810
		private RenderPipeline pipeline;

		// Token: 0x04000713 RID: 1811
		private DamageNumber.State state;

		// Token: 0x04000714 RID: 1812
		private bool paused;

		// Token: 0x020000E3 RID: 227
		private enum State
		{
			// Token: 0x04000717 RID: 1815
			Moving,
			// Token: 0x04000718 RID: 1816
			Waiting,
			// Token: 0x04000719 RID: 1817
			CleanUp,
			// Token: 0x0400071A RID: 1818
			Done
		}

		// Token: 0x020000E4 RID: 228
		// (Invoke) Token: 0x06000544 RID: 1348
		public delegate void CompletionHandler(DamageNumber sender);
	}
}
