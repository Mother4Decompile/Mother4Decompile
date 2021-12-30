using System;
using Mother4.Battle.Combatants;

namespace Mother4.Battle.Actions
{
	// Token: 0x02000053 RID: 83
	internal struct ActionParams
	{
		// Token: 0x040002EA RID: 746
		public Type actionType;

		// Token: 0x040002EB RID: 747
		public BattleController controller;

		// Token: 0x040002EC RID: 748
		public Combatant sender;

		// Token: 0x040002ED RID: 749
		public Combatant[] targets;

		// Token: 0x040002EE RID: 750
		public int priority;

		// Token: 0x040002EF RID: 751
		public object[] data;

		// Token: 0x040002F0 RID: 752
		public int weight;
	}
}
