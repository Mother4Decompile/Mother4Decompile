using System;
using System.Collections.Generic;

namespace Mother4.Battle.Combos
{
	// Token: 0x020000D3 RID: 211
	internal class ComboSet
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0001E89C File Offset: 0x0001CA9C
		public IList<ComboNode> ComboNodes
		{
			get
			{
				return this.combos;
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001E8A4 File Offset: 0x0001CAA4
		public ComboSet(IList<ComboNode> nodes)
		{
			this.combos = new List<ComboNode>(nodes);
			this.combos.Sort();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001E8C3 File Offset: 0x0001CAC3
		public ComboNode GetFirstCombo()
		{
			if (this.combos.Count <= 0)
			{
				return null;
			}
			return this.combos[0];
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001E8E4 File Offset: 0x0001CAE4
		public ComboNode GetNextCombo(ComboNode node)
		{
			if (node != null)
			{
				ComboNode comboNode = null;
				foreach (ComboNode comboNode2 in this.combos)
				{
					if (comboNode2.Timestamp > node.Timestamp)
					{
						comboNode = comboNode2;
						break;
					}
				}
				if (comboNode == null)
				{
					comboNode = this.GetFirstCombo();
				}
				return comboNode;
			}
			return this.GetFirstCombo();
		}

		// Token: 0x04000683 RID: 1667
		private List<ComboNode> combos;
	}
}
