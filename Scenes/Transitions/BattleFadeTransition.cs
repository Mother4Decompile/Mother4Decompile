using System;
using Carbine;
using Carbine.Graphics;
using Carbine.Scenes.Transitions;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes.Transitions
{
	// Token: 0x02000115 RID: 277
	internal class BattleFadeTransition : ITransition
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0002A520 File Offset: 0x00028720
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0002A528 File Offset: 0x00028728
		public float Progress
		{
			get
			{
				return this.progress;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0002A530 File Offset: 0x00028730
		public bool ShowNewScene
		{
			get
			{
				return this.progress > 1f - this.speed;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0002A546 File Offset: 0x00028746
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x0002A54E File Offset: 0x0002874E
		public bool Blocking { get; set; }

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002A558 File Offset: 0x00028758
		public BattleFadeTransition(float duration, Color color)
		{
			float num = 60f * duration;
			this.speed = 1f / num;
			this.color = color;
			this.isComplete = false;
			this.progress = 0f;
			this.target = Engine.FrameBuffer;
			float num2 = 160f;
			float num3 = 90f;
			this.verts = new Vertex[4];
			this.verts[0] = new Vertex(new Vector2f(-num2, -num3), color);
			this.verts[1] = new Vertex(new Vector2f(num2, -num3), color);
			this.verts[2] = new Vertex(new Vector2f(num2, num3), color);
			this.verts[3] = new Vertex(new Vector2f(-num2, num3), color);
			Transform transform = new Transform(1f, 0f, ViewManager.Instance.FinalCenter.X, 0f, 1f, ViewManager.Instance.FinalCenter.Y, 0f, 0f, 1f);
			this.renderStates = new RenderStates(transform);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0002A68C File Offset: 0x0002888C
		public void Update()
		{
			this.progress += this.speed;
			this.isComplete = (this.progress > 1f);
			byte b;
			if (this.progress < 0.333f)
			{
				b = (byte)(255.0 * (Math.Cos((double)(this.progress * 3f) * 3.141592653589793 + 3.141592653589793) / 4.0 + 0.25));
			}
			else if (this.progress < 0.666f)
			{
				b = 128;
			}
			else if (this.progress < 1f)
			{
				b = (byte)(255.0 * (Math.Cos((double)((this.progress - 0.666f) * 3f) * 3.141592653589793 + 3.141592653589793) / 4.0 + 0.75));
			}
			else
			{
				b = (byte)(255f * (1f - (this.progress - 1f)));
			}
			b /= 12;
			b *= 12;
			byte b2;
			byte b3;
			byte b4;
			if (this.progress < 0.6f)
			{
				b2 = this.color.R;
				b3 = this.color.G;
				b4 = this.color.B;
			}
			else if (this.progress < 1f)
			{
				float num = 0.7f - (this.progress - 0.6f) / 0.4f;
				num = Math.Max(0f, num);
				b2 = (byte)((float)this.color.R * num);
				b3 = (byte)((float)this.color.G * num);
				b4 = (byte)((float)this.color.B * num);
				b2 /= 12;
				b2 *= 12;
				b3 /= 12;
				b3 *= 12;
				b4 /= 12;
				b4 *= 12;
			}
			else
			{
				float num2 = 1f - (this.progress - 1f);
				num2 = Math.Max(0f, num2);
				b2 = (byte)((float)this.color.R * num2);
				b3 = (byte)((float)this.color.G * num2);
				b4 = (byte)((float)this.color.B * num2);
				b2 /= 12;
				b2 *= 12;
				b3 /= 12;
				b3 *= 12;
				b4 /= 12;
				b4 *= 12;
			}
			this.SetVertColor(0, b2, b3, b4, b);
			this.SetVertColor(1, b2, b3, b4, b);
			this.SetVertColor(2, b2, b3, b4, b);
			this.SetVertColor(3, b2, b3, b4, b);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0002A91C File Offset: 0x00028B1C
		private void SetVertColor(int index, byte R, byte G, byte B, byte A)
		{
			this.verts[index].Color.R = R;
			this.verts[index].Color.G = G;
			this.verts[index].Color.B = B;
			this.verts[index].Color.A = A;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002A988 File Offset: 0x00028B88
		public void Draw()
		{
			this.renderStates.Transform = new Transform(1f, 0f, ViewManager.Instance.FinalCenter.X, 0f, 1f, ViewManager.Instance.FinalCenter.Y, 0f, 0f, 1f);
			this.target.Draw(this.verts, PrimitiveType.Quads, this.renderStates);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002A9FE File Offset: 0x00028BFE
		public void Reset()
		{
			this.isComplete = false;
			this.progress = 0f;
		}

		// Token: 0x040008A4 RID: 2212
		private const int STEPS = 20;

		// Token: 0x040008A5 RID: 2213
		private float speed;

		// Token: 0x040008A6 RID: 2214
		private bool isComplete;

		// Token: 0x040008A7 RID: 2215
		private float progress;

		// Token: 0x040008A8 RID: 2216
		private RenderTarget target;

		// Token: 0x040008A9 RID: 2217
		private Vertex[] verts;

		// Token: 0x040008AA RID: 2218
		private RenderStates renderStates;

		// Token: 0x040008AB RID: 2219
		private Color color;
	}
}
