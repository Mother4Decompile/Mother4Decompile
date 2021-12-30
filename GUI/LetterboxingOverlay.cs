using System;
using Carbine;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	// Token: 0x0200003E RID: 62
	internal class LetterboxingOverlay : AnimatedRenderable
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00008914 File Offset: 0x00006B14
		public LetterboxingOverlay()
		{
			this.visible = false;
			this.depth = 2147450870;
			this.size = Engine.SCREEN_SIZE;
			this.origin = VectorMath.Truncate(Engine.SCREEN_SIZE / 2f);
			Vector2f size = new Vector2f(320f, 14f);
			this.topLetterbox = new RectangleShape(size);
			this.topLetterbox.FillColor = Color.Black;
			this.bottomLetterbox = new RectangleShape(size);
			this.bottomLetterbox.FillColor = Color.Black;
			this.topLetterboxY = -14f;
			this.bottomLetterboxY = 180f;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000089C0 File Offset: 0x00006BC0
		private void UpdateLetterboxing(float amount)
		{
			float num = Math.Max(0f, Math.Min(1f, amount));
			this.topLetterboxY = (float)((int)(-14f * (1f - num)));
			this.bottomLetterboxY = (float)(180L - (long)((int)(14f * num)));
			this.topLetterbox.Position = new Vector2f(ViewManager.Instance.Viewrect.Left, ViewManager.Instance.Viewrect.Top + this.topLetterboxY);
			this.bottomLetterbox.Position = new Vector2f(ViewManager.Instance.Viewrect.Left, ViewManager.Instance.Viewrect.Top + this.bottomLetterboxY);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00008A7C File Offset: 0x00006C7C
		public void Show()
		{
			this.position = ViewManager.Instance.FinalCenter;
			this.visible = true;
			if (this.letterboxProgressTarget <= 0f)
			{
				this.letterboxProgress = 0f;
				this.letterboxProgressTarget = 1f;
				this.UpdateLetterboxing(this.letterboxProgress);
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008ACF File Offset: 0x00006CCF
		public void Hide()
		{
			if (this.letterboxProgressTarget > 0f)
			{
				this.letterboxProgress = 1f;
				this.letterboxProgressTarget = 0f;
				this.UpdateLetterboxing(this.letterboxProgress);
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008B00 File Offset: 0x00006D00
		private float TweenLetterboxing()
		{
			return (float)(0.5 - Math.Cos((double)this.letterboxProgress * 3.141592653589793) / 2.0);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008B30 File Offset: 0x00006D30
		private void UpdateAnimation()
		{
			this.position = ViewManager.Instance.FinalCenter;
			Vector2f vector2f = this.position - ViewManager.Instance.View.Size / 2f;
			this.topLetterbox.Position = new Vector2f(vector2f.X, vector2f.Y + this.topLetterboxY);
			this.bottomLetterbox.Position = new Vector2f(vector2f.X, vector2f.Y + this.bottomLetterboxY);
			if (this.letterboxProgress + 0.2f < this.letterboxProgressTarget)
			{
				this.letterboxProgress += 0.2f;
			}
			else if (this.letterboxProgress - 0.2f > this.letterboxProgressTarget)
			{
				this.letterboxProgress -= 0.2f;
			}
			else if (this.letterboxProgressTarget <= 0f)
			{
				this.visible = false;
				base.AnimationComplete();
			}
			this.UpdateLetterboxing(this.TweenLetterboxing());
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00008C32 File Offset: 0x00006E32
		public override void Draw(RenderTarget target)
		{
			if (this.visible)
			{
				this.UpdateAnimation();
				target.Draw(this.topLetterbox);
				target.Draw(this.bottomLetterbox);
			}
		}

		// Token: 0x04000234 RID: 564
		private const float LETTERBOX_HEIGHT = 14f;

		// Token: 0x04000235 RID: 565
		private const float LETTERBOX_SPEED = 0.2f;

		// Token: 0x04000236 RID: 566
		private const int DEPTH = 2147450870;

		// Token: 0x04000237 RID: 567
		private RectangleShape topLetterbox;

		// Token: 0x04000238 RID: 568
		private RectangleShape bottomLetterbox;

		// Token: 0x04000239 RID: 569
		private float letterboxProgress;

		// Token: 0x0400023A RID: 570
		private float letterboxProgressTarget;

		// Token: 0x0400023B RID: 571
		private float topLetterboxY;

		// Token: 0x0400023C RID: 572
		private float bottomLetterboxY;
	}
}
