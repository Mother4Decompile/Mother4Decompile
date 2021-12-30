using System;
using System.Collections.Generic;

namespace Carbine.Utility
{
	// Token: 0x0200005E RID: 94
	public static class DictionaryExtensions
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000EA4D File Offset: 0x0000CC4D
		public static void AddReplace<K, V>(this Dictionary<K, V> dictionary, K key, V value)
		{
			if (dictionary.ContainsKey(key))
			{
				dictionary[key] = value;
				return;
			}
			dictionary.Add(key, value);
		}
	}
}
