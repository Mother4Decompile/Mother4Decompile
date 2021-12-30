using System;
using Carbine.Graphics;
using Carbine.GUI;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	// Token: 0x0200003F RID: 63
	internal class Nametag : Renderable
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00008C5A File Offset: 0x00006E5A
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00008C62 File Offset: 0x00006E62
		public override Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.Reposition(value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00008C6B File Offset: 0x00006E6B
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00008C78 File Offset: 0x00006E78
		public string Name
		{
			get
			{
				return this.nameText.Text;
			}
			set
			{
				this.SetName(value);
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00008C84 File Offset: 0x00006E84
		public Nametag(string nameString, Vector2f position, int depth)
		{
			this.position = position;
			this.depth = depth;
			this.nameText = new TextRegion(this.position + Nametag.TEXT_POSITION, this.depth + 1, Fonts.Main, nameString);
			this.left = new IndexedColorGraphic(Nametag.RESOURCE_NAME, "left", this.position, this.depth);
			this.center = new IndexedColorGraphic(Nametag.RESOURCE_NAME, "center", this.left.Position + new Vector2f(this.left.Size.X, 0f), this.depth);
			this.center.Scale = new Vector2f(this.nameText.Size.X + 2f, 1f);
			this.right = new IndexedColorGraphic(Nametag.RESOURCE_NAME, "right", this.center.Position + new Vector2f(this.nameText.Size.X + 2f, 0f), this.depth);
			this.CalculateSize();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00008DB0 File Offset: 0x00006FB0
		private void Reposition(Vector2f newPosition)
		{
			this.position = newPosition;
			this.nameText.Position = this.position + Nametag.TEXT_POSITION;
			this.left.Position = this.position;
			this.center.Position = this.left.Position + new Vector2f(this.left.Size.X, 0f);
			this.right.Position = this.center.Position + new Vector2f(this.nameText.Size.X + 2f, 0f);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00008E60 File Offset: 0x00007060
		private void SetName(string newName)
		{
			this.nameText.Reset(newName, 0, newName.Length);
			this.center.Scale = new Vector2f(this.nameText.Size.X + 2f, 1f);
			this.right.Position = this.center.Position + new Vector2f(this.nameText.Size.X + 2f, 0f);
			this.CalculateSize();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00008EEC File Offset: 0x000070EC
		private void CalculateSize()
		{
			this.size = new Vector2f(this.left.Size.X + this.nameText.Size.X + 2f + this.right.Size.X, this.left.Size.Y);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00008F4C File Offset: 0x0000714C
		public override void Draw(RenderTarget target)
		{
			this.left.Draw(target);
			this.center.Draw(target);
			this.right.Draw(target);
			this.nameText.Draw(target);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008F7E File Offset: 0x0000717E
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.left.Dispose();
				this.center.Dispose();
				this.right.Dispose();
				this.nameText.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400023D RID: 573
		private const string LEFT_SPRITE_NAME = "left";

		// Token: 0x0400023E RID: 574
		private const string CENTER_SPRITE_NAME = "center";

		// Token: 0x0400023F RID: 575
		private const string RIGHT_SPRITE_NAME = "right";

		// Token: 0x04000240 RID: 576
		private const int MARGIN = 2;

		// Token: 0x04000241 RID: 577
		private static readonly string RESOURCE_NAME = Paths.GRAPHICS + "nametag.dat";

		// Token: 0x04000242 RID: 578
		private static readonly Vector2f TEXT_POSITION = new Vector2f(5f, 1f);

		// Token: 0x04000243 RID: 579
		private IndexedColorGraphic left;

		// Token: 0x04000244 RID: 580
		private IndexedColorGraphic center;

		// Token: 0x04000245 RID: 581
		private IndexedColorGraphic right;

		// Token: 0x04000246 RID: 582
		private TextRegion nameText;
	}
}
