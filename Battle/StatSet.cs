using System;

namespace Mother4.Battle
{
	// Token: 0x020000D5 RID: 213
	internal struct StatSet
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x0001E95C File Offset: 0x0001CB5C
		public StatSet(StatSet set)
		{
			this.HP = set.HP;
			this.PP = set.PP;
			this.Defense = set.Defense;
			this.Guts = set.Guts;
			this.IQ = set.IQ;
			this.Luck = set.Luck;
			this.Offense = set.Offense;
			this.Speed = set.Speed;
			this.MaxHP = set.MaxHP;
			this.MaxPP = set.MaxPP;
			this.Meter = set.Meter;
			this.Experience = set.Experience;
			this.Level = set.Level;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001EA14 File Offset: 0x0001CC14
		public static StatSet operator +(StatSet set1, StatSet set2)
		{
			StatSet result = new StatSet(set1);
			result.Defense += set2.Defense;
			result.Guts += set2.Guts;
			result.HP += set2.HP;
			result.IQ += set2.IQ;
			result.Luck += set2.Luck;
			result.Offense += set2.Offense;
			result.PP += set2.PP;
			result.Speed += set2.Speed;
			result.MaxHP += set2.MaxHP;
			result.MaxPP += set2.MaxPP;
			result.Meter += set2.Meter;
			result.Level += set2.Level;
			result.Experience += set2.Experience;
			return result;
		}

		// Token: 0x04000687 RID: 1671
		public int Level;

		// Token: 0x04000688 RID: 1672
		public int Experience;

		// Token: 0x04000689 RID: 1673
		public int HP;

		// Token: 0x0400068A RID: 1674
		public int MaxHP;

		// Token: 0x0400068B RID: 1675
		public int PP;

		// Token: 0x0400068C RID: 1676
		public int MaxPP;

		// Token: 0x0400068D RID: 1677
		public int Speed;

		// Token: 0x0400068E RID: 1678
		public int Defense;

		// Token: 0x0400068F RID: 1679
		public int Offense;

		// Token: 0x04000690 RID: 1680
		public int IQ;

		// Token: 0x04000691 RID: 1681
		public int Guts;

		// Token: 0x04000692 RID: 1682
		public int Luck;

		// Token: 0x04000693 RID: 1683
		public float Meter;
	}
}
