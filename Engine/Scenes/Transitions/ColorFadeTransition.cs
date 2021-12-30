using System;
using Carbine.Graphics;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Scenes.Transitions
{
	// Token: 0x02000058 RID: 88
	public class ColorFadeTransition : ITransition
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000DA1F File Offset: 0x0000BC1F
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000DA27 File Offset: 0x0000BC27
		public float Progress
		{
			get
			{
				return this.progress;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000DA2F File Offset: 0x0000BC2F
		public bool ShowNewScene
		{
			get
			{
				return this.progress > 0.5f;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000DA3E File Offset: 0x0000BC3E
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000DA46 File Offset: 0x0000BC46
		public bool Blocking { get; set; }

		// Token: 0x06000284 RID: 644 RVA: 0x0000DA50 File Offset: 0x0000BC50
		public ColorFadeTransition(float duration, Color color)
		{
			float num = 60f * duration;
			this.speed = 1f / num;
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

		// Token: 0x06000285 RID: 645 RVA: 0x0000DB80 File Offset: 0x0000BD80
		public void Update()
		{
			this.progress += this.speed;
			this.isComplete = (this.progress > 1f);
			byte b = (byte)(255.0 * (Math.Cos((double)(this.progress * 2f) * 3.141592653589793 + 3.141592653589793) / 2.0 + 0.5));
			b /= 25;
			b *= 25;
			this.verts[0].Color.A = b;
			this.verts[1].Color.A = b;
			this.verts[2].Color.A = b;
			this.verts[3].Color.A = b;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000DC64 File Offset: 0x0000BE64
		public void Draw()
		{
			this.renderStates.Transform = new Transform(1f, 0f, ViewManager.Instance.FinalCenter.X, 0f, 1f, ViewManager.Instance.FinalCenter.Y, 0f, 0f, 1f);
			this.target.Draw(this.verts, PrimitiveType.Quads, this.renderStates);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000DCDC File Offset: 0x0000BEDC
		public void Reset()
		{
			this.isComplete = false;
			this.progress = 0f;
			this.verts[0].Color.A = 0;
			this.verts[1].Color.A = 0;
			this.verts[2].Color.A = 0;
			this.verts[3].Color.A = 0;
		}

		// Token: 0x040001E8 RID: 488
		private const int STEPS = 10;

		// Token: 0x040001E9 RID: 489
		private float speed;

		// Token: 0x040001EA RID: 490
		private bool isComplete;

		// Token: 0x040001EB RID: 491
		private float progress;

		// Token: 0x040001EC RID: 492
		private RenderTarget target;

		// Token: 0x040001ED RID: 493
		private Vertex[] verts;

		// Token: 0x040001EE RID: 494
		private RenderStates renderStates;
	}
}
