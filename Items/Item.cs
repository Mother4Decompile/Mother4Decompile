using System;
using System.Collections.Generic;
using Carbine.Utility;

namespace Mother4.Items
{
	// Token: 0x020000F9 RID: 249
	internal class Item
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x000223CD File Offset: 0x000205CD
		public bool Key
		{
			get
			{
				return this.isKey;
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000223D5 File Offset: 0x000205D5
		public Item(bool isKey)
		{
			this.isKey = isKey;
			this.properties = new Dictionary<int, object>();
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000223F0 File Offset: 0x000205F0
		public void Set(string property, object value)
		{
			int key = Hash.Get(property);
			if (this.properties.ContainsKey(key))
			{
				this.properties[key] = value;
				return;
			}
			this.properties.Add(key, value);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00022430 File Offset: 0x00020630
		public T Get<T>(string name)
		{
			int key = Hash.Get(name);
			T result;
			try
			{
				T t = (T)((object)this.properties[key]);
				result = t;
			}
			catch (KeyNotFoundException)
			{
				throw new ItemPropertyNotFoundException(name);
			}
			catch (InvalidCastException)
			{
				throw new InvalidPropertyType(typeof(T), this.properties[key].GetType());
			}
			return result;
		}

		// Token: 0x04000783 RID: 1923
		public const string NAME = "name";

		// Token: 0x04000784 RID: 1924
		private bool isKey;

		// Token: 0x04000785 RID: 1925
		private Dictionary<int, object> properties;
	}
}
