using System;
using Mother4.Battle;
using Mother4.Data.Enemies;

namespace Mother4.Data
{
	// Token: 0x02000084 RID: 132
	internal class EnemyStats
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x0001129C File Offset: 0x0000F49C
		public static StatSet GetStats(EnemyType enemy)
		{
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			return data.GetStatSet();
		}
	}
}
