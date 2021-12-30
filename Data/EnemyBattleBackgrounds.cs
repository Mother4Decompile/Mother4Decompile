using System;
using Mother4.Data.Enemies;

namespace Mother4.Data
{
	// Token: 0x02000082 RID: 130
	internal class EnemyBattleBackgrounds
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0001112C File Offset: 0x0000F32C
		public static string GetFile(EnemyType enemy)
		{
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			return Paths.GRAPHICS + "BBG/xml/" + data.BackgroundName + ".xml";
		}
	}
}
