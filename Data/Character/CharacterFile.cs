using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Utility;
using fNbt;
using SFML.System;

namespace Mother4.Data.Character
{
	// Token: 0x0200001B RID: 27
	internal class CharacterFile
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00004318 File Offset: 0x00002518
		public static CharacterFile Instance
		{
			get
			{
				CharacterFile.Load();
				return CharacterFile.INSTANCE;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004324 File Offset: 0x00002524
		public static void Load()
		{
			if (CharacterFile.INSTANCE == null)
			{
				CharacterFile.INSTANCE = new CharacterFile();
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00004337 File Offset: 0x00002537
		public string InitialMap
		{
			get
			{
				return this.initialMap;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000433F File Offset: 0x0000253F
		public Vector2i InitialPosition
		{
			get
			{
				return this.initialPosition;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00004347 File Offset: 0x00002547
		public CharacterType[] InitialParty
		{
			get
			{
				return this.initialParty;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004350 File Offset: 0x00002550
		private CharacterFile()
		{
			this.characterDataDict = new Dictionary<int, CharacterData>();
			string text = Paths.DATA + "character.dat";
			if (File.Exists(text))
			{
				this.Load(text);
				return;
			}
			throw new FileNotFoundException("The character data file is missing.", text);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000439C File Offset: 0x0000259C
		private Vector2i ReadInitialPosition(NbtTag tag)
		{
			return default(Vector2i);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000043B4 File Offset: 0x000025B4
		private CharacterType[] ReadInitialParty(NbtTag tag)
		{
			CharacterType[] array = null;
			if (tag is NbtList)
			{
				NbtList nbtList = (NbtList)tag;
				if (nbtList.ListType == NbtTagType.String)
				{
					array = new CharacterType[nbtList.Count];
					for (int i = 0; i < array.Length; i++)
					{
						NbtString nbtString = nbtList.Get<NbtString>(i);
						array[i] = this.GetCharacterType(nbtString.Value);
					}
				}
			}
			return array;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004418 File Offset: 0x00002618
		private void LoadSettings(NbtCompound tag)
		{
			NbtTag nbtTag;
			this.initialMap = (tag.TryGet("map", out nbtTag) ? nbtTag.StringValue : string.Empty);
			this.initialPosition = (tag.TryGet("position", out nbtTag) ? this.ReadInitialPosition(nbtTag) : default(Vector2i));
			this.initialParty = (tag.TryGet("party", out nbtTag) ? this.ReadInitialParty(nbtTag) : null);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004490 File Offset: 0x00002690
		private void LoadCharacterData(NbtCompound compoundTag)
		{
			foreach (NbtTag nbtTag in compoundTag)
			{
				if (nbtTag is NbtCompound)
				{
					CharacterData characterData = new CharacterData((NbtCompound)nbtTag);
					int key = Hash.Get(characterData.QualifiedName);
					this.characterDataDict.Add(key, characterData);
				}
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004500 File Offset: 0x00002700
		private void Load(string path)
		{
			NbtFile nbtFile = new NbtFile(path);
			NbtCompound nbtCompound;
			if (nbtFile.RootTag.TryGet<NbtCompound>("party", out nbtCompound))
			{
				this.LoadCharacterData(nbtCompound);
			}
			if (nbtFile.RootTag.TryGet<NbtCompound>("extra", out nbtCompound))
			{
				this.LoadCharacterData(nbtCompound);
			}
			if (nbtFile.RootTag.TryGet<NbtCompound>("initial", out nbtCompound))
			{
				this.LoadSettings(nbtCompound);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004568 File Offset: 0x00002768
		public List<CharacterType> GetAllCharacterTypes()
		{
			List<CharacterType> list = new List<CharacterType>();
			foreach (int id in this.characterDataDict.Keys)
			{
				list.Add(new CharacterType(id));
			}
			return list;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000045CC File Offset: 0x000027CC
		public List<CharacterData> GetAllCharacterData()
		{
			return new List<CharacterData>(this.characterDataDict.Values);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000045E0 File Offset: 0x000027E0
		public CharacterType GetCharacterType(string qualifiedName)
		{
			int num = Hash.Get(qualifiedName);
			if (!this.characterDataDict.ContainsKey(num))
			{
				string message = string.Format("Character \"{0}\" is not present in the character file.", qualifiedName);
				throw new ArgumentException(message);
			}
			return new CharacterType(num);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000461C File Offset: 0x0000281C
		public CharacterData GetData(CharacterType type)
		{
			CharacterData result = null;
			if (!this.characterDataDict.TryGetValue(type.Identifier, out result))
			{
				throw new ArgumentException("Character type is not present in the character file.");
			}
			return result;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004650 File Offset: 0x00002850
		public CharacterData GetData(string qualifiedName)
		{
			CharacterData result = null;
			int key = Hash.Get(qualifiedName);
			if (!this.characterDataDict.TryGetValue(key, out result))
			{
				string message = string.Format("Character \"{0}\" is not present in the character file.", qualifiedName);
				throw new ArgumentException(message);
			}
			return result;
		}

		// Token: 0x0400010E RID: 270
		private const string FILENAME = "character.dat";

		// Token: 0x0400010F RID: 271
		private const string SETTINGS_TAG = "initial";

		// Token: 0x04000110 RID: 272
		private const string PARTY_TAG = "party";

		// Token: 0x04000111 RID: 273
		private const string EXTRA_TAG = "extra";

		// Token: 0x04000112 RID: 274
		private static CharacterFile INSTANCE;

		// Token: 0x04000113 RID: 275
		private Dictionary<int, CharacterData> characterDataDict;

		// Token: 0x04000114 RID: 276
		private string initialMap;

		// Token: 0x04000115 RID: 277
		private Vector2i initialPosition;

		// Token: 0x04000116 RID: 278
		private CharacterType[] initialParty;
	}
}
