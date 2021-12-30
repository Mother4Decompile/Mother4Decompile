using System;
using System.Linq;
using Mother4.Battle.Combatants;
using Mother4.Battle.PsiAnimation;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.Data.Psi;
using Rufini.Strings;

namespace Mother4.Battle.Actions
{
	// Token: 0x0200005D RID: 93
	internal class PsiAction : BattleAction
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000D5AC File Offset: 0x0000B7AC
		public PsiAction(ActionParams aparams) : base(aparams)
		{
			if (aparams.data.Length == 1)
			{
				this.psiLevel = (this.psiLevel = (PsiLevel)aparams.data[0]);
				this.psiData = PsiFile.Instance.GetData(this.psiLevel.PsiType);
			}
			if (this.psiData is OffensePsiData)
			{
				this.SetupOffensivePsi();
			}
			else if (this.psiData is DefensePsiData)
			{
				this.SetupDefensivePsi();
			}
			else if (this.psiData is AssistPsiData)
			{
				this.SetupAssistivePsi();
			}
			else
			{
				if (!(this.psiData is OtherPsiData))
				{
					this.state = PsiAction.State.Finish;
					return;
				}
				this.SetupOtherPsi();
			}
			this.state = PsiAction.State.Initialize;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000D668 File Offset: 0x0000B868
		private void SetupStatusEffects()
		{
			if (this.psiData.Key == "psi.shield")
			{
				this.statusEffects = new StatusEffect[]
				{
					StatusEffect.Shield
				};
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000D6A0 File Offset: 0x0000B8A0
		private void SetupOffensivePsi()
		{
			OffensePsiData offensePsiData = (OffensePsiData)this.psiData;
			if (offensePsiData.Damage != null && offensePsiData.Damage.Length >= this.psiLevel.Level)
			{
				this.minDamage = offensePsiData.Damage[this.psiLevel.Level].Min;
				this.maxDamage = offensePsiData.Damage[this.psiLevel.Level].Max;
			}
			this.SetupStatusEffects();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000D71E File Offset: 0x0000B91E
		private void SetupDefensivePsi()
		{
			DefensePsiData defensePsiData = (DefensePsiData)this.psiData;
			this.SetupStatusEffects();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000D732 File Offset: 0x0000B932
		private void SetupAssistivePsi()
		{
			AssistPsiData assistPsiData = (AssistPsiData)this.psiData;
			this.SetupStatusEffects();
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000D746 File Offset: 0x0000B946
		private void SetupOtherPsi()
		{
			OtherPsiData otherPsiData = (OtherPsiData)this.psiData;
			this.SetupStatusEffects();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000D75C File Offset: 0x0000B95C
		private void RemoveInvalidTargets()
		{
			Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.EnemyTeam, true);
			bool[] array = new bool[this.targets.Length];
			int num = this.targets.Length;
			for (int i = 0; i < this.targets.Length; i++)
			{
				Combatant combatant = this.targets[i];
				if (!this.controller.CombatantController.IsIdValid(combatant.ID))
				{
					array[i] = true;
					num--;
					foreach (Combatant combatant2 in factionCombatants)
					{
						if (!this.targets.Contains(combatant2))
						{
							this.targets[i] = combatant2;
							array[i] = false;
							num++;
							break;
						}
					}
				}
			}
			Combatant[] array3 = new Combatant[num];
			int k = 0;
			int num2 = 0;
			while (k < this.targets.Length)
			{
				if (!array[k])
				{
					array3[num2] = this.targets[k];
					num2++;
				}
				k++;
			}
			this.targets = array3;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000D85C File Offset: 0x0000BA5C
		private void DoInitialize()
		{
			string arg = StringFile.Instance.Get(this.psiData.Key).Value ?? string.Empty;
			string message = string.Format("{0} tried {1} {2}!", CharacterNames.GetName(((PlayerCombatant)this.sender).Character), arg, PsiLetters.Get((int)this.psiData.Symbols[this.psiLevel.Level]));
			this.RemoveInvalidTargets();
			this.controller.InterfaceController.OnTextboxComplete += this.OnTextboxComplete;
			this.controller.InterfaceController.ShowTextBox(message, false);
			this.controller.InterfaceController.PrePsiSound.Play();
			this.controller.InterfaceController.PopCard(((PlayerCombatant)this.sender).ID, 12);
			this.state = PsiAction.State.WaitForUI;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000D940 File Offset: 0x0000BB40
		private void DoAnimate()
		{
			PsiElementList animation = PsiAnimations.Get((int)this.psiData.Animations[this.psiLevel.Level]);
			PsiAnimator psiAnimator = this.controller.InterfaceController.AddPsiAnimation(animation, this.sender, this.targets);
			psiAnimator.OnAnimationComplete += this.OnAnimationComplete;
			this.state = PsiAction.State.WaitForUI;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000D9A4 File Offset: 0x0000BBA4
		private void DoDamageNumbers()
		{
			foreach (Combatant combatant in this.targets)
			{
				int num = BattleCalculator.CalculatePsiDamage(this.minDamage, this.maxDamage, this.sender, combatant);
				DamageNumber damageNumber = this.controller.InterfaceController.AddDamageNumber(combatant, num);
				damageNumber.OnComplete += this.OnDamageNumberComplete;
				StatSet statChange = new StatSet
				{
					HP = -num
				};
				combatant.AlterStats(statChange);
				this.controller.InterfaceController.BlinkEnemy(combatant as EnemyCombatant, 3, 2);
			}
			StatSet statChange2 = new StatSet
			{
				PP = (int)(-(int)this.psiData.PP[this.psiLevel.Level]),
				Meter = 0.026666667f
			};
			this.sender.AlterStats(statChange2);
			this.state = PsiAction.State.WaitForUI;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000DA91 File Offset: 0x0000BC91
		private void DoFinish()
		{
			this.controller.InterfaceController.PopCard(((PlayerCombatant)this.sender).ID, 0);
			this.complete = true;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000DABC File Offset: 0x0000BCBC
		protected override void UpdateAction()
		{
			switch (this.state)
			{
			case PsiAction.State.Initialize:
				this.DoInitialize();
				return;
			case PsiAction.State.Animate:
				this.DoAnimate();
				return;
			case PsiAction.State.WaitForUI:
				break;
			case PsiAction.State.DamageNumbers:
				this.DoDamageNumbers();
				return;
			case PsiAction.State.Finish:
				this.DoFinish();
				break;
			default:
				return;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000DB06 File Offset: 0x0000BD06
		private void OnDamageNumberComplete(DamageNumber sender)
		{
			sender.OnComplete -= this.OnDamageNumberComplete;
			this.state = PsiAction.State.Finish;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000DB21 File Offset: 0x0000BD21
		private void OnTextboxComplete()
		{
			this.controller.InterfaceController.OnTextboxComplete -= this.OnTextboxComplete;
			this.controller.InterfaceController.HideTextBox();
			this.state = PsiAction.State.Animate;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000DB56 File Offset: 0x0000BD56
		private void OnAnimationComplete(PsiAnimator anim)
		{
			anim.OnAnimationComplete -= this.OnAnimationComplete;
			this.state = PsiAction.State.DamageNumbers;
		}

		// Token: 0x04000328 RID: 808
		private const float ONE_GP = 0.013333334f;

		// Token: 0x04000329 RID: 809
		private const int CARD_POP_HEIGHT = 12;

		// Token: 0x0400032A RID: 810
		private const int DAMAGE_NUMBER_WAIT = 70;

		// Token: 0x0400032B RID: 811
		private const int PSI_INDEX = 0;

		// Token: 0x0400032C RID: 812
		private const int PSI_LEVEL_INDEX = 1;

		// Token: 0x0400032D RID: 813
		private PsiAction.State state;

		// Token: 0x0400032E RID: 814
		private PsiData psiData;

		// Token: 0x0400032F RID: 815
		private PsiLevel psiLevel;

		// Token: 0x04000330 RID: 816
		private int minDamage;

		// Token: 0x04000331 RID: 817
		private int maxDamage;

		// Token: 0x04000332 RID: 818
		private StatusEffect[] statusEffects;

		// Token: 0x0200005E RID: 94
		private enum State
		{
			// Token: 0x04000334 RID: 820
			Initialize,
			// Token: 0x04000335 RID: 821
			Animate,
			// Token: 0x04000336 RID: 822
			WaitForUI,
			// Token: 0x04000337 RID: 823
			DamageNumbers,
			// Token: 0x04000338 RID: 824
			Finish
		}

		// Token: 0x0200005F RID: 95
		private enum Mode
		{
			// Token: 0x0400033A RID: 826
			Offensive,
			// Token: 0x0400033B RID: 827
			Defensive,
			// Token: 0x0400033C RID: 828
			Assistive,
			// Token: 0x0400033D RID: 829
			Other
		}
	}
}
