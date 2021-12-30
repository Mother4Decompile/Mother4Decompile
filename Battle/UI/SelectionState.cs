using System;
using Mother4.Battle.Combatants;
using Mother4.Data.Psi;

namespace Mother4.Battle.UI
{
	// Token: 0x020000E9 RID: 233
	internal struct SelectionState
	{
		// Token: 0x04000730 RID: 1840
		public SelectionState.SelectionType Type;

		// Token: 0x04000731 RID: 1841
		public Combatant[] Targets;

		// Token: 0x04000732 RID: 1842
		public TargetingMode TargetingMode;

		// Token: 0x04000733 RID: 1843
		public int AttackIndex;

		// Token: 0x04000734 RID: 1844
		public int ItemIndex;

		// Token: 0x04000735 RID: 1845
		public PsiLevel Psi;

		// Token: 0x020000EA RID: 234
		public enum SelectionType
		{
			// Token: 0x04000737 RID: 1847
			Bash,
			// Token: 0x04000738 RID: 1848
			PSI,
			// Token: 0x04000739 RID: 1849
			Talk,
			// Token: 0x0400073A RID: 1850
			Items,
			// Token: 0x0400073B RID: 1851
			Guard,
			// Token: 0x0400073C RID: 1852
			Run,
			// Token: 0x0400073D RID: 1853
			Undo
		}
	}
}
