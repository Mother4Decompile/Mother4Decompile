using System;
using System.Collections.Generic;

namespace Mother4.Battle.PsiAnimation
{
	// Token: 0x02000074 RID: 116
	internal class PsiElementList
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000FBDC File Offset: 0x0000DDDC
		public bool HasElements
		{
			get
			{
				return this.elementCounter < this.elements.Count;
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000FBF1 File Offset: 0x0000DDF1
		public PsiElementList(List<PsiElement> elements)
		{
			this.elements = new List<PsiElement>(elements);
			this.elements.Sort(new Comparison<PsiElement>(PsiElementList.CompareElements));
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		private static int CompareElements(PsiElement x, PsiElement y)
		{
			return y.Timestamp - x.Timestamp;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000FC48 File Offset: 0x0000DE48
		public List<PsiElement> GetElementsAtTime(int timestamp)
		{
			List<PsiElement> list = this.elements.FindAll((PsiElement x) => x.Timestamp == timestamp);
			this.elementCounter += list.Count;
			return list;
		}

		// Token: 0x040003E1 RID: 993
		private List<PsiElement> elements;

		// Token: 0x040003E2 RID: 994
		private int elementCounter;
	}
}
