using System;
using System.Collections.Generic;
using fNbt;

namespace Carbine.Flags
{
	// Token: 0x0200001D RID: 29
	public class FlagManager
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00005E13 File Offset: 0x00004013
		public static FlagManager Instance
		{
			get
			{
				if (FlagManager.instance == null)
				{
					FlagManager.instance = new FlagManager();
				}
				return FlagManager.instance;
			}
		}

		// Token: 0x1700003E RID: 62
		public bool this[int flag]
		{
			get
			{
				return this.flags.ContainsKey(flag) && this.flags[flag];
			}
			set
			{
				if (flag > 0)
				{
					if (this.flags.ContainsKey(flag))
					{
						this.flags[flag] = value;
						return;
					}
					this.flags.Add(flag, value);
				}
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005E78 File Offset: 0x00004078
		private FlagManager()
		{
			this.flags = new Dictionary<int, bool>();
			this.SetInitialState();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005E91 File Offset: 0x00004091
		private void SetInitialState()
		{
			this.flags.Add(0, true);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005EA0 File Offset: 0x000040A0
		public void Toggle(int flag)
		{
			if (this.flags.ContainsKey(flag))
			{
				this.flags[flag] = !this.flags[flag];
				return;
			}
			this.flags.Add(flag, true);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005ED9 File Offset: 0x000040D9
		public void Reset()
		{
			this.flags.Clear();
			this.SetInitialState();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005EEC File Offset: 0x000040EC
		public void LoadFromNBT(NbtIntArray flagTag)
		{
			this.Reset();
			if (flagTag != null)
			{
				foreach (int num in flagTag.IntArrayValue)
				{
					int flag = num >> 1;
					bool value = (num & 1) == 1;
					this[flag] = value;
				}
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005F30 File Offset: 0x00004130
		public NbtIntArray ToNBT()
		{
			int[] array = new int[this.flags.Count - 1];
			int num = 0;
			foreach (KeyValuePair<int, bool> keyValuePair in this.flags)
			{
				if (keyValuePair.Key > 0)
				{
					array[num++] = (keyValuePair.Key << 1 | (keyValuePair.Value ? 1 : 0));
				}
			}
			return new NbtIntArray("flags", array);
		}

		// Token: 0x04000092 RID: 146
		public const string NBT_TAG_NAME = "flags";

		// Token: 0x04000093 RID: 147
		public const int FLAG_TRUE = 0;

		// Token: 0x04000094 RID: 148
		public const int FLAG_DAY_NIGHT = 1;

		// Token: 0x04000095 RID: 149
		public const int FLAG_QUESTION_REGISTER = 2;

		// Token: 0x04000096 RID: 150
		public const int FLAG_TELEPATHY_REQUEST = 3;

		// Token: 0x04000097 RID: 151
		public const int FLAG_TELEPATHY_MODE = 4;

		// Token: 0x04000098 RID: 152
		private static FlagManager instance;

		// Token: 0x04000099 RID: 153
		private Dictionary<int, bool> flags;
	}
}
