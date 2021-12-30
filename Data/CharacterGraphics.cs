using System;
using System.Collections.Generic;

namespace Mother4.Data
{
	// Token: 0x020000EC RID: 236
	internal static class CharacterGraphics
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x000210F7 File Offset: 0x0001F2F7
		public static string GetFile(CharacterType character)
		{
			return CharacterGraphics.GetFile(character, true);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00021100 File Offset: 0x0001F300
		public static string GetFile(CharacterType character, bool fullPath)
		{
			if (!CharacterGraphics.graphics.ContainsKey(character))
			{
				return "";
			}
			if (!fullPath)
			{
				return CharacterGraphics.graphics[character];
			}
			return Paths.GRAPHICS + CharacterGraphics.graphics[character] + CharacterGraphics.EXTENSION;
		}

		// Token: 0x0400073F RID: 1855
		private static string EXTENSION = ".dat";

		// Token: 0x04000740 RID: 1856
		private static Dictionary<CharacterType, string> graphics = new Dictionary<CharacterType, string>
		{
			{
				CharacterType.Travis,
				"travis"
			},
			{
				CharacterType.Meryl,
				"meryl"
			},
			{
				CharacterType.Floyd,
				"floyd"
			},
			{
				CharacterType.Leo,
				"leo"
			},
			{
				CharacterType.Zack,
				"zack"
			},
			{
				CharacterType.Renee,
				"sensitivityinred"
			},
			{
				CharacterType.Dog,
				"mutt"
			}
		};
	}
}
