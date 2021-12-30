using System;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Graphics
{
	// Token: 0x02000027 RID: 39
	public class IndexedColorGraphic : Graphic
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00006810 File Offset: 0x00004A10
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00006818 File Offset: 0x00004A18
		public uint CurrentPalette
		{
			get
			{
				return this.currentPalette;
			}
			set
			{
				if (this.currentPalette != value)
				{
					this.previousPalette = this.currentPalette;
					this.currentPalette = value;
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006836 File Offset: 0x00004A36
		// (set) Token: 0x0600014E RID: 334 RVA: 0x0000683E File Offset: 0x00004A3E
		public override Color Color
		{
			get
			{
				return this.blend;
			}
			set
			{
				this.blend = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00006847 File Offset: 0x00004A47
		// (set) Token: 0x06000150 RID: 336 RVA: 0x0000684F File Offset: 0x00004A4F
		public ColorBlendMode ColorBlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				this.blendMode = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00006858 File Offset: 0x00004A58
		public uint PreviousPalette
		{
			get
			{
				return this.previousPalette;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00006860 File Offset: 0x00004A60
		public RenderStates RenderStates
		{
			get
			{
				return this.renderStates;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00006868 File Offset: 0x00004A68
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00006870 File Offset: 0x00004A70
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

		// Token: 0x06000155 RID: 341 RVA: 0x0000687C File Offset: 0x00004A7C
		public IndexedColorGraphic(string resource, string spriteName, Vector2f position, int depth)
		{
			this.texture = TextureManager.Instance.Use(resource);
			this.sprite = new Sprite(this.texture.Image);
			this.Position = position;
			this.sprite.Position = this.Position;
			this.Depth = depth;
			this.Rotation = 0f;
			this.scale = new Vector2f(1f, 1f);
			this.SetSprite(spriteName);
			((IndexedTexture)this.texture).CurrentPalette = this.currentPalette;
			this.blend = Color.White;
			this.blendMode = ColorBlendMode.Multiply;
			this.renderStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, IndexedColorGraphic.INDEXED_COLOR_SHADER);
			this.animationEnabled = true;
			this.Visible = true;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000694E File Offset: 0x00004B4E
		public void SetSprite(string name)
		{
			this.SetSprite(name, true);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006958 File Offset: 0x00004B58
		public void SetSprite(string name, bool reset)
		{
			SpriteDefinition spriteDefinition = ((IndexedTexture)this.texture).GetSpriteDefinition(name);
			if (spriteDefinition == null)
			{
				spriteDefinition = ((IndexedTexture)this.texture).GetDefaultSpriteDefinition();
			}
			this.sprite.Origin = spriteDefinition.Origin;
			this.Origin = spriteDefinition.Origin;
			this.sprite.TextureRect = new IntRect(spriteDefinition.Coords.X, spriteDefinition.Coords.Y, spriteDefinition.Bounds.X, spriteDefinition.Bounds.Y);
			this.startTextureRect = this.sprite.TextureRect;
			this.Size = new Vector2f((float)this.sprite.TextureRect.Width, (float)this.sprite.TextureRect.Height);
			this.flipX = spriteDefinition.FlipX;
			this.flipY = spriteDefinition.FlipY;
			this.finalScale.X = (this.flipX ? (-this.scale.X) : this.scale.X);
			this.finalScale.Y = (this.flipY ? (-this.scale.Y) : this.scale.Y);
			this.sprite.Scale = this.finalScale;
			base.Frames = spriteDefinition.Frames;
			base.Speeds = spriteDefinition.Speeds;
			this.mode = spriteDefinition.Mode;
			if (reset)
			{
				this.frame = 0f;
				this.betaFrame = 0f;
				this.speedIndex = 0f;
				this.speedModifier = 1f;
				return;
			}
			this.frame %= (float)base.Frames;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006B0C File Offset: 0x00004D0C
		protected override void IncrementFrame()
		{
			float frameSpeed = base.GetFrameSpeed();
			switch (this.mode)
			{
			case 0:
				this.frame = (this.frame + frameSpeed) % (float)base.Frames;
				break;
			case 1:
				this.betaFrame = (this.betaFrame + frameSpeed) % 4f;
				this.frame = (float)IndexedColorGraphic.MODE_ONE_FRAMES[(int)this.betaFrame];
				break;
			}
			this.speedIndex = (float)((int)this.frame % this.speeds.Length);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006B90 File Offset: 0x00004D90
		public override void Draw(RenderTarget target)
		{
			if (!this.disposed)
			{
				if (base.Frames > 1 && this.animationEnabled)
				{
					base.UpdateAnimation();
				}
				this.sprite.Position = this.Position;
				this.sprite.Origin = this.Origin;
				this.sprite.Rotation = this.Rotation;
				this.finalScale.X = (this.flipX ? (-this.scale.X) : this.scale.X);
				this.finalScale.Y = (this.flipY ? (-this.scale.Y) : this.scale.Y);
				this.sprite.Scale = this.finalScale;
				((IndexedTexture)this.texture).CurrentPalette = this.currentPalette;
				IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("image", this.texture.Image);
				IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("palette", ((IndexedTexture)this.texture).Palette);
				IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("palIndex", ((IndexedTexture)this.texture).CurrentPaletteFloat);
				IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("palSize", ((IndexedTexture)this.texture).PaletteSize);
				IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("blend", this.blend);
				IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("blendMode", (float)this.blendMode);
				if (!this.disposed)
				{
					target.Draw(this.sprite, this.renderStates);
				}
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006D2F File Offset: 0x00004F2F
		public FullColorTexture CopyToTexture()
		{
			return ((IndexedTexture)this.texture).ToFullColorTexture();
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006D44 File Offset: 0x00004F44
		public SpriteDefinition GetSpriteDefinition(string sprite)
		{
			int hash = Hash.Get(sprite);
			return ((IndexedTexture)this.texture).GetSpriteDefinition(hash);
		}

		// Token: 0x040000BA RID: 186
		private static readonly int[] MODE_ONE_FRAMES = new int[]
		{
			0,
			1,
			0,
			2
		};

		// Token: 0x040000BB RID: 187
		private static readonly Shader INDEXED_COLOR_SHADER = new Shader(EmbeddedResources.GetStream("Carbine.Resources.pal.vert"), EmbeddedResources.GetStream("Carbine.Resources.pal.frag"));

		// Token: 0x040000BC RID: 188
		private RenderStates renderStates;

		// Token: 0x040000BD RID: 189
		private ColorBlendMode blendMode;

		// Token: 0x040000BE RID: 190
		private bool flipX;

		// Token: 0x040000BF RID: 191
		private bool flipY;

		// Token: 0x040000C0 RID: 192
		private int mode;

		// Token: 0x040000C1 RID: 193
		private float betaFrame;

		// Token: 0x040000C2 RID: 194
		private uint previousPalette;

		// Token: 0x040000C3 RID: 195
		private uint currentPalette;

		// Token: 0x040000C4 RID: 196
		private Color blend;

		// Token: 0x040000C5 RID: 197
		private bool animationEnabled;
	}
}
