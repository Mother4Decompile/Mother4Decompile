using System;
using Carbine.Flags;
using Carbine.Input;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Carbine.Utility;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.Data.Psi;
using Mother4.GUI;
using Mother4.GUI.OverworldMenu;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x0200010A RID: 266
	internal class MenuScene : StandardScene
	{
		// Token: 0x06000633 RID: 1587 RVA: 0x00025364 File Offset: 0x00023564
		public MenuScene()
		{
			this.mainPanel = new MainMenu();
			this.mainPanel.Visible = false;
			this.pipeline.Add(this.mainPanel);
			this.activePanel = this.mainPanel;
			this.moneyPanel = new MoneyMenu();
			this.moneyPanel.Visible = false;
			this.pipeline.Add(this.moneyPanel);
			this.goodsPanel = new GoodsMenu();
			this.goodsPanel.Visible = false;
			this.pipeline.Add(this.goodsPanel);
			this.psiPanel = new PsiMenu();
			this.psiPanel.Visible = false;
			this.pipeline.Add(this.psiPanel);
			this.cardBar = new CardBar(this.pipeline, PartyManager.Instance.ToArray());
			this.cardBar.Hide(true);
			this.cardBar.Show();
			this.actorManager.Add(this.cardBar);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00025466 File Offset: 0x00023666
		private void Initialize()
		{
			if (!this.initialized)
			{
				this.mainPanel.Visible = true;
				this.moneyPanel.Visible = true;
				this.initialized = true;
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0002548F File Offset: 0x0002368F
		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			this.isCursorTime = true;
			this.axis = axis;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0002549F File Offset: 0x0002369F
		private void ChangeActivePanel(MenuPanel panel)
		{
			this.activePanel.Unfocus();
			this.activePanel = panel;
			this.activePanel.Visible = true;
			this.activePanel.Focus();
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000254CA File Offset: 0x000236CA
		private void ExitMenu()
		{
			SceneManager.Instance.Transition = new InstantTransition();
			SceneManager.Instance.Pop();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000254E8 File Offset: 0x000236E8
		private void HandleMainMenuButton(object retVal)
		{
			switch (((int?)retVal).Value)
			{
			case -1:
				this.ExitMenu();
				return;
			case 0:
				this.ChangeActivePanel(this.goodsPanel);
				return;
			case 1:
				this.ChangeActivePanel(this.psiPanel);
				break;
			case 2:
			case 3:
				break;
			default:
				return;
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00025544 File Offset: 0x00023744
		private void HandleGoodsMenuButton(object retVal)
		{
			int value = ((int?)retVal).Value;
			if (value != -1)
			{
				return;
			}
			this.activePanel.Unfocus();
			this.goodsPanel.Visible = false;
			this.activePanel = this.mainPanel;
			this.activePanel.Focus();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00025594 File Offset: 0x00023794
		private void HandlePsiMenuButton(object retVal)
		{
			if (!(retVal is int))
			{
				if (retVal is PsiLevel && ((PsiLevel)retVal).PsiType.Identifier == Hash.Get("psi.other.telepathy"))
				{
					FlagManager.Instance[3] = true;
					this.ExitMenu();
				}
				return;
			}
			int num = (int)retVal;
			if (num != -1)
			{
				return;
			}
			this.activePanel.Unfocus();
			this.psiPanel.Visible = false;
			this.activePanel = this.mainPanel;
			this.activePanel.Focus();
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00025622 File Offset: 0x00023822
		private void ButtonPressed(InputManager sender, Button b)
		{
			this.isButtonTime = true;
			this.button = b;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00025634 File Offset: 0x00023834
		public override void Focus()
		{
			base.Focus();
			SceneManager.Instance.CompositeMode = true;
			base.DrawBehind = true;
			this.Initialize();
			InputManager.Instance.AxisPressed += this.AxisPressed;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0002568C File Offset: 0x0002388C
		public override void Unfocus()
		{
			base.Unfocus();
			SceneManager.Instance.CompositeMode = false;
			base.DrawBehind = false;
			InputManager.Instance.AxisPressed -= this.AxisPressed;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x000256E0 File Offset: 0x000238E0
		public override void Update()
		{
			base.Update();
			if (this.isButtonTime)
			{
				if (this.activePanel != null)
				{
					object obj = this.activePanel.ButtonPressed(this.button);
					if (obj != null)
					{
						if (this.activePanel is MainMenu)
						{
							this.HandleMainMenuButton(obj);
						}
						else if (this.activePanel is GoodsMenu)
						{
							this.HandleGoodsMenuButton(obj);
						}
						else if (this.activePanel is PsiMenu)
						{
							this.HandlePsiMenuButton(obj);
						}
					}
				}
				this.isButtonTime = false;
			}
			if (this.isCursorTime)
			{
				if (this.activePanel != null)
				{
					this.activePanel.AxisPressed(this.axis);
				}
				this.isCursorTime = false;
			}
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00025788 File Offset: 0x00023988
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.mainPanel.Dispose();
				this.goodsPanel.Dispose();
				this.psiPanel.Dispose();
				this.moneyPanel.Dispose();
				this.cardBar.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400080A RID: 2058
		private bool initialized;

		// Token: 0x0400080B RID: 2059
		private bool isButtonTime;

		// Token: 0x0400080C RID: 2060
		private bool isCursorTime;

		// Token: 0x0400080D RID: 2061
		private Button button;

		// Token: 0x0400080E RID: 2062
		private Vector2f axis;

		// Token: 0x0400080F RID: 2063
		private MenuPanel activePanel;

		// Token: 0x04000810 RID: 2064
		private MenuPanel mainPanel;

		// Token: 0x04000811 RID: 2065
		private MenuPanel moneyPanel;

		// Token: 0x04000812 RID: 2066
		private MenuPanel goodsPanel;

		// Token: 0x04000813 RID: 2067
		private MenuPanel psiPanel;

		// Token: 0x04000814 RID: 2068
		private CardBar cardBar;
	}
}
