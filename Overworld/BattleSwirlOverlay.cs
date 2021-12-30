using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Mother4.Data;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	// Token: 0x020000FB RID: 251
	internal class BattleSwirlOverlay : Renderable
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060005C5 RID: 1477 RVA: 0x000224B8 File Offset: 0x000206B8
		// (remove) Token: 0x060005C6 RID: 1478 RVA: 0x000224F0 File Offset: 0x000206F0
		public event BattleSwirlOverlay.AnimationCompleteHandler OnAnimationComplete;

		// Token: 0x060005C7 RID: 1479 RVA: 0x00022528 File Offset: 0x00020728
		public BattleSwirlOverlay(BattleSwirlOverlay.Style style, int depth, float speed)
		{
			this.depth = depth;
			string file = BattleSwirlOverlay.RESOURCES[style];
			this.textures = TextureManager.Instance.UseMultipart(file);
			this.gradMap = TextureManager.Instance.UseUnprocessed(Paths.BATTLE_SWIRL + "gradmap.png");
			Random random = new Random(5551247);
			int num = (Engine.Random.Next(100) >= 50) ? -1 : 1;
			int num2 = (Engine.Random.Next(100) >= 50) ? -1 : 1;
			int num3 = num * 160;
			int num4 = num2 * 90;
			this.layers = new VertexArray[this.textures.Length - 1];
			this.delta = new float[this.layers.Length];
			this.speed = new float[this.layers.Length];
			for (int i = 0; i < this.layers.Length; i++)
			{
				this.speed[i] = speed + (float)((random.NextDouble() - 0.5) * 0.002);
				this.delta[i] = 0f;
				this.layers[i] = new VertexArray(PrimitiveType.Quads, 4U);
				this.layers[i][0U] = new Vertex(new Vector2f((float)(-(float)num3), (float)(-(float)num4)), new Vector2f(0f, 0f));
				this.layers[i][1U] = new Vertex(new Vector2f((float)num3, (float)(-(float)num4)), new Vector2f(1f, 0f));
				this.layers[i][2U] = new Vertex(new Vector2f((float)num3, (float)num4), new Vector2f(1f, 1f));
				this.layers[i][3U] = new Vertex(new Vector2f((float)(-(float)num3), (float)num4), new Vector2f(0f, 1f));
				this.textures[1 + i].CurrentPalette = BattleSwirlOverlay.PALETTES[style];
			}
			this.scale = new Vector2f(1f, 1f);
			this.position = this.position;
			this.origin = this.textures[0].GetSpriteDefinition("default").Origin;
			this.depth = depth;
			this.size = Engine.SCREEN_SIZE;
			this.blend = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 160);
			this.shader = new Shader(EmbeddedResources.GetStream("Mother4.Resources.bbg.vert"), EmbeddedResources.GetStream("Mother4.Resources.gradmap.frag"));
			this.shader.SetParameter("gradmap", this.gradMap.Image);
			this.shader.SetParameter("image", this.textures[1].Image);
			this.shader.SetParameter("palette", this.textures[1].Palette);
			this.shader.SetParameter("palIndex", this.textures[1].CurrentPaletteFloat);
			this.shader.SetParameter("palSize", this.textures[1].PaletteSize);
			this.shader.SetParameter("blend", this.blend);
			this.shader.SetParameter("blendMode", 1f);
			this.shader.SetParameter("delta", 0f);
			this.renderStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, this.shader);
			this.UpdatePosition(ViewManager.Instance.FinalCenter);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000228C0 File Offset: 0x00020AC0
		public void Reset()
		{
			for (int i = 0; i < this.delta.Length; i++)
			{
				this.delta[i] = 0f;
			}
			this.isComplete = false;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000228F4 File Offset: 0x00020AF4
		private void UpdatePosition(Vector2f position)
		{
			this.position = position;
			this.transform = Transform.Identity;
			this.transform.Translate(this.position);
			this.renderStates.Transform = this.transform;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0002292C File Offset: 0x00020B2C
		public override void Draw(RenderTarget target)
		{
			if (this.visible)
			{
				this.UpdatePosition(ViewManager.Instance.FinalCenter);
				bool flag = true;
				for (int i = 0; i < this.layers.Length; i++)
				{
					if (!this.isComplete)
					{
						this.delta[i] = Math.Min(1f, this.delta[i] + this.speed[i]);
						this.shader.SetParameter("delta", this.delta[i]);
					}
					flag &= (this.delta[i] >= 1f);
					this.shader.SetParameter("image", this.textures[1 + i].Image);
					this.shader.SetParameter("palette", this.textures[1 + i].Palette);
					this.shader.SetParameter("palIndex", this.textures[1 + i].CurrentPaletteFloat);
					this.shader.SetParameter("palSize", this.textures[1 + i].PaletteSize);
					target.Draw(this.layers[i], this.renderStates);
				}
				if (!this.isComplete && flag)
				{
					this.isComplete = true;
					if (this.OnAnimationComplete != null)
					{
						this.OnAnimationComplete(this);
					}
				}
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00022A80 File Offset: 0x00020C80
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					for (int i = 0; i < this.layers.Length; i++)
					{
						this.layers[i].Dispose();
					}
				}
				TextureManager.Instance.Unuse(this.textures);
				TextureManager.Instance.Unuse(this.gradMap);
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000786 RID: 1926
		private const int RANDOM_SEED = 5551247;

		// Token: 0x04000787 RID: 1927
		private const string SPRITE_NAME = "default";

		// Token: 0x04000788 RID: 1928
		private static readonly Dictionary<BattleSwirlOverlay.Style, string> RESOURCES = new Dictionary<BattleSwirlOverlay.Style, string>
		{
			{
				BattleSwirlOverlay.Style.Blue,
				Paths.BATTLE_SWIRL + "green.sdat"
			},
			{
				BattleSwirlOverlay.Style.Green,
				Paths.BATTLE_SWIRL + "green.sdat"
			},
			{
				BattleSwirlOverlay.Style.Red,
				Paths.BATTLE_SWIRL + "green.sdat"
			},
			{
				BattleSwirlOverlay.Style.Boss,
				Paths.BATTLE_SWIRL + "green.sdat"
			}
		};

		// Token: 0x04000789 RID: 1929
		private static readonly Dictionary<BattleSwirlOverlay.Style, uint> PALETTES = new Dictionary<BattleSwirlOverlay.Style, uint>
		{
			{
				BattleSwirlOverlay.Style.Blue,
				1U
			},
			{
				BattleSwirlOverlay.Style.Green,
				0U
			},
			{
				BattleSwirlOverlay.Style.Red,
				2U
			},
			{
				BattleSwirlOverlay.Style.Boss,
				0U
			}
		};

		// Token: 0x0400078A RID: 1930
		private VertexArray[] layers;

		// Token: 0x0400078B RID: 1931
		private IndexedTexture[] textures;

		// Token: 0x0400078C RID: 1932
		private FullColorTexture gradMap;

		// Token: 0x0400078D RID: 1933
		private Shader shader;

		// Token: 0x0400078E RID: 1934
		private RenderStates renderStates;

		// Token: 0x0400078F RID: 1935
		private Transform transform;

		// Token: 0x04000790 RID: 1936
		private Vector2f scale;

		// Token: 0x04000791 RID: 1937
		private Color blend;

		// Token: 0x04000792 RID: 1938
		private float[] speed;

		// Token: 0x04000793 RID: 1939
		private float[] delta;

		// Token: 0x04000794 RID: 1940
		private bool isComplete;

		// Token: 0x020000FC RID: 252
		public enum Style
		{
			// Token: 0x04000797 RID: 1943
			Blue,
			// Token: 0x04000798 RID: 1944
			Green,
			// Token: 0x04000799 RID: 1945
			Red,
			// Token: 0x0400079A RID: 1946
			Boss
		}

		// Token: 0x020000FD RID: 253
		// (Invoke) Token: 0x060005CE RID: 1486
		public delegate void AnimationCompleteHandler(BattleSwirlOverlay anim);
	}
}
