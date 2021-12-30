using System;
using System.Text;
using Mother4.Battle.Combatants;
using Mother4.Battle.PsiAnimation;
using Mother4.Data;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	// Token: 0x02000060 RID: 96
	internal class ShieldAction : StatusEffectAction
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000DB74 File Offset: 0x0000BD74
		public ShieldAction(ActionParams aparams) : base(aparams)
		{
			if (this.sender is PlayerCombatant)
			{
				this.actionStartSound = this.controller.InterfaceController.PrePlayerAttack;
			}
			else if (this.sender is EnemyCombatant)
			{
				this.actionStartSound = this.controller.InterfaceController.PreEnemyAttack;
			}
			this.effect = (StatusEffectInstance)aparams.data[0];
			this.names = new string[this.targets.Length];
			for (int i = 0; i < this.targets.Length; i++)
			{
				this.names[i] = base.BuildCombatantName(this.targets[i]);
			}
			this.state = ShieldAction.State.Initialize;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000DC28 File Offset: 0x0000BE28
		protected override void UpdateAction()
		{
			switch (this.state)
			{
			case ShieldAction.State.Initialize:
				this.Initialize();
				return;
			case ShieldAction.State.Animate:
				this.Animate();
				return;
			case ShieldAction.State.WaitForUI:
				this.WaitForUI();
				return;
			case ShieldAction.State.Finish:
				this.Finish();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000DC70 File Offset: 0x0000BE70
		protected override void Initialize()
		{
			if (this.sender is PlayerCombatant)
			{
				this.controller.InterfaceController.PopCard(this.sender.ID, 12);
			}
			else if (this.sender is EnemyCombatant)
			{
				this.controller.InterfaceController.FlashEnemy(this.sender as EnemyCombatant, Color.Black, 8, 2);
			}
			this.controller.InterfaceController.OnTextTrigger += this.OnTextTrigger;
			this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
			string arg = string.Empty;
			if (this.sender is PlayerCombatant)
			{
				arg = CharacterNames.GetName(((PlayerCombatant)this.sender).Character);
			}
			else if (this.sender is EnemyCombatant)
			{
				arg = string.Format("{0} {1}", EnemyNames.GetArticle(((EnemyCombatant)this.sender).Enemy), EnemyNames.GetName(((EnemyCombatant)this.sender).Enemy)).Trim();
			}
			string message = string.Format("{0} tried Shield {1}!", arg, PsiLetters.Get(3));
			this.controller.InterfaceController.ShowTextBox(message, false);
			StatSet statChange = new StatSet
			{
				PP = -20
			};
			this.sender.AlterStats(statChange);
			this.nextState = ShieldAction.State.Animate;
			this.state = ShieldAction.State.WaitForUI;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000DDD8 File Offset: 0x0000BFD8
		protected void Animate()
		{
			PsiElementList animation = PsiAnimations.Get(8);
			PsiAnimator psiAnimator = this.controller.InterfaceController.AddPsiAnimation(animation, this.sender, this.targets);
			psiAnimator.OnAnimationComplete += this.OnAnimationComplete;
			this.nextState = ShieldAction.State.Finish;
			this.state = ShieldAction.State.WaitForUI;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000DE2C File Offset: 0x0000C02C
		protected override void Finish()
		{
			if (this.sender is PlayerCombatant)
			{
				this.controller.InterfaceController.PopCard(this.sender.ID, 0);
			}
			this.controller.InterfaceController.OnTextTrigger -= this.OnTextTrigger;
			this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
			this.complete = true;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000DEA4 File Offset: 0x0000C0A4
		protected virtual void OnAnimationComplete(PsiAnimator anim)
		{
			anim.OnAnimationComplete -= this.OnAnimationComplete;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.names.Length; i++)
			{
				stringBuilder.AppendFormat("[t:{0},{1}]{2} was protected by the shield.", 200, i, this.names[i]);
				if (i < this.names.Length - 1)
				{
					stringBuilder.Append("\n");
				}
			}
			this.controller.InterfaceController.ShowTextBox(stringBuilder.ToString(), false);
			this.nextState = ShieldAction.State.Finish;
			this.state = ShieldAction.State.WaitForUI;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000DF40 File Offset: 0x0000C140
		protected virtual void OnTextTrigger(int type, string[] args)
		{
			if (type == 200)
			{
				int num = 0;
				if (int.TryParse(args[0], out num) && num >= 0 && num < this.targets.Length)
				{
					this.controller.InterfaceController.AddShieldAnimation(this.targets[num]);
					this.targets[num].AddStatusEffect(this.effect);
				}
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000DF9E File Offset: 0x0000C19E
		protected override void InteractionComplete()
		{
			this.state = this.nextState;
		}

		// Token: 0x0400033E RID: 830
		private const int EFFECT_TYPE_INDEX = 0;

		// Token: 0x0400033F RID: 831
		private const int CARD_POP_HEIGHT = 12;

		// Token: 0x04000340 RID: 832
		private const bool USE_BUTTON = false;

		// Token: 0x04000341 RID: 833
		private const int BLINK_DURATION = 8;

		// Token: 0x04000342 RID: 834
		private const int BLINK_COUNT = 2;

		// Token: 0x04000343 RID: 835
		private const int SHIELD_TRIGGER_ID = 200;

		// Token: 0x04000344 RID: 836
		private ShieldAction.State state;

		// Token: 0x04000345 RID: 837
		private ShieldAction.State nextState;

		// Token: 0x04000346 RID: 838
		private string[] names;

		// Token: 0x02000061 RID: 97
		private enum State
		{
			// Token: 0x04000348 RID: 840
			Initialize,
			// Token: 0x04000349 RID: 841
			Animate,
			// Token: 0x0400034A RID: 842
			WaitForUI,
			// Token: 0x0400034B RID: 843
			Finish
		}
	}
}
