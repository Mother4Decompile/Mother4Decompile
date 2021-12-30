using System;
using System.Collections.Generic;
using fNbt;

namespace Carbine.Flags
{
	// Token: 0x0200001E RID: 30
	public class ValueManager
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005FC6 File Offset: 0x000041C6
		public static ValueManager Instance
		{
			get
			{
				if (ValueManager.instance == null)
				{
					ValueManager.instance = new ValueManager();
				}
				return ValueManager.instance;
			}
		}

		// Token: 0x17000040 RID: 64
		public int this[int index]
		{
			get
			{
				if (!this.values.ContainsKey(index))
				{
					return 0;
				}
				return this.values[index];
			}
			set
			{
				if (this.values.ContainsKey(index))
				{
					this.values[index] = value;
					return;
				}
				this.values.Add(index, value);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006027 File Offset: 0x00004227
		private ValueManager()
		{
			this.values = new Dictionary<int, int>();
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000603A File Offset: 0x0000423A
		public void Reset()
		{
			this.values.Clear();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006048 File Offset: 0x00004248
		public void LoadFromNBT(NbtIntArray valueTag)
		{
			this.values.Clear();
			if (valueTag != null)
			{
				int[] intArrayValue = valueTag.IntArrayValue;
				for (int i = 0; i < intArrayValue.Length; i += 2)
				{
					this[intArrayValue[i]] = intArrayValue[i + 1];
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006088 File Offset: 0x00004288
		public NbtTag ToNBT()
		{
			int[] array = new int[this.values.Count * 2];
			int num = 0;
			foreach (KeyValuePair<int, int> keyValuePair in this.values)
			{
				array[num++] = keyValuePair.Key;
				array[num++] = keyValuePair.Value;
			}
			return new NbtIntArray("vals", array);
		}

		// Token: 0x0400009A RID: 154
		public const string NBT_TAG_NAME = "vals";

		// Token: 0x0400009B RID: 155
		public const int VALUE_ACTION_RETURN = 0;

		// Token: 0x0400009C RID: 156
		public const int VALUE_MONEY = 1;

		// Token: 0x0400009D RID: 157
		private static ValueManager instance;

		// Token: 0x0400009E RID: 158
		private Dictionary<int, int> values;
	}
}
