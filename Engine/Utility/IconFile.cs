using System;
using System.Collections.Generic;
using fNbt;

namespace Carbine.Utility
{
	// Token: 0x02000063 RID: 99
	internal class IconFile
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x0000EB98 File Offset: 0x0000CD98
		public IconFile(string filename)
		{
			this.icons = new Dictionary<int, byte[]>();
			NbtFile file = new NbtFile(filename);
			this.LoadFile(file);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000EBC4 File Offset: 0x0000CDC4
		private void LoadFile(NbtFile file)
		{
			NbtCompound rootTag = file.RootTag;
			foreach (NbtTag nbtTag in rootTag)
			{
				if (nbtTag is NbtByteArray)
				{
					NbtByteArray nbtByteArray = (NbtByteArray)nbtTag;
					byte[] byteArrayValue = nbtByteArray.ByteArrayValue;
					int key = (int)Math.Sqrt((double)(byteArrayValue.Length / 4));
					this.icons.Add(key, byteArrayValue);
				}
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000EC44 File Offset: 0x0000CE44
		public byte[] GetBytesForSize(int width)
		{
			byte[] result = null;
			this.icons.TryGetValue(width, out result);
			return result;
		}

		// Token: 0x04000203 RID: 515
		private const int BYTES_PER_PIXEL = 4;

		// Token: 0x04000204 RID: 516
		private Dictionary<int, byte[]> icons;
	}
}
