using System;
using System.Globalization;

namespace Mother4.Utility
{
	// Token: 0x02000171 RID: 369
	internal class Capitalizer
	{
		// Token: 0x060007C5 RID: 1989 RVA: 0x00032210 File Offset: 0x00030410
		public static string Capitalize(string word)
		{
			if (word != null && word.Length > 0)
			{
				string str = word.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture);
				string str2 = word.Substring(1);
				return str + str2;
			}
			return string.Empty;
		}
	}
}
