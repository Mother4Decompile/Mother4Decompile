using System;

namespace Carbine.Utility
{
	// Token: 0x02000062 RID: 98
	public static class Hash
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x0000EB68 File Offset: 0x0000CD68
		public static int Get(string input)
		{
			int num = 23;
			for (int i = 0; i < input.Length; i++)
			{
				num = num * 31 + (int)input[i];
			}
			return num;
		}
	}
}
