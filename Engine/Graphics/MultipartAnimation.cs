using System;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Graphics
{
	// Token: 0x02000029 RID: 41
	public class MultipartAnimation : AnimatedRenderable
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00007147 File Offset: 0x00005347
		// (set) Token: 0x0600016F RID: 367 RVA: 0x0000714F File Offset: 0x0000534F
		public virtual Vector2f Scale { get; set; }

		// Token: 0x06000170 RID: 368 RVA: 0x00007158 File Offset: 0x00005358
		public MultipartAnimation(string resource, Vector2f position, float speed, int depth)
		{
			this.textures = TextureManager.Instance.UseMultipart(resource);
			this.sprite = new Sprite(this.textures[0].Image);
			this.frames = this.textures.Length;
			this.speeds = new float[]
			{
				speed
			};
			this.Position = position;
			this.Origin = this.textures[0].GetSpriteDefinition("default").Origin;
			this.Depth = depth;
			this.Size = new Vector2f(this.textures[0].Image.Size.X, this.textures[0].Image.Size.Y);
			this.Scale = new Vector2f(1f, 1f);
			this.blend = Color.White;
			this.shader = new Shader(EmbeddedResources.GetStream("Carbine.Resources.pal.vert"), EmbeddedResources.GetStream("Carbine.Resources.pal.frag"));
			this.shader.SetParameter("image", this.textures[0].Image);
			this.shader.SetParameter("palette", this.textures[0].Palette);
			this.shader.SetParameter("palIndex", this.textures[0].CurrentPaletteFloat);
			this.shader.SetParameter("palSize", this.textures[0].PaletteSize);
			this.shader.SetParameter("blend", this.blend);
			this.shader.SetParameter("blendMode", 1f);
			this.renderStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, this.shader);
			this.Visible = true;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007320 File Offset: 0x00005520
		public void Reset()
		{
			this.frame = 0f;
			this.intFrame = (int)this.frame;
			this.lastFrame = this.intFrame;
			this.sprite.Texture = this.textures[this.intFrame].Image;
			this.renderStates.Texture = this.sprite.Texture;
			this.shader.SetParameter("image", this.sprite.Texture);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000073A0 File Offset: 0x000055A0
		private void UpdateFrame(float newFrame)
		{
			this.frame = newFrame % (float)this.frames;
			this.lastFrame = this.intFrame;
			this.intFrame = (int)this.frame;
			if (newFrame >= (float)this.frames)
			{
				base.AnimationComplete();
			}
			if (this.visible && this.intFrame != this.lastFrame)
			{
				this.sprite.Texture = this.textures[this.intFrame].Image;
				this.renderStates.Texture = this.sprite.Texture;
				this.shader.SetParameter("image", this.sprite.Texture);
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000744C File Offset: 0x0000564C
		public override void Draw(RenderTarget target)
		{
			if (this.visible)
			{
				float newFrame = this.frame + this.speeds[0];
				this.UpdateFrame(newFrame);
				this.sprite.Position = this.Position;
				this.sprite.Origin = this.Origin;
				this.sprite.Scale = this.Scale;
				target.Draw(this.sprite, this.renderStates);
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000074C0 File Offset: 0x000056C0
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.sprite.Dispose();
				}
				for (int i = 0; i < this.textures.Length; i++)
				{
					TextureManager.Instance.Unuse(this.textures[i]);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x040000CE RID: 206
		private const string SPRITE_NAME = "default";

		// Token: 0x040000CF RID: 207
		private Shader shader;

		// Token: 0x040000D0 RID: 208
		private RenderStates renderStates;

		// Token: 0x040000D1 RID: 209
		private IndexedTexture[] textures;

		// Token: 0x040000D2 RID: 210
		private Sprite sprite;

		// Token: 0x040000D3 RID: 211
		private Color blend;

		// Token: 0x040000D4 RID: 212
		private int lastFrame;

		// Token: 0x040000D5 RID: 213
		private int intFrame;
	}
}
