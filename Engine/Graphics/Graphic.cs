using System;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Graphics
{
	// Token: 0x02000025 RID: 37
	public class Graphic : AnimatedRenderable
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000633E File Offset: 0x0000453E
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00006346 File Offset: 0x00004546
		public virtual float Rotation { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000634F File Offset: 0x0000454F
		// (set) Token: 0x06000135 RID: 309 RVA: 0x0000635C File Offset: 0x0000455C
		public virtual Color Color
		{
			get
			{
				return this.sprite.Color;
			}
			set
			{
				this.sprite.Color = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000636A File Offset: 0x0000456A
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00006372 File Offset: 0x00004572
		public virtual Vector2f Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000637B File Offset: 0x0000457B
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00006388 File Offset: 0x00004588
		public IntRect TextureRect
		{
			get
			{
				return this.sprite.TextureRect;
			}
			set
			{
				this.sprite.TextureRect = value;
				this.Size = new Vector2f((float)value.Width, (float)value.Height);
				this.startTextureRect = value;
				this.frame = 0f;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600013A RID: 314 RVA: 0x000063C3 File Offset: 0x000045C3
		public ICarbineTexture Texture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000063CC File Offset: 0x000045CC
		public Graphic(string resource, Vector2f position, IntRect textureRect, Vector2f origin, int depth)
		{
			this.texture = TextureManager.Instance.UseUnprocessed(resource);
			this.sprite = new Sprite(this.texture.Image);
			this.sprite.TextureRect = textureRect;
			this.startTextureRect = textureRect;
			this.Position = position;
			this.Origin = origin;
			this.Size = new Vector2f((float)textureRect.Width, (float)textureRect.Height);
			this.Depth = depth;
			this.Rotation = 0f;
			this.scale = new Vector2f(1f, 1f);
			this.finalScale = this.scale;
			this.speedModifier = 1f;
			this.sprite.Position = this.Position;
			this.sprite.Origin = this.Origin;
			this.speeds = new float[]
			{
				1f
			};
			this.speedIndex = 0f;
			this.Visible = true;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000064CC File Offset: 0x000046CC
		protected Graphic()
		{
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000064D4 File Offset: 0x000046D4
		protected void UpdateAnimation()
		{
			int num = this.startTextureRect.Left + (int)this.frame * (int)this.size.X;
			int left = num % (int)this.sprite.Texture.Size.X;
			int top = this.startTextureRect.Top + num / (int)this.sprite.Texture.Size.X * (int)this.size.Y;
			this.sprite.TextureRect = new IntRect(left, top, (int)this.Size.X, (int)this.Size.Y);
			if (this.frame + this.GetFrameSpeed() >= (float)base.Frames)
			{
				base.AnimationComplete();
			}
			this.speedIndex = (this.speedIndex + this.GetFrameSpeed()) % (float)this.speeds.Length;
			this.IncrementFrame();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000065B4 File Offset: 0x000047B4
		protected virtual void IncrementFrame()
		{
			this.frame = (this.frame + this.GetFrameSpeed()) % (float)base.Frames;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000065D1 File Offset: 0x000047D1
		protected float GetFrameSpeed()
		{
			return this.speeds[(int)this.speedIndex % this.speeds.Length] * this.speedModifier;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000065F1 File Offset: 0x000047F1
		public void Translate(Vector2f v)
		{
			this.Translate(v.X, v.Y);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006607 File Offset: 0x00004807
		public virtual void Translate(float x, float y)
		{
			this.position.X = this.position.X + x;
			this.position.Y = this.position.Y + y;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006630 File Offset: 0x00004830
		public override void Draw(RenderTarget target)
		{
			if (this.visible)
			{
				if (base.Frames > 0)
				{
					this.UpdateAnimation();
				}
				this.sprite.Position = this.Position;
				this.sprite.Origin = this.Origin;
				this.sprite.Rotation = this.Rotation;
				this.finalScale = this.scale;
				this.sprite.Scale = this.finalScale;
				target.Draw(this.sprite);
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000066B0 File Offset: 0x000048B0
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing && this.sprite != null)
				{
					this.sprite.Dispose();
				}
				TextureManager.Instance.Unuse(this.texture);
			}
			this.disposed = true;
		}

		// Token: 0x040000B3 RID: 179
		protected Sprite sprite;

		// Token: 0x040000B4 RID: 180
		protected ICarbineTexture texture;

		// Token: 0x040000B5 RID: 181
		protected IntRect startTextureRect;

		// Token: 0x040000B6 RID: 182
		protected Vector2f scale;

		// Token: 0x040000B7 RID: 183
		protected Vector2f finalScale;
	}
}
