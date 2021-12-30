using System;
using Mother4.Data.Enemies;
using Rufini.Strings;

namespace Mother4.Data
{
	// Token: 0x02000077 RID: 119
	internal class EnemyDeathText
	{
		// Token: 0x06000288 RID: 648 RVA: 0x0000FF0C File Offset: 0x0000E10C
		public static string Get(EnemyType enemy)
		{
			string result = string.Empty;
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			string qualifiedName;
			if (data.TryGetStringQualifiedName("defeat", out qualifiedName))
			{
				result = StringFile.Instance.Get(qualifiedName).Value;
			}
			return result;
		}
	}
}
