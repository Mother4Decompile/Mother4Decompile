using System;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Graphics
{
	// Token: 0x0200002C RID: 44
	public class ShapeGraphic : Renderable
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00007960 File Offset: 0x00005B60
		public Shape Shape
		{
			get
			{
				return this.shape;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00007968 File Offset: 0x00005B68
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00007975 File Offset: 0x00005B75
		public override Vector2f Position
		{
			get
			{
				return this.shape.Position;
			}
			set
			{
				this.shape.Position = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00007983 File Offset: 0x00005B83
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00007990 File Offset: 0x00005B90
		public override Vector2f Origin
		{
			get
			{
				return this.shape.Origin;
			}
			set
			{
				this.shape.Origin = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000799E File Offset: 0x00005B9E
		// (set) Token: 0x06000189 RID: 393 RVA: 0x000079AB File Offset: 0x00005BAB
		public Color FillColor
		{
			get
			{
				return this.shape.FillColor;
			}
			set
			{
				this.shape.FillColor = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000079B9 File Offset: 0x00005BB9
		// (set) Token: 0x0600018B RID: 395 RVA: 0x000079C6 File Offset: 0x00005BC6
		public Color OutlineColor
		{
			get
			{
				return this.shape.OutlineColor;
			}
			set
			{
				this.shape.OutlineColor = value;
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000079D4 File Offset: 0x00005BD4
		public ShapeGraphic(Shape shape, Vector2f position, Vector2f origin, Vector2f size, int depth)
		{
			this.size = size;
			this.depth = depth;
			this.shape = shape;
			this.shape.Position = position;
			this.shape.Origin = origin;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007A0B File Offset: 0x00005C0B
		public override void Draw(RenderTarget target)
		{
			target.Draw(this.shape);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007A19 File Offset: 0x00005C19
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x040000E2 RID: 226
		private Shape shape;
	}
}
