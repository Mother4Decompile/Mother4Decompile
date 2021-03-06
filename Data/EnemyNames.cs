using System;
using Mother4.Data.Enemies;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;

namespace Mother4.Data
{
	// Token: 0x020000F0 RID: 240
	internal class EnemyNames
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x000216D4 File Offset: 0x0001F8D4
		private static string GetStringOrDefault(EnemyData enemyData, string stringType, string defaultValue)
		{
			string qualifiedName;
			string result;
			if (enemyData.TryGetStringQualifiedName(stringType, out qualifiedName))
			{
				RufiniString rufiniString = StringFile.Instance.Get(qualifiedName);
				result = ((rufiniString.Value != null) ? rufiniString.Value : defaultValue);
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00021714 File Offset: 0x0001F914
		public static string GetName(EnemyType enemy)
		{
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			return EnemyNames.GetStringOrDefault(data, "name", "DUMMY");
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00021740 File Offset: 0x0001F940
		public static string GetArticle(EnemyType enemy)
		{
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			return EnemyNames.GetStringOrDefault(data, "article", "THE");
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0002176C File Offset: 0x0001F96C
		public static string GetSubjectivePronoun(EnemyType enemy)
		{
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			return EnemyNames.GetStringOrDefault(data, "subjective", "IT");
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00021798 File Offset: 0x0001F998
		public static string GetPosessivePronoun(EnemyType enemy)
		{
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			return EnemyNames.GetStringOrDefault(data, "possessive", "ITS");
		}

		// Token: 0x04000754 RID: 1876
		private const string STRING_NAME = "name";

		// Token: 0x04000755 RID: 1877
		private const string STRING_ARTICLE = "article";

		// Token: 0x04000756 RID: 1878
		private const string STRING_POSSESSIVE = "possessive";

		// Token: 0x04000757 RID: 1879
		private const string STRING_SUBJECTIVE = "subjective";
	}
}
