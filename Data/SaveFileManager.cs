using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Flags;
using fNbt;
using Mother4.Data.Character;
using SFML.System;

namespace Mother4.Data
{
	// Token: 0x02000035 RID: 53
	internal class SaveFileManager
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006771 File Offset: 0x00004971
		public static SaveFileManager Instance
		{
			get
			{
				if (SaveFileManager.instance != null)
				{
					return SaveFileManager.instance;
				}
				return SaveFileManager.instance = new SaveFileManager();
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000678B File Offset: 0x0000498B
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00006793 File Offset: 0x00004993
		public SaveProfile CurrentProfile
		{
			get
			{
				return this.currentProfile;
			}
			set
			{
				this.currentProfile = value;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000679C File Offset: 0x0000499C
		private SaveFileManager()
		{
			if (File.Exists("sav.dat"))
			{
				this.file = new NbtFile("sav.dat");
			}
			else
			{
				NbtCompound rootTag = new NbtCompound("saves");
				this.file = new NbtFile(rootTag);
			}
			if (!this.file.RootTag.Contains("v"))
			{
				this.file.RootTag.Add(new NbtInt("v", 2));
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006818 File Offset: 0x00004A18
		private string[] PartyToStringArray(CharacterType[] party)
		{
			string[] array = new string[(party == null) ? 0 : party.Length];
			for (int i = 0; i < array.Length; i++)
			{
				CharacterData data = CharacterFile.Instance.GetData(party[i]);
				array[i] = data.QualifiedName;
			}
			return array;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006864 File Offset: 0x00004A64
		private SaveProfile LoadSaveProfile(NbtCompound saveTag)
		{
			bool flag = true;
			NbtInt nbtInt = null;
			flag &= saveTag.TryGet<NbtInt>("idx", out nbtInt);
			NbtList nbtList = null;
			flag &= saveTag.TryGet<NbtList>("prty", out nbtList);
			NbtString nbtString = null;
			flag &= saveTag.TryGet<NbtString>("map", out nbtString);
			NbtInt nbtInt2 = null;
			flag &= saveTag.TryGet<NbtInt>("x", out nbtInt2);
			NbtInt nbtInt3 = null;
			flag &= saveTag.TryGet<NbtInt>("y", out nbtInt3);
			NbtInt nbtInt4 = null;
			flag &= saveTag.TryGet<NbtInt>("tm", out nbtInt4);
			NbtInt nbtInt5 = null;
			flag &= saveTag.TryGet<NbtInt>("flv", out nbtInt5);
			Vector2f position = default(Vector2f);
			if (nbtInt2 != null && nbtInt3 != null)
			{
				position.X = (float)nbtInt2.IntValue;
				position.Y = (float)nbtInt3.IntValue;
			}
			CharacterType[] array;
			if (nbtList != null)
			{
				array = new CharacterType[nbtList.Count];
				for (int i = 0; i < array.Length; i++)
				{
					NbtString nbtString2 = nbtList.Get<NbtString>(i);
					array[i] = CharacterFile.Instance.GetCharacterType(nbtString2.Value);
				}
			}
			else
			{
				array = new CharacterType[0];
			}
			return new SaveProfile
			{
				IsValid = flag,
				Index = ((nbtInt == null) ? 0 : nbtInt.IntValue),
				Party = array,
				MapName = ((nbtString == null) ? string.Empty : nbtString.StringValue),
				Position = position,
				Time = ((nbtInt4 == null) ? 0 : nbtInt4.IntValue),
				Flavor = ((nbtInt5 == null) ? 0 : nbtInt5.IntValue)
			};
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000069F8 File Offset: 0x00004BF8
		private NbtTag SaveProfileToNBT(SaveProfile profile)
		{
			NbtCompound nbtCompound = new NbtCompound("prf");
			nbtCompound.Add(new NbtInt("idx", profile.Index));
			nbtCompound.Add(new NbtString("map", profile.MapName ?? string.Empty));
			nbtCompound.Add(new NbtInt("x", (int)profile.Position.X));
			nbtCompound.Add(new NbtInt("y", (int)profile.Position.Y));
			nbtCompound.Add(new NbtInt("tm", profile.Time));
			nbtCompound.Add(new NbtInt("flv", profile.Flavor));
			NbtList nbtList = new NbtList("prty", NbtTagType.String);
			string[] array = this.PartyToStringArray(profile.Party);
			for (int i = 0; i < array.Length; i++)
			{
				NbtString newTag = new NbtString(array[i]);
				nbtList.Add(newTag);
			}
			nbtCompound.Add(nbtList);
			return nbtCompound;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006AF4 File Offset: 0x00004CF4
		public IDictionary<int, SaveProfile> LoadProfiles()
		{
			IDictionary<int, SaveProfile> dictionary = new Dictionary<int, SaveProfile>();
			NbtCompound rootTag = this.file.RootTag;
			NbtList nbtList = rootTag.Get<NbtList>("sav");
			if (nbtList != null)
			{
				foreach (NbtTag nbtTag in nbtList)
				{
					if (nbtTag is NbtCompound)
					{
						NbtCompound nbtCompound = (NbtCompound)nbtTag;
						NbtCompound nbtCompound2 = nbtCompound.Get<NbtCompound>("prf");
						if (nbtCompound2 != null)
						{
							SaveProfile value = this.LoadSaveProfile(nbtCompound2);
							if (!dictionary.ContainsKey(value.Index))
							{
								dictionary.Add(value.Index, value);
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006BA8 File Offset: 0x00004DA8
		public void LoadFile(int saveIndex)
		{
			NbtCompound rootTag = this.file.RootTag;
			NbtList nbtList = rootTag.Get<NbtList>("sav");
			if (nbtList != null)
			{
				foreach (NbtTag nbtTag in nbtList)
				{
					NbtCompound nbtCompound = (NbtCompound)nbtTag;
					NbtCompound saveTag = null;
					bool flag = nbtCompound.TryGet<NbtCompound>("prf", out saveTag);
					if (flag)
					{
						SaveProfile saveProfile = this.LoadSaveProfile(saveTag);
						if (saveProfile.Index == saveIndex)
						{
							this.currentProfile = saveProfile;
							PartyManager.Instance.Clear();
							PartyManager.Instance.AddAll(this.currentProfile.Party);
							NbtIntArray flagTag = null;
							flag &= nbtCompound.TryGet<NbtIntArray>("flags", out flagTag);
							FlagManager.Instance.LoadFromNBT(flagTag);
							NbtIntArray valueTag = null;
							flag &= nbtCompound.TryGet<NbtIntArray>("vals", out valueTag);
							ValueManager.Instance.LoadFromNBT(valueTag);
							NbtList nameListTag = null;
							flag &= nbtCompound.TryGet<NbtList>("names", out nameListTag);
							CharacterNames.LoadFromNBT(nameListTag);
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006CCC File Offset: 0x00004ECC
		public void SaveFile()
		{
			DateTime now = DateTime.Now;
			NbtCompound rootTag = this.file.RootTag;
			NbtList nbtList = rootTag.Get<NbtList>("sav");
			NbtCompound nbtCompound = null;
			if (nbtList != null)
			{
				using (IEnumerator<NbtTag> enumerator = nbtList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NbtTag nbtTag = enumerator.Current;
						NbtCompound nbtCompound2 = (NbtCompound)nbtTag;
						NbtCompound nbtCompound3 = nbtCompound2.Get<NbtCompound>("prf");
						if (nbtCompound3 != null)
						{
							NbtInt nbtInt = nbtCompound3.Get<NbtInt>("idx");
							if (nbtInt != null && nbtInt.IntValue == this.currentProfile.Index)
							{
								nbtCompound = nbtCompound2;
								nbtCompound.Clear();
								break;
							}
						}
					}
					goto IL_A9;
				}
			}
			nbtList = new NbtList("sav", NbtTagType.Compound);
			rootTag.Add(nbtList);
			IL_A9:
			bool flag = false;
			if (nbtCompound == null)
			{
				nbtCompound = new NbtCompound();
				flag = true;
			}
			nbtCompound.Add(this.SaveProfileToNBT(this.currentProfile));
			nbtCompound.Add(FlagManager.Instance.ToNBT());
			nbtCompound.Add(ValueManager.Instance.ToNBT());
			nbtCompound.Add(CharacterNames.ToNBT());
			if (flag)
			{
				nbtList.Add(nbtCompound);
			}
			this.file.SaveToFile("sav.dat", NbtCompression.GZip);
			Console.WriteLine("Saved profile in {0} seconds", Math.Round((double)(DateTime.Now.Millisecond - now.Millisecond) / 1000.0, 2));
		}

		// Token: 0x040001EC RID: 492
		public const string SAVE_FILE = "sav.dat";

		// Token: 0x040001ED RID: 493
		public const string PROFILE_TAG_NAME = "prf";

		// Token: 0x040001EE RID: 494
		public const string LIST_TAG_NAME = "sav";

		// Token: 0x040001EF RID: 495
		private const int SAVE_FORMAT_VERSION = 2;

		// Token: 0x040001F0 RID: 496
		private const int LOWEST_COMPAT_SAVE_FORMAT_VERSION = 2;

		// Token: 0x040001F1 RID: 497
		private static SaveFileManager instance;

		// Token: 0x040001F2 RID: 498
		private SaveProfile currentProfile;

		// Token: 0x040001F3 RID: 499
		private NbtFile file;
	}
}
