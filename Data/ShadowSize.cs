using System;
using SFML.System;

namespace Mother4.Data
{
	// Token: 0x02000036 RID: 54
	internal static class ShadowSize
	{
		// Token: 0x0600010B RID: 267 RVA: 0x00006E34 File Offset: 0x00005034
		public static string GetSubsprite(Vector2f size)
		{
			return ShadowSize.GetSubsprite((int)size.X);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006E43 File Offset: 0x00005043
		public static string GetSubsprite(float width)
		{
			return ShadowSize.GetSubsprite((int)width);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006E4C File Offset: 0x0000504C
		public static string GetSubsprite(int width)
		{
			string result;
			if (width <= 10)
			{
				result = "small";
			}
			else if (width <= 36)
			{
				result = "medium";
			}
			else if (width <= 64)
			{
				result = "large";
			}
			else
			{
				result = "huge";
			}
			return result;
		}

		// Token: 0x040001F4 RID: 500
		private const string SMALL = "small";

		// Token: 0x040001F5 RID: 501
		private const string MEDIUM = "medium";

		// Token: 0x040001F6 RID: 502
		private const string LARGE = "large";

		// Token: 0x040001F7 RID: 503
		private const string HUGE = "huge";

		// Token: 0x040001F8 RID: 504
		private const int WIDTH_SMALL = 10;

		// Token: 0x040001F9 RID: 505
		private const int WIDTH_MEDIUM = 36;

		// Token: 0x040001FA RID: 506
		private const int WIDTH_LARGE = 64;
	}
}
