using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Graphics;
using Carbine.Input;
using Mother4.Data;
using Mother4.GUI;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x02000111 RID: 273
	internal class PsiTestScene : StandardScene
	{
		// Token: 0x0600068E RID: 1678 RVA: 0x00029790 File Offset: 0x00027990
		public PsiTestScene()
		{
			this.animations = new List<MultipartAnimation>();
			IEnumerable<string> enumerable = Directory.EnumerateFiles(Paths.PSI_GRAPHICS, "*.sdat");
			foreach (string resource in enumerable)
			{
				MultipartAnimation multipartAnimation = new MultipartAnimation(resource, new Vector2f(160f, 90f), 0.5f, 0);
				multipartAnimation.Visible = false;
				multipartAnimation.OnAnimationComplete += this.AnimationComplete;
				this.pipeline.Add(multipartAnimation);
				this.animations.Add(multipartAnimation);
			}
			this.animIndex = 0;
			this.animations[this.animIndex].Visible = true;
			this.psiList = new PsiList(new Vector2f(16f, 16f), CharacterType.Meryl, 288, 4, 0);
			this.pipeline.Add(this.psiList);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00029894 File Offset: 0x00027A94
		private void AnimationComplete(AnimatedRenderable anim)
		{
			anim.Visible = false;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002989D File Offset: 0x00027A9D
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.A)
			{
				this.psiList.Accept();
				return;
			}
			if (b == Button.B)
			{
				this.psiList.Cancel();
			}
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000298C0 File Offset: 0x00027AC0
		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			if (axis.X < 0f)
			{
				this.psiList.SelectLeft();
			}
			else if (axis.X > 0f)
			{
				this.psiList.SelectRight();
			}
			if (axis.Y < 0f)
			{
				this.psiList.SelectUp();
				return;
			}
			if (axis.Y > 0f)
			{
				this.psiList.SelectDown();
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00029934 File Offset: 0x00027B34
		public override void Focus()
		{
			base.Focus();
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			InputManager.Instance.AxisPressed += this.AxisPressed;
			this.psiList.Show();
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00029973 File Offset: 0x00027B73
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002997B File Offset: 0x00027B7B
		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			InputManager.Instance.AxisPressed -= this.AxisPressed;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x000299B0 File Offset: 0x00027BB0
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.psiList.Dispose();
				foreach (MultipartAnimation multipartAnimation in this.animations)
				{
					multipartAnimation.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000893 RID: 2195
		private List<MultipartAnimation> animations;

		// Token: 0x04000894 RID: 2196
		private int animIndex;

		// Token: 0x04000895 RID: 2197
		private PsiList psiList;
	}
}
