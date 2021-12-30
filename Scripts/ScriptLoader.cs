using System;
using System.Collections.Generic;
using System.Linq;
using fNbt;
using Mother4.Data;
using Mother4.Scripts.Actions;

namespace Mother4.Scripts
{
	// Token: 0x0200016F RID: 367
	internal class ScriptLoader
	{
		// Token: 0x060007BD RID: 1981 RVA: 0x00031FA0 File Offset: 0x000301A0
		public static Script? Load(string name)
		{
			NbtFile nbtFile = new NbtFile(ScriptLoader.SCRIPT_FILE);
			NbtCompound rootTag = nbtFile.RootTag;
			NbtTag nbtTag = rootTag.Get<NbtTag>(name);
			ICollection<NbtTag> collection = null;
			Script? result = null;
			if (nbtTag != null)
			{
				if (nbtTag is NbtList)
				{
					collection = (NbtList)nbtTag;
					int count = ((NbtList)nbtTag).Count;
				}
				else if (nbtTag is NbtCompound)
				{
					collection = (NbtCompound)nbtTag;
					int count2 = ((NbtCompound)nbtTag).Count;
				}
				RufiniAction[] array = new RufiniAction[collection.Count<NbtTag>()];
				int num = 0;
				foreach (NbtTag nbtTag2 in collection)
				{
					if (nbtTag2 != null && nbtTag2 is NbtCompound)
					{
						array[num++] = ActionFactory.FromNbt((NbtCompound)nbtTag2);
					}
				}
				result = new Script?(new Script
				{
					Name = name,
					Actions = array
				});
			}
			return result;
		}

		// Token: 0x0400097B RID: 2427
		private static readonly string SCRIPT_FILE = Paths.DATA + "script.dat";
	}
}
