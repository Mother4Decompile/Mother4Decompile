using System;
using Carbine;
using Carbine.Input;
using Mother4.Overworld;
using SFML.Graphics;

namespace Mother4.Scenes
{
	// Token: 0x02000112 RID: 274
	internal class SwirlTestScene : StandardScene
	{
		// Token: 0x06000696 RID: 1686 RVA: 0x00029A20 File Offset: 0x00027C20
		public SwirlTestScene()
		{
			this.swirl = new BattleSwirlOverlay(BattleSwirlOverlay.Style.Green, 0, 0.01f);
			this.swirl.OnAnimationComplete += this.AnimationComplete;
			this.swirl.Visible = true;
			this.pipeline.Add(this.swirl);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00029A79 File Offset: 0x00027C79
		private void AnimationComplete(BattleSwirlOverlay anim)
		{
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00029A7B File Offset: 0x00027C7B
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.A)
			{
				this.swirl.Reset();
				this.swirl.Visible = true;
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00029A97 File Offset: 0x00027C97
		public override void Focus()
		{
			base.Focus();
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			Engine.ClearColor = Color.Magenta;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00029ABF File Offset: 0x00027CBF
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00029AC7 File Offset: 0x00027CC7
		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00029AE5 File Offset: 0x00027CE5
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.swirl.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000896 RID: 2198
		private BattleSwirlOverlay swirl;
	}
}
