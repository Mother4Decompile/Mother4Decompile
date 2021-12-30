using System;
using System.Collections.Generic;
using Rufini.Strings;

namespace Mother4.Data
{
	// Token: 0x0200008A RID: 138
	internal class PsiLetters
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x0001240C File Offset: 0x0001060C
		public static string Get(int level)
		{
			if (PsiLetters.LETTER_MAP == null)
			{
				PsiLetters.LETTER_MAP = new Dictionary<int, string>
				{
					{
						0,
						StringFile.Instance.Get("psi.symbols.alpha").Value
					},
					{
						1,
						StringFile.Instance.Get("psi.symbols.beta").Value
					},
					{
						2,
						StringFile.Instance.Get("psi.symbols.gamma").Value
					},
					{
						3,
						StringFile.Instance.Get("psi.symbols.omega").Value
					},
					{
						4,
						StringFile.Instance.Get("psi.symbols.sigma").Value
					},
					{
						5,
						StringFile.Instance.Get("psi.symbols.lambda").Value
					},
					{
						6,
						StringFile.Instance.Get("psi.symbols.xx").Value
					}
				};
			}
			string text = null;
			PsiLetters.LETTER_MAP.TryGetValue(level, out text);
			return text ?? "?";
		}

		// Token: 0x0400042D RID: 1069
		private static Dictionary<int, string> LETTER_MAP;
	}
}
