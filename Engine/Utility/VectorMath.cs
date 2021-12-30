using System;
using SFML.System;

namespace Carbine.Utility
{
	// Token: 0x02000068 RID: 104
	public static class VectorMath
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x0000EE6B File Offset: 0x0000D06B
		public static Vector2f DirectionToVector(int direction)
		{
			return VectorMath.DIR_TO_VECTOR[direction % VectorMath.DIR_TO_VECTOR.Length];
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000EE88 File Offset: 0x0000D088
		public static int VectorToDirection(Vector2f v)
		{
			double num = Math.Atan2((double)(-(double)v.Y), (double)v.X) + 0.39269908169872414;
			int num2 = (int)Math.Floor(num / 0.7853981633974483);
			if (num2 < 0)
			{
				num2 += 8;
			}
			return num2;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000EED1 File Offset: 0x0000D0D1
		public static float Magnitude(Vector2f v)
		{
			return (float)Math.Sqrt((double)(v.X * v.X + v.Y * v.Y));
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000EEFC File Offset: 0x0000D0FC
		public static Vector2f Normalize(Vector2f v)
		{
			float num = VectorMath.Magnitude(v);
			Vector2f zero_VECTOR;
			if (num > 0f)
			{
				float x = v.X / num;
				float y = v.Y / num;
				zero_VECTOR = new Vector2f(x, y);
			}
			else
			{
				zero_VECTOR = VectorMath.ZERO_VECTOR;
			}
			return zero_VECTOR;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000EF3E File Offset: 0x0000D13E
		public static Vector2f LeftNormal(Vector2f v)
		{
			return new Vector2f(v.Y, -v.X);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000EF54 File Offset: 0x0000D154
		public static Vector2f RightNormal(Vector2f v)
		{
			return new Vector2f(-v.Y, v.X);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EF6A File Offset: 0x0000D16A
		public static float DotProduct(Vector2f a, Vector2f b)
		{
			return a.X * b.X + a.Y * b.Y;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000EF8C File Offset: 0x0000D18C
		public static Vector2f Truncate(Vector2f v)
		{
			int num = (int)v.X;
			int num2 = (int)v.Y;
			return new Vector2f((float)num, (float)num2);
		}

		// Token: 0x0400020F RID: 527
		public const double PI_OVER_FOUR = 0.7853981633974483;

		// Token: 0x04000210 RID: 528
		public const double PI_OVER_EIGHT = 0.39269908169872414;

		// Token: 0x04000211 RID: 529
		public static readonly Vector2f ZERO_VECTOR = new Vector2f(0f, 0f);

		// Token: 0x04000212 RID: 530
		private static Vector2f[] DIR_TO_VECTOR = new Vector2f[]
		{
			new Vector2f(1f, 0f),
			new Vector2f(1f, -1f),
			new Vector2f(0f, -1f),
			new Vector2f(-1f, -1f),
			new Vector2f(-1f, 0f),
			new Vector2f(-1f, 1f),
			new Vector2f(0f, 1f),
			new Vector2f(1f, 1f)
		};
	}
}
