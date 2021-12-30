using System;

namespace Carbine.Utility
{
	// Token: 0x02000061 RID: 97
	public class GaussianRandom
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000EB10 File Offset: 0x0000CD10
		public static double Next(double mean, double stdDev)
		{
			double d = Engine.Random.NextDouble();
			double num = Engine.Random.NextDouble();
			double num2 = Math.Sqrt(-2.0 * Math.Log(d)) * Math.Sin(6.283185307179586 * num);
			return mean + stdDev * num2;
		}
	}
}
