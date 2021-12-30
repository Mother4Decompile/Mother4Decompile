using System;
using System.Collections.Generic;

namespace Mother4.Items
{
	// Token: 0x020000F6 RID: 246
	internal class Inventory
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x000221B5 File Offset: 0x000203B5
		public int MaxItems
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x000221BD File Offset: 0x000203BD
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x000221CA File Offset: 0x000203CA
		public bool Full
		{
			get
			{
				return this.Count >= this.MaxItems;
			}
		}

		// Token: 0x170000EA RID: 234
		public Item this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000221E6 File Offset: 0x000203E6
		public Inventory(int max)
		{
			this.max = max;
			this.items = new List<Item>(max);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00022201 File Offset: 0x00020401
		public void Add(Item item)
		{
			this.items.Add(item);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0002220F File Offset: 0x0002040F
		public void Remove(Item item)
		{
			this.items.Remove(item);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00022220 File Offset: 0x00020420
		public void Remove(Item item, int count)
		{
			int num = 0;
			foreach (Item item2 in this.items)
			{
				if (item2 == item)
				{
					this.items.Remove(item2);
					num++;
					if (num > count)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0002229C File Offset: 0x0002049C
		public void RemoveAll(Item item)
		{
			this.items.RemoveAll((Item x) => x == item);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x000222CE File Offset: 0x000204CE
		public void RemoveAt(int index)
		{
			this.items.RemoveAt(index);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x000222DC File Offset: 0x000204DC
		public Item Get(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return this.items[index];
			}
			return null;
		}

		// Token: 0x0400077C RID: 1916
		private int max;

		// Token: 0x0400077D RID: 1917
		private List<Item> items;
	}
}
