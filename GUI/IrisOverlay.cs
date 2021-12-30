using System;
using Carbine.Graphics;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	// Token: 0x0200008D RID: 141
	internal class IrisOverlay : Renderable
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002DA RID: 730 RVA: 0x000126E0 File Offset: 0x000108E0
		// (set) Token: 0x060002DB RID: 731 RVA: 0x000126E8 File Offset: 0x000108E8
		public float Progress
		{
			get
			{
				return this.progress;
			}
			set
			{
				this.SetProgress(value);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002DC RID: 732 RVA: 0x000126F1 File Offset: 0x000108F1
		// (set) Token: 0x060002DD RID: 733 RVA: 0x000126F9 File Offset: 0x000108F9
		public float Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060002DE RID: 734 RVA: 0x00012704 File Offset: 0x00010904
		// (remove) Token: 0x060002DF RID: 735 RVA: 0x0001273C File Offset: 0x0001093C
		public event IrisOverlay.AnimationCompleteHandler OnAnimationComplete;

		// Token: 0x060002E0 RID: 736 RVA: 0x00012774 File Offset: 0x00010974
		public IrisOverlay(Vector2f position, Vector2f origin, float progress)
		{
			this.position = position;
			this.origin = origin;
			this.progress = progress;
			this.animationDone = true;
			this.size = new Vector2f(320f, 180f);
			this.depth = 2147450880;
			int num = 160;
			int num2 = 90;
			this.verts = new VertexArray(PrimitiveType.Quads, 4U);
			this.verts[0U] = new Vertex(new Vector2f((float)(-(float)num), (float)(-(float)num2)), new Vector2f(0f, 0f));
			this.verts[1U] = new Vertex(new Vector2f((float)num, (float)(-(float)num2)), new Vector2f(1f, 0f));
			this.verts[2U] = new Vertex(new Vector2f((float)num, (float)num2), new Vector2f(1f, 1f));
			this.verts[3U] = new Vertex(new Vector2f((float)(-(float)num), (float)num2), new Vector2f(0f, 1f));
			this.shader = new Shader(EmbeddedResources.GetStream("Mother4.Resources.bbg.vert"), EmbeddedResources.GetStream("Mother4.Resources.iris.frag"));
			this.shader.SetParameter("progress", this.progress);
			this.shader.SetParameter("size", this.size);
			this.transform = Transform.Identity;
			this.transform.Translate(this.position);
			this.states = new RenderStates(BlendMode.Alpha, this.transform, null, this.shader);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00012905 File Offset: 0x00010B05
		private void UpdatePosition(Vector2f position)
		{
			this.position = position;
			this.transform = Transform.Identity;
			this.transform.Translate(this.position);
			this.states.Transform = this.transform;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001293C File Offset: 0x00010B3C
		private void UpdateProgress(float progress)
		{
			float num = this.progress;
			this.progress = progress;
			if (this.progress != num)
			{
				this.shader.SetParameter("progress", this.progress);
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00012976 File Offset: 0x00010B76
		private void SetProgress(float progress)
		{
			if (this.speed > 0f)
			{
				this.targetProgress = progress;
				this.animationDone = false;
				return;
			}
			this.UpdateProgress(progress);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001299C File Offset: 0x00010B9C
		private void UpdateAnimation()
		{
			if (!this.animationDone && this.speed > 0f)
			{
				if (Math.Abs(this.targetProgress - this.progress) > 0.01f)
				{
					float num = this.progress + (float)Math.Sign(this.targetProgress - this.progress) * this.speed;
					this.UpdateProgress(num);
					return;
				}
				this.animationDone = true;
				this.progress = this.targetProgress;
				if (this.OnAnimationComplete != null)
				{
					this.OnAnimationComplete(this);
				}
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00012A28 File Offset: 0x00010C28
		public override void Draw(RenderTarget target)
		{
			this.UpdatePosition(ViewManager.Instance.FinalCenter);
			this.UpdateAnimation();
			target.Draw(this.verts, this.states);
		}

		// Token: 0x04000430 RID: 1072
		private const string PARAM_PROGRESS = "progress";

		// Token: 0x04000431 RID: 1073
		private const string PARAM_SIZE = "size";

		// Token: 0x04000432 RID: 1074
		private float progress;

		// Token: 0x04000433 RID: 1075
		private float targetProgress;

		// Token: 0x04000434 RID: 1076
		private float speed;

		// Token: 0x04000435 RID: 1077
		private bool animationDone;

		// Token: 0x04000436 RID: 1078
		private Shader shader;

		// Token: 0x04000437 RID: 1079
		private Transform transform;

		// Token: 0x04000438 RID: 1080
		private RenderStates states;

		// Token: 0x04000439 RID: 1081
		private VertexArray verts;

		// Token: 0x0200008E RID: 142
		// (Invoke) Token: 0x060002E7 RID: 743
		public delegate void AnimationCompleteHandler(IrisOverlay sender);
	}
}
