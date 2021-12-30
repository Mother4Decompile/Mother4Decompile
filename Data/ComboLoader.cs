using System;
using System.Collections.Generic;
using Mother4.Battle.Combos;

namespace Mother4.Data
{
	// Token: 0x020000F1 RID: 241
	internal class ComboLoader
	{
		// Token: 0x06000588 RID: 1416 RVA: 0x000217CC File Offset: 0x0001F9CC
		public static ComboSet Load(string resource)
		{
			return new ComboSet(new List<ComboNode>
			{
				new ComboNode(ComboType.Point, 0U, 1U),
				new ComboNode(ComboType.Point, 375U, 1U),
				new ComboNode(ComboType.Point, 750U, 1U),
				new ComboNode(ComboType.Point, 1125U, 1U),
				new ComboNode(ComboType.Point, 1500U, 1U),
				new ComboNode(ComboType.Point, 1875U, 1U),
				new ComboNode(ComboType.BPMRange, 2500U, 97500U, 120f)
			});
		}
	}
}
