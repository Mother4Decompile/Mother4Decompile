using System;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x020000E5 RID: 229
	internal class Odometer : IDisposable
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0002098A File Offset: 0x0001EB8A
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x00020992 File Offset: 0x0001EB92
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
				this.posDirty = true;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x000209A2 File Offset: 0x0001EBA2
		public int TargetValue
		{
			get
			{
				return this.rollers[0].Number;
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000209B4 File Offset: 0x0001EBB4
		public Odometer(RenderPipeline pipeline, Vector2f position, int depth, int places, int initialValue, int maxValue)
		{
			this.rollerContainer = new IndexedColorGraphic(Paths.GRAPHICS + "battleui.dat", "odometer", position - new Vector2f(1f, 1f), depth - 1);
			pipeline.Add(this.rollerContainer);
			this.rollers = new OdometerRoller[places];
			this.hidden = new bool[this.rollers.Length];
			for (int i = 0; i < this.rollers.Length; i++)
			{
				int num = (int)Math.Pow(10.0, (double)i);
				int initialNumber = initialValue / num;
				this.rollers[i] = new OdometerRoller(pipeline, initialNumber, position + new Vector2f((float)(8 * (places - 1 - i)), 0f), depth);
				if (i > 0)
				{
					this.rollers[i - 1].OnRollover += this.rollers[i].StepRoll;
				}
			}
			this.holdPlaces = Digits.CountDigits(maxValue) - ((maxValue == 0) ? 1 : 0);
			this.places = places;
			this.position = position;
			this.Update();
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00020AD0 File Offset: 0x0001ECD0
		~Odometer()
		{
			this.Dispose(false);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00020B00 File Offset: 0x0001ED00
		public void Update()
		{
			for (int i = 0; i < this.rollers.Length; i++)
			{
				if (this.posDirty)
				{
					this.rollers[i].Position = this.Position + new Vector2f((float)(8 * (this.places - 1 - i)), 0f);
					this.rollerContainer.Position = this.Position - new Vector2f(1f, 1f);
				}
				this.rollers[i].Update();
				if (i >= this.holdPlaces && this.rollers[i].CurrentNumber == 0 && this.rollers[i].Number == 0 && (i + 1 >= this.rollers.Length || this.hidden[i + 1]))
				{
					if (!this.hidden[i])
					{
						this.hidden[i] = true;
						this.rollers[i].Hide();
					}
				}
				else if (this.hidden[i])
				{
					this.hidden[i] = false;
					this.rollers[i].Show();
				}
			}
			this.posDirty = false;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00020C18 File Offset: 0x0001EE18
		public void SetValue(int newValue)
		{
			Console.Write("odometer setvalue: ");
			for (int i = 0; i < this.rollers.Length; i++)
			{
				int num = newValue / (int)Math.Pow(10.0, (double)i);
				this.rollers[i].Number = num;
				Console.Write("{0} ", num);
			}
			Console.WriteLine();
			int num2 = Digits.CountDigits(newValue) - ((newValue == 0) ? 1 : 0);
			if (num2 > this.holdPlaces)
			{
				this.holdPlaces = num2;
			}
			this.rollers[0].DoEntireRoll();
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00020CA5 File Offset: 0x0001EEA5
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00020CB4 File Offset: 0x0001EEB4
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.rollerContainer.Dispose();
					foreach (OdometerRoller odometerRoller in this.rollers)
					{
						odometerRoller.Dispose();
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x0400071B RID: 1819
		private bool disposed;

		// Token: 0x0400071C RID: 1820
		private Graphic rollerContainer;

		// Token: 0x0400071D RID: 1821
		private OdometerRoller[] rollers;

		// Token: 0x0400071E RID: 1822
		private int places;

		// Token: 0x0400071F RID: 1823
		private int holdPlaces;

		// Token: 0x04000720 RID: 1824
		private Vector2f position;

		// Token: 0x04000721 RID: 1825
		private bool posDirty;

		// Token: 0x04000722 RID: 1826
		private bool[] hidden;
	}
}
