using System;
using Mother4.Battle.Combatants;
using Mother4.Battle.PsiAnimation;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.Utility;

namespace Mother4.Battle.Actions
{
	// Token: 0x02000054 RID: 84
	internal class EnemyProjectileAction : BattleAction
	{
		// Token: 0x06000211 RID: 529 RVA: 0x0000CB70 File Offset: 0x0000AD70
		public EnemyProjectileAction(ActionParams aparams) : base(aparams)
		{
			this.enemySender = (this.sender as EnemyCombatant);
			this.playerTarget = ((this.targets.Length > 0) ? (this.targets[0] as PlayerCombatant) : null);
			this.projectileName = ((aparams.data.Length > 0) ? ((string)aparams.data[0]) : "");
			this.targetDamage = ((aparams.data.Length > 1) ? ((int)aparams.data[1]) : 0);
			this.isReflected = BattleCalculator.CalculateReflection(this.sender, this.targets[0]);
			this.state = EnemyProjectileAction.State.Initialize;
			this.previousState = this.state;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000CC2C File Offset: 0x0000AE2C
		private void ChangeState(EnemyProjectileAction.State newState)
		{
			this.previousState = this.state;
			this.state = newState;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000CC44 File Offset: 0x0000AE44
		private void DoAnimation(int index)
		{
			PsiElementList animation = PsiAnimations.Get(index);
			PsiAnimator psiAnimator = this.controller.InterfaceController.AddPsiAnimation(animation, this.sender, this.targets);
			psiAnimator.OnAnimationComplete += this.OnAnimationComplete;
			this.ChangeState(EnemyProjectileAction.State.WaitForUI);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000CC90 File Offset: 0x0000AE90
		protected override void UpdateAction()
		{
			base.UpdateAction();
			if (this.state == EnemyProjectileAction.State.Initialize)
			{
				string message = string.Format("{0}{1} flung {2}!", Capitalizer.Capitalize(EnemyNames.GetArticle(this.enemySender.Enemy)), EnemyNames.GetName(this.enemySender.Enemy), this.projectileName);
				this.controller.InterfaceController.OnTextboxComplete += this.OnTextboxComplete;
				this.controller.InterfaceController.ShowTextBox(message, false);
				this.controller.InterfaceController.PreEnemyAttack.Play();
				this.ChangeState(EnemyProjectileAction.State.WaitForUI);
				return;
			}
			if (this.state == EnemyProjectileAction.State.AnimateThrow)
			{
				this.DoAnimation(4);
				return;
			}
			if (this.state == EnemyProjectileAction.State.ShowReflectMessage)
			{
				string message2 = string.Format("{0} hit it back!", Capitalizer.Capitalize(CharacterNames.GetName(this.playerTarget.Character)));
				this.controller.InterfaceController.ShowTextBox(message2, false);
				this.controller.InterfaceController.ReflectSound.Play();
				this.ChangeState(EnemyProjectileAction.State.WaitForUI);
				return;
			}
			if (this.state == EnemyProjectileAction.State.AnimateReflect)
			{
				this.DoAnimation(5);
				return;
			}
			if (this.state == EnemyProjectileAction.State.AnimateHit)
			{
				this.DoAnimation(6);
				return;
			}
			if (this.state == EnemyProjectileAction.State.DamageNumbers)
			{
				if (this.isReflected)
				{
					int num = BattleCalculator.CalculateProjectileDamage(this.targetDamage, this.sender, this.sender);
					DamageNumber damageNumber = this.controller.InterfaceController.AddDamageNumber(this.sender, num);
					damageNumber.OnComplete += this.OnDamageNumberComplete;
					StatSet statChange = new StatSet
					{
						HP = -num
					};
					this.sender.AlterStats(statChange);
					this.controller.InterfaceController.BlinkEnemy(this.sender as EnemyCombatant, 3, 2);
					StatSet statChange2 = new StatSet
					{
						Meter = 0.2f
					};
					this.playerTarget.AlterStats(statChange2);
				}
				else
				{
					foreach (Combatant combatant in this.targets)
					{
						int num2 = BattleCalculator.CalculateProjectileDamage(this.targetDamage, this.sender, combatant);
						DamageNumber damageNumber2 = this.controller.InterfaceController.AddDamageNumber(combatant, num2);
						damageNumber2.OnComplete += this.OnDamageNumberComplete;
						StatSet statChange3 = new StatSet
						{
							HP = -num2
						};
						combatant.AlterStats(statChange3);
					}
				}
				this.ChangeState(EnemyProjectileAction.State.WaitForUI);
				return;
			}
			if (this.state == EnemyProjectileAction.State.Finish)
			{
				this.complete = true;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000CF10 File Offset: 0x0000B110
		private void OnDamageNumberComplete(DamageNumber sender)
		{
			sender.OnComplete -= this.OnDamageNumberComplete;
			this.ChangeState(EnemyProjectileAction.State.Finish);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000CF2C File Offset: 0x0000B12C
		private void OnTextboxComplete()
		{
			EnemyProjectileAction.State state = this.previousState;
			if (state == EnemyProjectileAction.State.ShowReflectMessage)
			{
				this.ChangeState(EnemyProjectileAction.State.AnimateReflect);
				return;
			}
			this.ChangeState(EnemyProjectileAction.State.AnimateThrow);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000CF54 File Offset: 0x0000B154
		private void OnAnimationComplete(PsiAnimator anim)
		{
			anim.OnAnimationComplete -= this.OnAnimationComplete;
			switch (this.previousState)
			{
			case EnemyProjectileAction.State.AnimateThrow:
				this.ChangeState(this.isReflected ? EnemyProjectileAction.State.ShowReflectMessage : EnemyProjectileAction.State.AnimateHit);
				return;
			case EnemyProjectileAction.State.AnimateReflect:
				this.ChangeState(EnemyProjectileAction.State.AnimateHit);
				return;
			case EnemyProjectileAction.State.AnimateHit:
				this.ChangeState(EnemyProjectileAction.State.DamageNumbers);
				return;
			default:
				return;
			}
		}

		// Token: 0x040002F1 RID: 753
		private const float ONE_GP = 0.013333334f;

		// Token: 0x040002F2 RID: 754
		private const int ANIMINDEX_THROW = 4;

		// Token: 0x040002F3 RID: 755
		private const int ANIMINDEX_HITBACK = 5;

		// Token: 0x040002F4 RID: 756
		private const int ANIMINDEX_EXPLODE = 6;

		// Token: 0x040002F5 RID: 757
		private EnemyProjectileAction.State state;

		// Token: 0x040002F6 RID: 758
		private EnemyProjectileAction.State previousState;

		// Token: 0x040002F7 RID: 759
		private EnemyCombatant enemySender;

		// Token: 0x040002F8 RID: 760
		private PlayerCombatant playerTarget;

		// Token: 0x040002F9 RID: 761
		private string projectileName;

		// Token: 0x040002FA RID: 762
		private bool isReflected;

		// Token: 0x040002FB RID: 763
		private int targetDamage;

		// Token: 0x02000055 RID: 85
		private enum State
		{
			// Token: 0x040002FD RID: 765
			Initialize,
			// Token: 0x040002FE RID: 766
			WaitForUI,
			// Token: 0x040002FF RID: 767
			AnimateThrow,
			// Token: 0x04000300 RID: 768
			AnimateReflect,
			// Token: 0x04000301 RID: 769
			AnimateHit,
			// Token: 0x04000302 RID: 770
			DamageNumbers,
			// Token: 0x04000303 RID: 771
			ShowReflectMessage,
			// Token: 0x04000304 RID: 772
			Finish
		}
	}
}
