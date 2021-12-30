using System;
using Carbine.Audio;
using Mother4.Battle.Combatants;
using Mother4.Data;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	// Token: 0x02000014 RID: 20
	internal class StatusEffectAction : BattleAction
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000037C4 File Offset: 0x000019C4
		public StatusEffectAction(ActionParams aparams) : base(aparams)
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
			this.state = StatusEffectAction.State.Initialize;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003988 File Offset: 0x00001B88
		protected string BuildCombatantName(Combatant combatant)
		{
			string arg;
			string arg2;
			if (combatant is PlayerCombatant)
			{
				arg = string.Empty;
				arg2 = CharacterNames.GetName((combatant as PlayerCombatant).Character);
			}
			else if (combatant is EnemyCombatant)
			{
				arg = EnemyNames.GetArticle((combatant as EnemyCombatant).Enemy);
				arg2 = EnemyNames.GetName((combatant as EnemyCombatant).Enemy);
			}
			else
			{
				arg = string.Empty;
				arg2 = string.Empty;
			}
			return string.Format("{0} {1}", arg, arg2).Trim();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003A00 File Offset: 0x00001C00
		protected override void UpdateAction()
		{
			switch (this.state)
			{
			case StatusEffectAction.State.Initialize:
				this.Initialize();
				return;
			case StatusEffectAction.State.WaitForUI:
				this.WaitForUI();
				return;
			case StatusEffectAction.State.Finish:
				this.Finish();
				return;
			default:
				return;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003A3C File Offset: 0x00001C3C
		protected virtual void Initialize()
		{
			if (this.message == null)
			{
				throw new InvalidOperationException("StatusEffectAction message is null.");
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
			this.state = StatusEffectAction.State.WaitForUI;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003B13 File Offset: 0x00001D13
		protected virtual void WaitForUI()
		{
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003B18 File Offset: 0x00001D18
		protected virtual void Finish()
		{
			this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
			if (this.sender is PlayerCombatant)
			{
				this.controller.InterfaceController.PopCard(this.sender.ID, 0);
			}
			this.complete = true;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003B72 File Offset: 0x00001D72
		protected virtual void InteractionComplete()
		{
			this.state = StatusEffectAction.State.Finish;
		}

		// Token: 0x040000DB RID: 219
		private const int EFFECT_TYPE_INDEX = 0;

		// Token: 0x040000DC RID: 220
		private const int CARD_POP_HEIGHT = 12;

		// Token: 0x040000DD RID: 221
		private const bool USE_BUTTON = false;

		// Token: 0x040000DE RID: 222
		private const int BLINK_DURATION = 8;

		// Token: 0x040000DF RID: 223
		private const int BLINK_COUNT = 2;

		// Token: 0x040000E0 RID: 224
		private StatusEffectAction.State state;

		// Token: 0x040000E1 RID: 225
		protected string message;

		// Token: 0x040000E2 RID: 226
		protected string senderName;

		// Token: 0x040000E3 RID: 227
		protected string targetName;

		// Token: 0x040000E4 RID: 228
		protected string senderArticle;

		// Token: 0x040000E5 RID: 229
		protected string targetArticle;

		// Token: 0x040000E6 RID: 230
		protected CarbineSound actionStartSound;

		// Token: 0x040000E7 RID: 231
		protected StatusEffectInstance effect;

		// Token: 0x02000015 RID: 21
		private enum State
		{
			// Token: 0x040000E9 RID: 233
			Initialize,
			// Token: 0x040000EA RID: 234
			WaitForUI,
			// Token: 0x040000EB RID: 235
			Finish
		}
	}
}
