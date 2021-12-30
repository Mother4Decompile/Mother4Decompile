using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Carbine.Scenes.Transitions;
using Carbine.Utility;
using Mother4.Overworld;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes.Transitions
{
	// Token: 0x02000116 RID: 278
	internal class BattleSwirlTransition : ITransition
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0002AA12 File Offset: 0x00028C12
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0002AA1A File Offset: 0x00028C1A
		public float Progress
		{
			get
			{
				return this.progress;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0002AA22 File Offset: 0x00028C22
		public bool ShowNewScene
		{
			get
			{
				return this.isComplete;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0002AA2A File Offset: 0x00028C2A
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x0002AA32 File Offset: 0x00028C32
		public bool Blocking { get; set; }

		// Token: 0x060006BB RID: 1723 RVA: 0x0002AA3C File Offset: 0x00028C3C
		public BattleSwirlTransition(BattleSwirlOverlay.Style style)
		{
			this.overlay = new BattleSwirlOverlay(style, 0, 0.015f);
			this.overlay.OnAnimationComplete += this.overlay_OnAnimationComplete;
			int num = 160;
			int num2 = 90;
			this.fadeColor = BattleSwirlTransition.COLOR_MAP[style];
			this.fadeStartColor = new Color(this.fadeColor);
			this.fadeStartColor.A = 0;
			this.fadeVerts = new Vertex[4];
			this.fadeVerts[0] = new Vertex(new Vector2f((float)(-(float)num), (float)(-(float)num2)), this.fadeStartColor);
			this.fadeVerts[1] = new Vertex(new Vector2f((float)num, (float)(-(float)num2)), this.fadeStartColor);
			this.fadeVerts[2] = new Vertex(new Vector2f((float)num, (float)num2), this.fadeStartColor);
			this.fadeVerts[3] = new Vertex(new Vector2f((float)(-(float)num), (float)num2), this.fadeStartColor);
			this.fadeStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, null);
			this.UpdateTransform();
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002AB70 File Offset: 0x00028D70
		private void overlay_OnAnimationComplete(BattleSwirlOverlay anim)
		{
			this.overlay.OnAnimationComplete -= this.overlay_OnAnimationComplete;
			this.isSwirlComplete = true;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002AB90 File Offset: 0x00028D90
		private void UpdateTransform()
		{
			this.fadeStates.Transform = new Transform(1f, 0f, ViewManager.Instance.FinalCenter.X, 0f, 1f, ViewManager.Instance.FinalCenter.Y, 0f, 0f, 1f);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002ABF0 File Offset: 0x00028DF0
		public void Update()
		{
			if (!this.isComplete)
			{
				if (this.isSwirlComplete)
				{
					this.progress += 0.024f;
					Color color = (this.progress < 0.7f) ? ColorHelper.BlendAlpha(this.fadeStartColor, this.fadeColor, this.progress / 0.7f) : ColorHelper.BlendAlpha(this.fadeColor, Color.Black, (this.progress - 0.7f) / 0.3f);
					for (int i = 0; i < this.fadeVerts.Length; i++)
					{
						this.fadeVerts[i].Color = color;
					}
				}
				if (this.progress >= 1f)
				{
					this.overlay.Dispose();
					this.overlay = null;
					this.fadeVerts = null;
					this.progress = 1f;
					this.isComplete = true;
				}
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002ACD1 File Offset: 0x00028ED1
		public void Draw()
		{
			if (!this.isComplete)
			{
				this.UpdateTransform();
				this.overlay.Draw(Engine.FrameBuffer);
				Engine.FrameBuffer.Draw(this.fadeVerts, PrimitiveType.Quads, this.fadeStates);
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002AD08 File Offset: 0x00028F08
		public void Reset()
		{
			this.overlay.Reset();
		}

		// Token: 0x040008AD RID: 2221
		private const float SWIRL_SPEED = 0.015f;

		// Token: 0x040008AE RID: 2222
		private const float FADE_SPEED = 0.024f;

		// Token: 0x040008AF RID: 2223
		private static Dictionary<BattleSwirlOverlay.Style, Color> COLOR_MAP = new Dictionary<BattleSwirlOverlay.Style, Color>
		{
			{
				BattleSwirlOverlay.Style.Green,
				new Color(148, 214, 161, byte.MaxValue)
			},
			{
				BattleSwirlOverlay.Style.Blue,
				new Color(160, 234, 250, byte.MaxValue)
			},
			{
				BattleSwirlOverlay.Style.Red,
				new Color(250, 160, 167, byte.MaxValue)
			},
			{
				BattleSwirlOverlay.Style.Boss,
				new Color(242, 220, 179, byte.MaxValue)
			}
		};

		// Token: 0x040008B0 RID: 2224
		private bool isComplete;

		// Token: 0x040008B1 RID: 2225
		private bool isSwirlComplete;

		// Token: 0x040008B2 RID: 2226
		private float progress;

		// Token: 0x040008B3 RID: 2227
		private BattleSwirlOverlay overlay;

		// Token: 0x040008B4 RID: 2228
		private Vertex[] fadeVerts;

		// Token: 0x040008B5 RID: 2229
		private RenderStates fadeStates;

		// Token: 0x040008B6 RID: 2230
		private Color fadeColor;

		// Token: 0x040008B7 RID: 2231
		private Color fadeStartColor;
	}
}
