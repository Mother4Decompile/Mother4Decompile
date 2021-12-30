using System;
using Carbine.Audio;
using Mother4.Battle.Combatants;
using Mother4.Data;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	// Token: 0x0200001D RID: 29
	internal class BuffCancelAction : BattleAction
	{
		// Token: 0x06000061 RID: 97 RVA: 0x00004734 File Offset: 0x00002934
		public BuffCancelAction(ActionParams aparams) : base(aparams)
		{
			if (this.sender is PlayerCombatant)
			{
				this.senderName = CharacterNames.GetName((this.sender as PlayerCombatant).Character);
			}
			else if (this.sender is EnemyCombatant)
			{
				this.senderName = EnemyNames.GetName((this.sender as EnemyCombatant).Enemy);
			}
			else
			{
				this.senderName = string.Empty;
			}
			if (this.targets != null && this.targets[0] is PlayerCombatant)
			{
				this.targetName = CharacterNames.GetName((this.targets[0] as PlayerCombatant).Character);
			}
			else if (this.targets != null && this.targets[0] is EnemyCombatant)
			{
				this.targetName = EnemyNames.GetName((this.targets[0] as EnemyCombatant).Enemy);
			}
			else
			{
				this.targetName = string.Empty;
			}
			if (this.sender is EnemyCombatant)
			{
				this.senderArticle = EnemyNames.GetArticle((this.sender as EnemyCombatant).Enemy);
			}
			else
			{
				this.senderArticle = string.Empty;
			}
			if (this.targets != null && this.targets[0] is EnemyCombatant)
			{
				this.targetArticle = EnemyNames.GetArticle((this.targets[0] as EnemyCombatant).Enemy);
			}
			else
			{
				this.targetArticle = string.Empty;
			}
			if (this.sender is PlayerCombatant)
			{
				this.actionStartSound = this.controller.InterfaceController.PrePlayerAttack;
			}
			else if (this.sender is EnemyCombatant)
			{
				this.actionStartSound = this.controller.InterfaceController.PreEnemyAttack;
			}
			this.effect = (StatusEffectInstance)aparams.data[0];
			this.message = string.Format("{0} removed a {1} effect on {3}!!", string.Format("{0} {1}", this.senderArticle, this.senderName).Trim(), Enum.GetName(typeof(StatusEffect), this.effect.Type), string.Format("{0} {1}", this.targetArticle, this.targetName).Trim()).Trim();
			this.state = BuffCancelAction.State.Initialize;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004964 File Offset: 0x00002B64
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case BuffCancelAction.State.Initialize:
				this.Initialize();
				return;
			case BuffCancelAction.State.WaitForUI:
				this.WaitForUI();
				return;
			case BuffCancelAction.State.Finish:
				this.Finish();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000049A8 File Offset: 0x00002BA8
		protected virtual void Initialize()
		{
			if (this.message == null)
			{
				throw new InvalidOperationException("BuffCancelAction message is null.");
			}
			for (int i = 0; i < BuffCancelAction.STATUS_EFFECT_TYPES.Length; i++)
			{
				this.sender.RemoveStatusEffect(BuffCancelAction.STATUS_EFFECT_TYPES[i]);
			}
			this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
			this.controller.InterfaceController.ShowTextBox(this.message, false);
			this.actionStartSound.Play();
			if (this.sender is PlayerCombatant)
			{
				this.controller.InterfaceController.PopCard(this.sender.ID, 12);
			}
			else if (this.sender is EnemyCombatant)
			{
				this.controller.InterfaceController.FlashEnemy(this.sender as EnemyCombatant, Color.Black, 8, 2);
			}
			this.sender.DecrementStatusEffect(this.effect.Type);
			this.state = BuffCancelAction.State.WaitForUI;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004AA4 File Offset: 0x00002CA4
		protected virtual void WaitForUI()
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004AA8 File Offset: 0x00002CA8
		protected virtual void Finish()
		{
			this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
			if (this.sender is PlayerCombatant)
			{
				this.controller.InterfaceController.PopCard(this.sender.ID, 0);
			}
			this.complete = true;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004B02 File Offset: 0x00002D02
		protected virtual void InteractionComplete()
		{
			this.state = BuffCancelAction.State.Finish;
		}

		// Token: 0x0400011B RID: 283
		private const int EFFECT_TYPE_INDEX = 0;

		// Token: 0x0400011C RID: 284
		private const int CARD_POP_HEIGHT = 12;

		// Token: 0x0400011D RID: 285
		private const bool USE_BUTTON = false;

		// Token: 0x0400011E RID: 286
		private const int BLINK_DURATION = 8;

		// Token: 0x0400011F RID: 287
		private const int BLINK_COUNT = 2;

		// Token: 0x04000120 RID: 288
		private static readonly StatusEffect[] STATUS_EFFECT_TYPES = new StatusEffect[]
		{
			StatusEffect.Shield,
			StatusEffect.PsiShield,
			StatusEffect.OffenseUp,
			StatusEffect.DefenseUp,
			StatusEffect.QuickUp
		};

		// Token: 0x04000121 RID: 289
		private BuffCancelAction.State state;

		// Token: 0x04000122 RID: 290
		protected string message;

		// Token: 0x04000123 RID: 291
		protected string senderName;

		// Token: 0x04000124 RID: 292
		protected string targetName;

		// Token: 0x04000125 RID: 293
		protected string senderArticle;

		// Token: 0x04000126 RID: 294
		protected string targetArticle;

		// Token: 0x04000127 RID: 295
		protected CarbineSound actionStartSound;

		// Token: 0x04000128 RID: 296
		protected readonly StatusEffectInstance effect;

		// Token: 0x0200001E RID: 30
		private enum State
		{
			// Token: 0x0400012A RID: 298
			Initialize,
			// Token: 0x0400012B RID: 299
			WaitForUI,
			// Token: 0x0400012C RID: 300
			Finish
		}
	}
}
