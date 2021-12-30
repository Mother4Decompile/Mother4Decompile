using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Utility;
using fNbt;

namespace Mother4.Data.Psi
{
	// Token: 0x02000031 RID: 49
	internal class PsiFile
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00006415 File Offset: 0x00004615
		public static PsiFile Instance
		{
			get
			{
				PsiFile.Load();
				return PsiFile.INSTANCE;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006421 File Offset: 0x00004621
		public static void Load()
		{
			if (PsiFile.INSTANCE == null)
			{
				PsiFile.INSTANCE = new PsiFile();
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006434 File Offset: 0x00004634
		private PsiFile()
		{
			this.psiDataDict = new Dictionary<int, PsiData>();
			string text = Paths.DATA + "psi.dat";
			if (File.Exists(text))
			{
				this.Load(text);
				return;
			}
			throw new FileNotFoundException("The psi data file is missing.", text);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006480 File Offset: 0x00004680
		private void Load(NbtCompound rootTag, Func<NbtCompound, PsiData> loadFunc)
		{
			foreach (NbtTag nbtTag in rootTag)
			{
				if (nbtTag is NbtCompound)
				{
					PsiData psiData = loadFunc((NbtCompound)nbtTag);
					int key = Hash.Get(psiData.QualifiedName);
					this.psiDataDict.Add(key, psiData);
				}
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006510 File Offset: 0x00004710
		private void Load(string path)
		{
			NbtFile nbtFile = new NbtFile(path);
			NbtCompound rootTag;
			if (nbtFile.RootTag.TryGet<NbtCompound>("offense", out rootTag))
			{
				this.Load(rootTag, (NbtCompound x) => new OffensePsiData(x));
			}
			if (nbtFile.RootTag.TryGet<NbtCompound>("defense", out rootTag))
			{
				this.Load(rootTag, (NbtCompound x) => new DefensePsiData(x));
			}
			if (nbtFile.RootTag.TryGet<NbtCompound>("assist", out rootTag))
			{
				this.Load(rootTag, (NbtCompound x) => new AssistPsiData(x));
			}
			if (nbtFile.RootTag.TryGet<NbtCompound>("other", out rootTag))
			{
				this.Load(rootTag, (NbtCompound x) => new OtherPsiData(x));
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006604 File Offset: 0x00004804
		public List<PsiType> GetAllPsiTypes()
		{
			List<PsiType> list = new List<PsiType>();
			foreach (int id in this.psiDataDict.Keys)
			{
				list.Add(new PsiType(id));
			}
			return list;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006668 File Offset: 0x00004868
		public List<PsiData> GetAllPsiData()
		{
			return new List<PsiData>(this.psiDataDict.Values);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000667C File Offset: 0x0000487C
		public PsiType GetPsiType(string qualifiedName)
		{
			int num = Hash.Get(qualifiedName);
			if (!this.psiDataDict.ContainsKey(num))
			{
				string message = string.Format("Enemy \"{0}\" is not present in the psi file.", qualifiedName);
				throw new ArgumentException(message);
			}
			return new PsiType(num);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000066B8 File Offset: 0x000048B8
		public PsiData GetData(PsiType type)
		{
			PsiData result;
			if (!this.psiDataDict.TryGetValue(type.Identifier, out result))
			{
				throw new ArgumentException("Psi type is not present in the psi file.");
			}
			return result;
		}

		// Token: 0x040001D7 RID: 471
		private const string FILENAME = "psi.dat";

		// Token: 0x040001D8 RID: 472
		private const string OFFENSE_TAG = "offense";

		// Token: 0x040001D9 RID: 473
		private const string DEFENSE_TAG = "defense";

		// Token: 0x040001DA RID: 474
		private const string ASSIST_TAG = "assist";

		// Token: 0x040001DB RID: 475
		private const string OTHER_TAG = "other";

		// Token: 0x040001DC RID: 476
		private static PsiFile INSTANCE;

		// Token: 0x040001DD RID: 477
		private Dictionary<int, PsiData> psiDataDict;
	}
}
