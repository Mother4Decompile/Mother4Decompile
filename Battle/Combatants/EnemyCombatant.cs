using System;
using Mother4.Battle.Actions;
using Mother4.Data;

namespace Mother4.Battle.Combatants
{
	// Token: 0x020000CF RID: 207
	internal class EnemyCombatant : Combatant
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0001E4C3 File Offset: 0x0001C6C3
		public EnemyType Enemy
		{
			get
			{
				return this.enemy;
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001E4CB File Offset: 0x0001C6CB
		public EnemyCombatant(EnemyType enemy) : base(BattleFaction.EnemyTeam)
		{
			this.enemy = enemy;
			this.stats = EnemyStats.GetStats(enemy);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001E4E8 File Offset: 0x0001C6E8
		public override DecisionAction GetDecisionAction(BattleController controller, int priority, bool isFromUndo)
		{
			return new EnemyDecisionAction(new ActionParams
			{
				actionType = typeof(EnemyDecisionAction),
				controller = controller,
				sender = this,
				priority = priority
			});
		}

		// Token: 0x04000677 RID: 1655
		private EnemyType enemy;
	}
}
