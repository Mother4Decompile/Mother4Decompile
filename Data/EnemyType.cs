using System;
using Carbine.Utility;

namespace Mother4.Data
{
	// Token: 0x020000EE RID: 238
	internal struct EnemyType
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00021509 File Offset: 0x0001F709
		public int Identifier
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00021511 File Offset: 0x0001F711
		public EnemyType(int id)
		{
			this.id = id;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0002151A File Offset: 0x0001F71A
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00021522 File Offset: 0x0001F722
		public override bool Equals(object obj)
		{
			return obj is EnemyType && this.GetHashCode() == obj.GetHashCode();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00021542 File Offset: 0x0001F742
		public static bool operator ==(EnemyType a, EnemyType b)
		{
			return a.id == b.id;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00021554 File Offset: 0x0001F754
		public static bool operator !=(EnemyType a, EnemyType b)
		{
			return !(a == b);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00021560 File Offset: 0x0001F760
		public static explicit operator int(EnemyType type)
		{
			return type.id;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00021569 File Offset: 0x0001F769
		public static explicit operator EnemyType(int id)
		{
			return new EnemyType(id);
		}

		// Token: 0x04000745 RID: 1861
		public static EnemyType Dummy = new EnemyType();

		// Token: 0x04000746 RID: 1862
		public static EnemyType MagicSnail = new EnemyType(Hash.Get("test.MagicSnail"));

		// Token: 0x04000747 RID: 1863
		public static EnemyType Stickat = new EnemyType(Hash.Get("test.Stickat"));

		// Token: 0x04000748 RID: 1864
		public static EnemyType Rat = new EnemyType(Hash.Get("test.Rat"));

		// Token: 0x04000749 RID: 1865
		public static EnemyType HermitCan = new EnemyType(Hash.Get("test.HermitCan"));

		// Token: 0x0400074A RID: 1866
		public static EnemyType Flamingo = new EnemyType(Hash.Get("test.Flamingo"));

		// Token: 0x0400074B RID: 1867
		public static EnemyType AtomicPowerRobo = new EnemyType(Hash.Get("test.AtomicPowerRobo"));

		// Token: 0x0400074C RID: 1868
		public static EnemyType CarbonPup = new EnemyType(Hash.Get("test.CarbonPup"));

		// Token: 0x0400074D RID: 1869
		public static EnemyType MeltyRobot = new EnemyType(Hash.Get("test.MeltyRobot"));

		// Token: 0x0400074E RID: 1870
		public static EnemyType ModernMind = new EnemyType(Hash.Get("test.ModernMind"));

		// Token: 0x0400074F RID: 1871
		public static EnemyType NotSoDeer = new EnemyType(Hash.Get("test.NotSoDeer"));

		// Token: 0x04000750 RID: 1872
		public static EnemyType PunkAssassin = new EnemyType(Hash.Get("test.PunkAssassin"));

		// Token: 0x04000751 RID: 1873
		public static EnemyType PunkEnforcer = new EnemyType(Hash.Get("test.PunkEnforcer"));

		// Token: 0x04000752 RID: 1874
		public static EnemyType RatDispenser = new EnemyType(Hash.Get("test.RatDispenser"));

		// Token: 0x04000753 RID: 1875
		private int id;
	}
}
