using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Tiles
{
	// Token: 0x0200005B RID: 91
	public class TileGroup : Renderable, IDisposable
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000DDB2 File Offset: 0x0000BFB2
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000DDBA File Offset: 0x0000BFBA
		public bool AnimationEnabled
		{
			get
			{
				return this.animationEnabled;
			}
			set
			{
				this.animationEnabled = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000DDC3 File Offset: 0x0000BFC3
		// (set) Token: 0x06000295 RID: 661 RVA: 0x0000DDCB File Offset: 0x0000BFCB
		public override Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
				this.ResetTransform();
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000DDDA File Offset: 0x0000BFDA
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000DDE2 File Offset: 0x0000BFE2
		public override Vector2f Origin
		{
			get
			{
				return this.origin;
			}
			set
			{
				this.origin = value;
				this.ResetTransform();
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000DDF1 File Offset: 0x0000BFF1
		public IndexedTexture Tileset
		{
			get
			{
				return this.tileset;
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000DDFC File Offset: 0x0000BFFC
		public TileGroup(IList<Tile> tiles, string resource, int depth, Vector2f position, uint palette)
		{
			this.tileset = TextureManager.Instance.Use(resource);
			this.tileset.CurrentPalette = palette;
			this.position = position;
			this.depth = depth;
			this.renderState = new RenderStates(BlendMode.Alpha, Transform.Identity, this.tileset.Image, TileGroup.TILE_GROUP_SHADER);
			this.animationEnabled = true;
			this.CreateAnimations(this.tileset.GetSpriteDefinitions());
			this.CreateVertexArray(tiles);
			this.ResetTransform();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000DE88 File Offset: 0x0000C088
		private void ResetTransform()
		{
			Transform identity = Transform.Identity;
			identity.Translate(this.position - this.origin);
			this.renderState.Transform = identity;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000DEC0 File Offset: 0x0000C0C0
		public int GetTileId(Vector2f location)
		{
			Vector2f vector2f = location - this.position + this.origin;
			uint num = (uint)(vector2f.X / 8f + vector2f.Y / 8f * (this.size.X / 8f));
			Vertex vertex = this.vertices[(int)((UIntPtr)(num * 4U))];
			Vector2f texCoords = vertex.TexCoords;
			return (int)(texCoords.X / 8f + texCoords.Y / 8f * (this.tileset.Image.Size.X / 8U));
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000DF6C File Offset: 0x0000C16C
		private void IDToTexCoords(uint id, out uint tx, out uint ty)
		{
			tx = id * 8U % this.tileset.Image.Size.X;
			ty = id * 8U / this.tileset.Image.Size.X * 8U;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
		private void CreateAnimations(ICollection<SpriteDefinition> definitions)
		{
			this.animations = new TileGroup.TileAnimation[definitions.Count];
			foreach (SpriteDefinition spriteDefinition in definitions)
			{
				int num = -1;
				int.TryParse(spriteDefinition.Name, out num);
				if (num >= 0)
				{
					if (spriteDefinition.Data != null && spriteDefinition.Data.Length > 0)
					{
						int[] data = spriteDefinition.Data;
						float speed = spriteDefinition.Speeds[0];
						this.animations[num].Tiles = data;
						this.animations[num].VertIndexes = new List<int>();
						this.animations[num].Speed = speed;
					}
					else
					{
						Console.WriteLine("Tried to load tile animation data for animation {0}, but there was no tile data.", num);
					}
				}
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000E088 File Offset: 0x0000C288
		private void AddVertIndex(Tile tile, int index)
		{
			if (tile.AnimationId > 0)
			{
				int num = (int)(tile.AnimationId - 1);
				this.animations[num].VertIndexes.Add(index);
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000E0C0 File Offset: 0x0000C2C0
		private unsafe void CreateVertexArray(IList<Tile> tiles)
		{
			this.vertices = new Vertex[tiles.Count * 4];
			uint num = 0U;
			uint num2 = 0U;
			Vector2f v = default(Vector2f);
			Vector2f v2 = default(Vector2f);
			fixed (Vertex* ptr = this.vertices)
			{
				for (int i = 0; i < tiles.Count; i++)
				{
					Vertex* ptr2 = ptr + i * 4;
					Tile tile = tiles[i];
					float x = tile.Position.X;
					float y = tile.Position.Y;
					ptr2->Position.X = x;
					ptr2->Position.Y = y;
					ptr2[1].Position.X = x + 8f;
					ptr2[1].Position.Y = y;
					ptr2[2].Position.X = x + 8f;
					ptr2[2].Position.Y = y + 8f;
					ptr2[3].Position.X = x;
					ptr2[3].Position.Y = y + 8f;
					this.IDToTexCoords(tile.ID, out num, out num2);
					if (!tile.FlipHorizontal && !tile.FlipVertical)
					{
						ptr2->TexCoords.X = num;
						ptr2->TexCoords.Y = num2;
						ptr2[1].TexCoords.X = num + 8U;
						ptr2[1].TexCoords.Y = num2;
						ptr2[2].TexCoords.X = num + 8U;
						ptr2[2].TexCoords.Y = num2 + 8U;
						ptr2[3].TexCoords.X = num;
						ptr2[3].TexCoords.Y = num2 + 8U;
					}
					else if (tile.FlipHorizontal && !tile.FlipVertical)
					{
						ptr2->TexCoords.X = num + 8U;
						ptr2->TexCoords.Y = num2;
						ptr2[1].TexCoords.X = num;
						ptr2[1].TexCoords.Y = num2;
						ptr2[2].TexCoords.X = num;
						ptr2[2].TexCoords.Y = num2 + 8U;
						ptr2[3].TexCoords.X = num + 8U;
						ptr2[3].TexCoords.Y = num2 + 8U;
					}
					else if (!tile.FlipHorizontal && tile.FlipVertical)
					{
						ptr2->TexCoords.X = num;
						ptr2->TexCoords.Y = num2 + 8U;
						ptr2[1].TexCoords.X = num + 8U;
						ptr2[1].TexCoords.Y = num2 + 8U;
						ptr2[2].TexCoords.X = num + 8U;
						ptr2[2].TexCoords.Y = num2;
						ptr2[3].TexCoords.X = num;
						ptr2[3].TexCoords.Y = num2;
					}
					else
					{
						ptr2->TexCoords.X = num + 8U;
						ptr2->TexCoords.Y = num2 + 8U;
						ptr2[1].TexCoords.X = num;
						ptr2[1].TexCoords.Y = num2 + 8U;
						ptr2[2].TexCoords.X = num;
						ptr2[2].TexCoords.Y = num2;
						ptr2[3].TexCoords.X = num + 8U;
						ptr2[3].TexCoords.Y = num2;
					}
					v.X = Math.Min(v.X, ptr2->Position.X);
					v.Y = Math.Min(v.Y, ptr2->Position.Y);
					v2.X = Math.Max(v2.X, ptr2[2].Position.X - v.X);
					v2.Y = Math.Max(v2.Y, ptr2[2].Position.Y - v.Y);
					this.AddVertIndex(tile, i * 4);
				}
			}
			this.size = v2 - v;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000E628 File Offset: 0x0000C828
		private unsafe void UpdateAnimations()
		{
			if (!this.animationEnabled)
			{
				return;
			}
			for (int i = 0; i < this.animations.Length; i++)
			{
				TileGroup.TileAnimation tileAnimation = this.animations[i];
				float num = (float)Engine.Frame * tileAnimation.Speed;
				uint num2 = (uint)tileAnimation.Tiles[(int)num % tileAnimation.Tiles.Length];
				uint num3;
				uint num4;
				this.IDToTexCoords(num2 - 1U, out num3, out num4);
				fixed (Vertex* ptr = this.vertices)
				{
					for (int j = 0; j < tileAnimation.VertIndexes.Count; j++)
					{
						int num5 = tileAnimation.VertIndexes[j];
						Vertex* ptr2 = ptr + num5;
						ptr2->TexCoords.X = num3;
						ptr2->TexCoords.Y = num4;
						ptr2[1].TexCoords.X = num3 + 8U;
						ptr2[1].TexCoords.Y = num4;
						ptr2[2].TexCoords.X = num3 + 8U;
						ptr2[2].TexCoords.Y = num4 + 8U;
						ptr2[3].TexCoords.X = num3;
						ptr2[3].TexCoords.Y = num4 + 8U;
					}
				}
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000E7C0 File Offset: 0x0000C9C0
		public override void Draw(RenderTarget target)
		{
			TileGroup.TILE_GROUP_SHADER.SetParameter("image", this.tileset.Image);
			TileGroup.TILE_GROUP_SHADER.SetParameter("palette", this.tileset.Palette);
			TileGroup.TILE_GROUP_SHADER.SetParameter("palIndex", this.tileset.CurrentPaletteFloat);
			TileGroup.TILE_GROUP_SHADER.SetParameter("palSize", this.tileset.PaletteSize);
			TileGroup.TILE_GROUP_SHADER.SetParameter("blend", Color.White);
			TileGroup.TILE_GROUP_SHADER.SetParameter("blendMode", 1f);
			this.UpdateAnimations();
			target.Draw(this.vertices, PrimitiveType.Quads, this.renderState);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000E878 File Offset: 0x0000CA78
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				TextureManager.Instance.Unuse(this.tileset);
			}
			this.disposed = true;
		}

		// Token: 0x040001F8 RID: 504
		private static readonly Shader TILE_GROUP_SHADER = new Shader(EmbeddedResources.GetStream("Carbine.Resources.pal.vert"), EmbeddedResources.GetStream("Carbine.Resources.pal.frag"));

		// Token: 0x040001F9 RID: 505
		private Vertex[] vertices;

		// Token: 0x040001FA RID: 506
		private IndexedTexture tileset;

		// Token: 0x040001FB RID: 507
		private RenderStates renderState;

		// Token: 0x040001FC RID: 508
		private TileGroup.TileAnimation[] animations;

		// Token: 0x040001FD RID: 509
		private bool animationEnabled;

		// Token: 0x0200005C RID: 92
		private struct TileAnimation
		{
			// Token: 0x040001FE RID: 510
			public int[] Tiles;

			// Token: 0x040001FF RID: 511
			public IList<int> VertIndexes;

			// Token: 0x04000200 RID: 512
			public float Speed;
		}
	}
}
