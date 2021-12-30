using System;
using Mother4.Data.Enemies;

namespace Mother4.Data
{
	// Token: 0x020000EF RID: 239
	internal class EnemyGraphics
	{
		// Token: 0x06000580 RID: 1408 RVA: 0x0002169C File Offset: 0x0001F89C
		public static string GetFilename(EnemyType enemy)
		{
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			return Paths.GRAPHICS + data.SpriteName + ".dat";
		}
	}
}
