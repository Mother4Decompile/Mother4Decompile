using System;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;

namespace Mother4.Battle.EnemyAI
{
	// Token: 0x02000067 RID: 103
	internal interface IEnemyAI
	{
		// Token: 0x0600024A RID: 586
		BattleAction GetAction(int priority, Combatant[] potentialTargets);
	}
}
