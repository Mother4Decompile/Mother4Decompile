using System;
using System.Collections.Generic;
using Carbine;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle.EnemyAI
{
	// Token: 0x02000069 RID: 105
	internal class TravisMustDieAI : IEnemyAI
	{
		// Token: 0x0600024E RID: 590 RVA: 0x0000E602 File Offset: 0x0000C802
		public TravisMustDieAI(BattleController controller, Combatant sender)
		{
			this.controller = controller;
			this.sender = sender;
			this.battleActionParams = EnemyBattleActions.GetBattleActionParams((sender as EnemyCombatant).Enemy);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000E630 File Offset: 0x0000C830
		public BattleAction GetAction(int priority, Combatant[] potentialTargets)
		{
			Combatant combatant = null;
			foreach (Combatant combatant2 in potentialTargets)
			{
				if (combatant2.Faction == BattleFaction.PlayerTeam)
				{
					PlayerCombatant playerCombatant = combatant2 as PlayerCombatant;
					if (playerCombatant.Character == CharacterType.Travis)
					{
						combatant = playerCombatant;
						break;
					}
				}
			}
			Combatant[] targets = new Combatant[]
			{
				(combatant != null) ? combatant : potentialTargets[Engine.Random.Next(potentialTargets.Length)]
			};
			ActionParams aparams = this.battleActionParams[Engine.Random.Next(this.battleActionParams.Count)];
			aparams.controller = this.controller;
			aparams.sender = this.sender;
			aparams.priority = this.sender.Stats.Speed;
			aparams.targets = targets;
			return BattleAction.GetInstance(aparams);
		}

		// Token: 0x04000360 RID: 864
		private List<ActionParams> battleActionParams;

		// Token: 0x04000361 RID: 865
		private BattleController controller;

		// Token: 0x04000362 RID: 866
		private Combatant sender;
	}
}
