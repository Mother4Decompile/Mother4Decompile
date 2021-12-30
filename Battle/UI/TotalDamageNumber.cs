using System;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x0200007D RID: 125
	internal class TotalDamageNumber : IDisposable
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000298 RID: 664 RVA: 0x000105A0 File Offset: 0x0000E7A0
		// (remove) Token: 0x06000299 RID: 665 RVA: 0x000105D8 File Offset: 0x0000E7D8
		public event TotalDamageNumber.CompletionHandler OnComplete;

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0001060D File Offset: 0x0000E80D
		public bool Done
		{
			get
			{
				return this.state == TotalDamageNumber.State.Finished;
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00010618 File Offset: 0x0000E818
		public TotalDamageNumber(RenderPipeline pipeline, Vector2f position, int number)
		{
			this.pipeline = pipeline;
			this.Reset(position, number);
			this.state = TotalDamageNumber.State.Waiting;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00010638 File Offset: 0x0000E838
		~TotalDamageNumber()
		{
			this.Dispose(false);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00010668 File Offset: 0x0000E868
		public void AddToNumber(int add)
		{
			this.number += add;
			this.Reset(this.position, this.number);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0001068C File Offset: 0x0000E88C
		public void Reset(Vector2f position, int number)
		{
			this.position = position;
			this.position.Y = Math.Max(12f, Math.Min(115f, this.position.Y));
			this.translation = default(Vector2f);
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
				this.numbers[j] = new IndexedColorGraphic(TotalDamageNumber.YELLOW_RESOURCE, "numbers", default(Vector2f), 32767);
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
			if (this.state == TotalDamageNumber.State.Finished)
			{
				this.timer = 0;
				this.paused = true;
				this.state = TotalDamageNumber.State.Waiting;
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00010874 File Offset: 0x0000EA74
		public void SetVisibility(bool visible)
		{
			for (int i = 0; i < this.numbers.Length; i++)
			{
				this.numbers[i].Visible = visible;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000108A4 File Offset: 0x0000EAA4
		public void Start()
		{
			this.paused = false;
			this.goal = this.position + TotalDamageNumber.UP_OFFSET;
			this.goal.Y = Math.Max(12f, Math.Min(115f, this.goal.Y));
			this.state = TotalDamageNumber.State.Rising;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00010900 File Offset: 0x0000EB00
		public void Update()
		{
			if (!this.paused)
			{
				if (this.state == TotalDamageNumber.State.Rising)
				{
					if (this.position.Y > this.goal.Y)
					{
						this.translation.Y = this.translation.Y - 0.2f;
						this.UpdatePosition();
						return;
					}
					this.timer = 0;
					this.translation = default(Vector2f);
					this.state = TotalDamageNumber.State.Hanging;
					return;
				}
				else if (this.state == TotalDamageNumber.State.Hanging)
				{
					if (this.timer < 38)
					{
						this.timer++;
						return;
					}
					this.timer = 0;
					this.goal = this.position + TotalDamageNumber.RIGHT_OFFSET;
					this.state = TotalDamageNumber.State.CleanUp;
					return;
				}
				else if (this.state == TotalDamageNumber.State.Exiting)
				{
					if (this.position.X < this.goal.X)
					{
						this.translation.X = this.translation.X + 0.2f;
						this.UpdatePosition();
						return;
					}
					this.timer = 0;
					this.translation = default(Vector2f);
					this.state = TotalDamageNumber.State.CleanUp;
					return;
				}
				else if (this.state == TotalDamageNumber.State.CleanUp)
				{
					this.timer = 0;
					this.translation = default(Vector2f);
					this.state = TotalDamageNumber.State.Finished;
					for (int i = 0; i < this.numbers.Length; i++)
					{
						this.pipeline.Remove(this.numbers[i]);
					}
					if (this.OnComplete != null)
					{
						this.OnComplete(this);
					}
				}
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00010A70 File Offset: 0x0000EC70
		private void UpdatePosition()
		{
			this.position += this.translation;
			for (int i = 0; i < this.numbers.Length; i++)
			{
				this.numbers[i].Position = new Vector2f((float)((int)(this.numbers[i].Position.X + this.translation.X)), (float)((int)(this.numbers[i].Position.Y + this.translation.Y)));
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00010AF9 File Offset: 0x0000ECF9
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00010B08 File Offset: 0x0000ED08
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

		// Token: 0x04000401 RID: 1025
		private const int PADDING = -1;

		// Token: 0x04000402 RID: 1026
		private const int RISE_HEIGHT = 24;

		// Token: 0x04000403 RID: 1027
		private const int HANG_TIME = 38;

		// Token: 0x04000404 RID: 1028
		private const float ACCELERATION = 0.2f;

		// Token: 0x04000405 RID: 1029
		private const float TOP_Y_BOUND = 12f;

		// Token: 0x04000406 RID: 1030
		private const float BOTTOM_Y_BOUND = 115f;

		// Token: 0x04000407 RID: 1031
		private static readonly string YELLOW_RESOURCE = Paths.GRAPHICS + "numberset2.dat";

		// Token: 0x04000408 RID: 1032
		private static readonly Vector2f UP_OFFSET = new Vector2f(0f, -32f);

		// Token: 0x04000409 RID: 1033
		private static readonly Vector2f RIGHT_OFFSET = new Vector2f(320f, 0f);

		// Token: 0x0400040A RID: 1034
		private bool disposed;

		// Token: 0x0400040B RID: 1035
		private Vector2f position;

		// Token: 0x0400040C RID: 1036
		private Vector2f goal;

		// Token: 0x0400040D RID: 1037
		private Vector2f translation;

		// Token: 0x0400040E RID: 1038
		private Graphic[] numbers;

		// Token: 0x0400040F RID: 1039
		private RenderPipeline pipeline;

		// Token: 0x04000410 RID: 1040
		private TotalDamageNumber.State state;

		// Token: 0x04000411 RID: 1041
		private int timer;

		// Token: 0x04000412 RID: 1042
		private int number;

		// Token: 0x04000413 RID: 1043
		private bool paused;

		// Token: 0x0200007E RID: 126
		private enum State
		{
			// Token: 0x04000416 RID: 1046
			Waiting,
			// Token: 0x04000417 RID: 1047
			Rising,
			// Token: 0x04000418 RID: 1048
			Hanging,
			// Token: 0x04000419 RID: 1049
			Exiting,
			// Token: 0x0400041A RID: 1050
			CleanUp,
			// Token: 0x0400041B RID: 1051
			Finished
		}

		// Token: 0x0200007F RID: 127
		// (Invoke) Token: 0x060002A7 RID: 679
		public delegate void CompletionHandler(TotalDamageNumber sender);
	}
}
