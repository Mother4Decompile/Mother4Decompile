using System;
using System.Collections.Generic;
using Carbine;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle.EnemyAI
{
	// Token: 0x02000068 RID: 104
	internal class RandomAI : IEnemyAI
	{
		// Token: 0x0600024B RID: 587 RVA: 0x0000E4A6 File Offset: 0x0000C6A6
		public RandomAI(BattleController controller, Combatant sender)
		{
			this.controller = controller;
			this.sender = sender;
			this.battleActionParams = EnemyBattleActions.GetBattleActionParams((sender as EnemyCombatant).Enemy);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000E4D4 File Offset: 0x0000C6D4
		private ActionParams PickAction()
		{
			int num = 0;
			for (int i = 0; i < this.battleActionParams.Count; i++)
			{
				num += this.battleActionParams[i].weight;
			}
			int num2 = Engine.Random.Next(num);
			ActionParams? actionParams = null;
			int num3 = 0;
			for (int j = 0; j < this.battleActionParams.Count; j++)
			{
				num3 += this.battleActionParams[j].weight;
				if (num2 < num3)
				{
					actionParams = new ActionParams?(this.battleActionParams[j]);
					break;
				}
			}
			if (actionParams == null)
			{
				actionParams = new ActionParams?(this.battleActionParams[0]);
			}
			return actionParams.Value;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000E594 File Offset: 0x0000C794
		public BattleAction GetAction(int priority, Combatant[] potentialTargets)
		{
			Combatant[] targets = new Combatant[]
			{
				potentialTargets[Engine.Random.Next(potentialTargets.Length)]
			};
			ActionParams aparams = this.PickAction();
			aparams.controller = this.controller;
			aparams.sender = this.sender;
			aparams.priority = this.sender.Stats.Speed;
			aparams.targets = targets;
			return BattleAction.GetInstance(aparams);
		}

		// Token: 0x0400035D RID: 861
		private List<ActionParams> battleActionParams;

		// Token: 0x0400035E RID: 862
		private BattleController controller;

		// Token: 0x0400035F RID: 863
		private Combatant sender;
	}
}
