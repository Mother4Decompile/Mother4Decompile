using System;
using System.Collections.Generic;
using fNbt;
using Mother4.Data.Character;
using Mother4.GUI.Text;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;

namespace Mother4.Data
{
	// Token: 0x020000ED RID: 237
	internal static class CharacterNames
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x000211D4 File Offset: 0x0001F3D4
		public static string GetName(CharacterType character)
		{
			string empty;
			if (!CharacterNames.names.TryGetValue(character, out empty))
			{
				empty = string.Empty;
			}
			return empty;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000211F8 File Offset: 0x0001F3F8
		public static string GetGroup(CharacterType[] characters)
		{
			string result = string.Empty;
			if (characters.Length == 1)
			{
				result = CharacterNames.GetName(characters[0]);
			}
			else if (characters.Length == 2)
			{
				RufiniString rufiniString = StringFile.Instance.Get("system.party.two");
				if (rufiniString.Value != null)
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					for (int i = 0; i < characters.Length; i++)
					{
						dictionary.Add((i + 1).ToString(), CharacterNames.GetName(characters[i]));
					}
					result = TextProcessor.ProcessReplacements(rufiniString.Value, dictionary);
				}
			}
			else if (characters.Length > 2)
			{
				RufiniString rufiniString2 = StringFile.Instance.Get("system.party.many");
				if (rufiniString2.Value != null)
				{
					Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
					for (int j = 0; j < characters.Length; j++)
					{
						dictionary2.Add((j + 1).ToString(), CharacterNames.GetName(characters[j]));
					}
					result = TextProcessor.ProcessReplacements(rufiniString2.Value, dictionary2);
				}
			}
			return result;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00021300 File Offset: 0x0001F500
		public static void SetName(CharacterType character, string name)
		{
			if (CharacterNames.names.ContainsKey(character))
			{
				CharacterNames.names[character] = name;
				return;
			}
			CharacterNames.names.Add(character, name);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00021328 File Offset: 0x0001F528
		public static void LoadFromNBT(NbtList nameListTag)
		{
			if (nameListTag != null && nameListTag.TagType == NbtTagType.Compound)
			{
				foreach (NbtTag nbtTag in nameListTag)
				{
					NbtCompound nbtCompound = (NbtCompound)nbtTag;
					bool flag = true;
					NbtString nbtString = null;
					flag &= nbtCompound.TryGet<NbtString>("k", out nbtString);
					NbtString nbtString2 = null;
					flag &= nbtCompound.TryGet<NbtString>("v", out nbtString2);
					if (flag)
					{
						CharacterType characterType = CharacterFile.Instance.GetCharacterType(nbtString.Value);
						string value = nbtString2.Value;
						CharacterNames.SetName(characterType, value);
					}
				}
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000213D0 File Offset: 0x0001F5D0
		public static NbtTag ToNBT()
		{
			IList<NbtCompound> list = new List<NbtCompound>();
			foreach (KeyValuePair<CharacterType, string> keyValuePair in CharacterNames.names)
			{
				CharacterType key = keyValuePair.Key;
				CharacterData data = CharacterFile.Instance.GetData(key);
				list.Add(new NbtCompound
				{
					new NbtString("k", data.QualifiedName),
					new NbtString("v", keyValuePair.Value)
				});
			}
			return new NbtList("names", list);
		}

		// Token: 0x04000741 RID: 1857
		public const string NBT_TAG_NAME = "names";

		// Token: 0x04000742 RID: 1858
		private const string TWO_CHARACTER_GROUP = "system.party.two";

		// Token: 0x04000743 RID: 1859
		private const string MANY_CHARACTER_GROUP = "system.party.many";

		// Token: 0x04000744 RID: 1860
		private static Dictionary<CharacterType, string> names = new Dictionary<CharacterType, string>
		{
			{
				CharacterType.Travis,
				"Travis"
			},
			{
				CharacterType.Zack,
				"Zack"
			},
			{
				CharacterType.Meryl,
				"Meryl"
			},
			{
				CharacterType.Floyd,
				"Floyd"
			},
			{
				CharacterType.Leo,
				"Leo"
			},
			{
				CharacterType.Renee,
				"Renee"
			},
			{
				CharacterType.Dog,
				"Doggie O'Dogovan"
			}
		};
	}
}
