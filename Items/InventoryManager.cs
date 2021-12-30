using System;
using System.Collections.Generic;
using Mother4.Data;

namespace Mother4.Items
{
	// Token: 0x020000F7 RID: 247
	internal class InventoryManager
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000222F9 File Offset: 0x000204F9
		public static InventoryManager Instance
		{
			get
			{
				if (InventoryManager.instance == null)
				{
					InventoryManager.instance = new InventoryManager();
				}
				return InventoryManager.instance;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x00022311 File Offset: 0x00020511
		public Inventory KeyInventory
		{
			get
			{
				return this.keyInventory;
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00022319 File Offset: 0x00020519
		private InventoryManager()
		{
			this.keyInventory = new Inventory(14);
			this.inventories = new Dictionary<CharacterType, Inventory>();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0002233C File Offset: 0x0002053C
		public Inventory Add(CharacterType key)
		{
			Inventory inventory;
			if (!this.inventories.ContainsKey(key))
			{
				inventory = new Inventory(14);
				this.inventories.Add(key, inventory);
			}
			else
			{
				inventory = this.inventories[key];
			}
			return inventory;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0002237C File Offset: 0x0002057C
		public void Remove(CharacterType key)
		{
			this.inventories.Remove(key);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0002238B File Offset: 0x0002058B
		public Inventory Get(CharacterType key)
		{
			if (!this.inventories.ContainsKey(key))
			{
				this.Add(key);
			}
			return this.inventories[key];
		}

		// Token: 0x0400077E RID: 1918
		public const int INVENTORY_SIZE = 14;

		// Token: 0x0400077F RID: 1919
		public const int KEY_INVENTORY_SIZE = 14;

		// Token: 0x04000780 RID: 1920
		private static InventoryManager instance;

		// Token: 0x04000781 RID: 1921
		private Inventory keyInventory;

		// Token: 0x04000782 RID: 1922
		private Dictionary<CharacterType, Inventory> inventories;
	}
}
