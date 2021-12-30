using System;
using Mother4.Battle.Combatants;
using Mother4.Battle.EnemyAI;
using Mother4.Data;

namespace Mother4.Battle.Actions
{
	// Token: 0x020000B6 RID: 182
	internal class EnemyDecisionAction : DecisionAction
	{
		// Token: 0x060003E8 RID: 1000 RVA: 0x00018BF4 File Offset: 0x00016DF4
		public EnemyDecisionAction(ActionParams aparams) : base(aparams)
		{
			this.enemyType = (this.sender as EnemyCombatant).Enemy;
			if (this.enemyType == EnemyType.ModernMind)
			{
				this.aicontrol = new TravisMustDieAI(this.controller, this.sender);
				return;
			}
			this.aicontrol = new RandomAI(this.controller, this.sender);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00018C60 File Offset: 0x00016E60
		protected override void UpdateAction()
		{
			base.UpdateAction();
			bool flag = false;
			StatusEffectInstance[] statusEffects = this.sender.GetStatusEffects();
			if (statusEffects.Length > 0)
			{
				foreach (StatusEffectInstance statusEffectInstance in statusEffects)
				{
					Type type = StatusEffectActions.Get(statusEffectInstance.Type);
					if (type != null)
					{
						ActionParams aparams = new ActionParams
						{
							actionType = type,
							controller = this.controller,
							sender = this.sender,
							priority = this.sender.Stats.Speed,
							targets = this.sender.SavedTargets,
							data = new object[]
							{
								statusEffectInstance
							}
						};
						this.controller.AddAction(BattleAction.GetInstance(aparams));
						flag |= (statusEffectInstance.Type == StatusEffect.Talking);
					}
				}
			}
			if (!flag)
			{
				Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
				BattleAction action = this.aicontrol.GetAction(this.sender.Stats.Speed, factionCombatants);
				this.controller.AddAction(action);
			}
			this.complete = true;
		}

		// Token: 0x040005A8 RID: 1448
		private EnemyType enemyType;

		// Token: 0x040005A9 RID: 1449
		private IEnemyAI aicontrol;
	}
}
