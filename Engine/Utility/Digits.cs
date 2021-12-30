using System;

namespace Carbine.Utility
{
	// Token: 0x0200005F RID: 95
	public class Digits
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000EA69 File Offset: 0x0000CC69
		public static int Get(int number, int place)
		{
			return Math.Abs(number) / (int)Math.Pow(10.0, (double)(place - 1)) % 10;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000EA88 File Offset: 0x0000CC88
		public static int CountDigits(int number)
		{
			int result = 1;
			if (number != 0)
			{
				result = (int)(Math.Log10((double)Math.Abs(number)) + 1.0);
			}
			return result;
		}
	}
}
