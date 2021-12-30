using System;
using Carbine.Graphics;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.Background
{
	// Token: 0x020000BD RID: 189
	internal class BackgroundLayer : IDisposable
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x00019B87 File Offset: 0x00017D87
		public BackgroundLayer(Shader shader, LayerParams newParams)
		{
			this.shader = shader;
			this.parameters = newParams;
			this.SetupTexture();
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00019BA4 File Offset: 0x00017DA4
		~BackgroundLayer()
		{
			this.Dispose(false);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00019BD4 File Offset: 0x00017DD4
		private void SetupTexture()
		{
			this.texture = TextureManager.Instance.Use(this.parameters.File);
			this.texture.Image.Repeated = true;
			this.states = new RenderStates(BlendMode.None, Transform.Identity, this.texture.Image, this.shader);
			uint x = this.texture.Image.Size.X;
			uint y = this.texture.Image.Size.Y;
			this.verts = new VertexArray(PrimitiveType.Quads, 4U);
			this.verts[0U] = new Vertex(new Vector2f(0f, 0f), new Vector2f(0f, 1f));
			this.verts[1U] = new Vertex(new Vector2f(x, 0f), new Vector2f(1f, 1f));
			this.verts[2U] = new Vertex(new Vector2f(x, y), new Vector2f(1f, 0f));
			this.verts[3U] = new Vertex(new Vector2f(0f, y), new Vector2f(0f, 0f));
			this.time = 0f;
			this.xTrans = 0f;
			this.yTrans = 0f;
			this.factors = new float[this.parameters.Variation.Length];
			this.palChangeIndex = 0;
			this.palFrame = 0;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00019D68 File Offset: 0x00017F68
		public void UpdateParameters(LayerParams newParams)
		{
			LayerParams layerParams = this.parameters;
			this.parameters = newParams;
			if (layerParams.File != this.parameters.File)
			{
				TextureManager.Instance.Unuse(this.texture);
				this.SetupTexture();
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00019DB1 File Offset: 0x00017FB1
		public void ResetTranslation()
		{
			this.xTrans = 0f;
			this.yTrans = 0f;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00019DC9 File Offset: 0x00017FC9
		public void AddTranslation(float x, float y, float xFactor, float yFactor)
		{
			this.xTrans += x * xFactor;
			this.yTrans += y * yFactor;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00019DEC File Offset: 0x00017FEC
		public void Draw(RenderTexture oldFrame, RenderTexture newFrame)
		{
			this.shader.SetParameter("layerTex", this.texture.Image);
			this.shader.SetParameter("bottomTex", oldFrame.Texture);
			this.shader.SetParameter("height", 180f);
			int num = this.factors.Length;
			for (int i = 0; i < num; i++)
			{
				float a = this.parameters.Variation[i].A;
				float b = this.parameters.Variation[i].B;
				float c = this.parameters.Variation[i].C;
				float d = this.parameters.Variation[i].D;
				float e = this.parameters.Variation[i].E;
				switch (this.parameters.Variation[i].Mode)
				{
				case 0:
					this.factors[i] = a;
					break;
				case 1:
					this.factors[i] = (float)((double)a * Math.Sin((double)(b * this.time + c)) + (double)d);
					break;
				case 2:
					this.factors[i] = (float)((double)a * Math.Sin((double)(b * this.time + c)) * Math.Sin((double)(d * this.time)) + (double)e);
					break;
				}
			}
			this.shader.SetParameter("time", this.time);
			this.shader.SetParameter("amp", this.parameters.Amplitude * this.factors[3]);
			this.shader.SetParameter("freq", this.parameters.Frequency * this.factors[0]);
			this.shader.SetParameter("scale", this.parameters.Scale * this.factors[1]);
			this.shader.SetParameter("comp", this.parameters.Compression * this.factors[2]);
			this.shader.SetParameter("blend", (float)this.parameters.Blend);
			this.shader.SetParameter("mode", (float)this.parameters.Mode);
			this.shader.SetParameter("opacity", this.parameters.Opacity * this.factors[5]);
			float num2 = this.parameters.Speed * this.factors[4];
			this.xTrans += this.parameters.Xtrans * this.factors[6] * num2;
			this.yTrans += this.parameters.Ytrans * this.factors[7] * num2;
			this.xTrans %= 320f;
			this.yTrans %= 180f;
			this.shader.SetParameter("xTrans", this.xTrans);
			this.shader.SetParameter("yTrans", this.yTrans);
			this.shader.SetParameter("palette", this.texture.Palette);
			if (this.parameters.Palette.Length > 0 && this.palChangeIndex < this.parameters.Palette.Length)
			{
				this.texture.CurrentPalette = (uint)this.parameters.Palette[this.palChangeIndex].Index;
				this.shader.SetParameter("palIndex", this.texture.CurrentPaletteFloat);
				this.shader.SetParameter("palSize", this.texture.PaletteSize);
				this.shader.SetParameter("palShift", (float)this.parameters.Palette[this.palChangeIndex].Shift / this.texture.PaletteSize);
				if (this.palFrame < this.parameters.Palette[this.palChangeIndex].Duration)
				{
					this.palFrame++;
				}
				else
				{
					this.palChangeIndex = (this.palChangeIndex + 1) % this.parameters.Palette.Length;
					this.palFrame = 0;
				}
			}
			else
			{
				this.palChangeIndex = 0;
				this.palFrame = 0;
				this.shader.SetParameter("palIndex", 0f);
				this.shader.SetParameter("palSize", this.texture.PaletteSize);
				this.shader.SetParameter("palShift", 0f);
			}
			newFrame.Draw(this.verts, this.states);
			this.time += num2;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001A2B8 File Offset: 0x000184B8
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001A2C7 File Offset: 0x000184C7
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				TextureManager.Instance.Unuse(this.texture);
			}
			this.disposed = true;
		}

		// Token: 0x040005D6 RID: 1494
		protected bool disposed;

		// Token: 0x040005D7 RID: 1495
		private LayerParams parameters;

		// Token: 0x040005D8 RID: 1496
		private float time;

		// Token: 0x040005D9 RID: 1497
		private float xTrans;

		// Token: 0x040005DA RID: 1498
		private float yTrans;

		// Token: 0x040005DB RID: 1499
		private float[] factors;

		// Token: 0x040005DC RID: 1500
		private int palChangeIndex;

		// Token: 0x040005DD RID: 1501
		private int palFrame;

		// Token: 0x040005DE RID: 1502
		private Shader shader;

		// Token: 0x040005DF RID: 1503
		private RenderStates states;

		// Token: 0x040005E0 RID: 1504
		private IndexedTexture texture;

		// Token: 0x040005E1 RID: 1505
		private VertexArray verts;
	}
}
