using System;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.Text.Printables
{
	// Token: 0x0200004A RID: 74
	internal class GraphicPrintable : Printable
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000AC63 File Offset: 0x00008E63
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x0000AC70 File Offset: 0x00008E70
		public override Vector2f Position
		{
			get
			{
				return this.graphic.Position;
			}
			set
			{
				this.graphic.Position = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000AC7E File Offset: 0x00008E7E
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x0000AC8B File Offset: 0x00008E8B
		public override Vector2f Origin
		{
			get
			{
				return this.graphic.Origin;
			}
			set
			{
				this.graphic.Origin = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000AC99 File Offset: 0x00008E99
		public override Vector2f Size
		{
			get
			{
				return this.graphic.Size;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000ACA6 File Offset: 0x00008EA6
		public GraphicPrintable(string subsprite)
		{
			this.graphic = new IndexedColorGraphic(Paths.GRAPHICS + "emote.dat", subsprite, VectorMath.ZERO_VECTOR, 0);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000ACCF File Offset: 0x00008ECF
		public override void Update()
		{
			this.isDone = true;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000ACD8 File Offset: 0x00008ED8
		public override void Draw(RenderTarget target)
		{
			this.graphic.Draw(target);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000ACE6 File Offset: 0x00008EE6
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.graphic.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400028D RID: 653
		private IndexedColorGraphic graphic;
	}
}
