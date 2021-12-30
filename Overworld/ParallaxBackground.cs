using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	// Token: 0x020000FF RID: 255
	internal class ParallaxBackground : TiledBackground
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x00022E6C File Offset: 0x0002106C
		// (set) Token: 0x060005DF RID: 1503 RVA: 0x00022E74 File Offset: 0x00021074
		public Vector2f Vector
		{
			get
			{
				return this.vector;
			}
			set
			{
				this.vector = value;
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00022E80 File Offset: 0x00021080
		public ParallaxBackground(string sprite, Vector2f vector, IntRect area, int depth) : base(sprite, area, true, true, VectorMath.ZERO_VECTOR, depth)
		{
			this.vector = vector;
			this.areaPoint = new Vector2f((float)area.Left, (float)area.Top);
			this.areaDimensions = new Vector2f((float)area.Width, (float)area.Height);
			this.position = this.areaPoint;
			this.size = this.areaDimensions;
			this.w = this.areaDimensions.X - this.texture.Image.Size.X;
			this.h = this.areaDimensions.Y - this.texture.Image.Size.Y;
			this.tw = (this.texture.Image.Size.X - 320U) / 2f;
			this.th = (this.texture.Image.Size.Y - 180U) / 2f;
			this.Update();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00022FA0 File Offset: 0x000211A0
		private void Update()
		{
			float num = ViewManager.Instance.FinalCenter.X - 160f;
			float num2 = ViewManager.Instance.FinalCenter.Y - 90f;
			this.previousPosition = this.position;
			this.position.X = num + (this.areaPoint.X - num) / this.w * (this.vector.X * this.tw);
			this.position.Y = num2 + (this.areaPoint.Y - num2) / this.h * (this.vector.Y * this.th);
			for (int i = 0; i < this.yRepeatCount; i++)
			{
				for (int j = 0; j < this.xRepeatCount; j++)
				{
					this.sprites[j, i].Position += this.position - this.previousPosition;
				}
			}
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0002309D File Offset: 0x0002129D
		public override void Draw(RenderTarget target)
		{
			this.Update();
			base.Draw(target);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x000230AC File Offset: 0x000212AC
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				TextureManager.Instance.Unuse(this.texture);
			}
			this.disposed = true;
		}

		// Token: 0x040007A4 RID: 1956
		private Vector2f previousPosition;

		// Token: 0x040007A5 RID: 1957
		private Vector2f vector;

		// Token: 0x040007A6 RID: 1958
		private Vector2f areaPoint;

		// Token: 0x040007A7 RID: 1959
		private Vector2f areaDimensions;

		// Token: 0x040007A8 RID: 1960
		private float w;

		// Token: 0x040007A9 RID: 1961
		private float h;

		// Token: 0x040007AA RID: 1962
		private float tw;

		// Token: 0x040007AB RID: 1963
		private float th;
	}
}
