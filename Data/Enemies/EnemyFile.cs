using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Utility;
using fNbt;

namespace Mother4.Data.Enemies
{
	// Token: 0x02000025 RID: 37
	internal class EnemyFile
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00005B81 File Offset: 0x00003D81
		public static EnemyFile Instance
		{
			get
			{
				EnemyFile.Load();
				return EnemyFile.INSTANCE;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005B8D File Offset: 0x00003D8D
		public static void Load()
		{
			if (EnemyFile.INSTANCE == null)
			{
				EnemyFile.INSTANCE = new EnemyFile();
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005BA0 File Offset: 0x00003DA0
		private EnemyFile()
		{
			this.enemyDataDict = new Dictionary<int, EnemyData>();
			string text = Paths.DATA + "enemy.dat";
			if (File.Exists(text))
			{
				this.Load(text);
				return;
			}
			throw new FileNotFoundException("The enemy data file is missing.", text);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005BEC File Offset: 0x00003DEC
		private void Load(string path)
		{
			NbtFile nbtFile = new NbtFile(path);
			foreach (NbtTag nbtTag in nbtFile.RootTag)
			{
				if (nbtTag is NbtCompound)
				{
					EnemyData enemyData = new EnemyData((NbtCompound)nbtTag);
					int key = Hash.Get(enemyData.QualifiedName);
					this.enemyDataDict.Add(key, enemyData);
				}
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005C6C File Offset: 0x00003E6C
		public List<EnemyType> GetAllEnemyTypes()
		{
			List<EnemyType> list = new List<EnemyType>();
			foreach (int id in this.enemyDataDict.Keys)
			{
				list.Add(new EnemyType(id));
			}
			return list;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005CD0 File Offset: 0x00003ED0
		public List<EnemyData> GetAllEnemyData()
		{
			return new List<EnemyData>(this.enemyDataDict.Values);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005CE4 File Offset: 0x00003EE4
		public EnemyType GetEnemyType(string qualifiedName)
		{
			int num = Hash.Get(qualifiedName);
			if (!this.enemyDataDict.ContainsKey(num))
			{
				string message = string.Format("Enemy \"{0}\" is not present in the enemy file.", qualifiedName);
				throw new ArgumentException(message);
			}
			return new EnemyType(num);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005D20 File Offset: 0x00003F20
		public EnemyData GetData(EnemyType type)
		{
			EnemyData result = null;
			if (!this.enemyDataDict.TryGetValue(type.Identifier, out result))
			{
				throw new ArgumentException("Enemy type is not present in the enemy file.");
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005D54 File Offset: 0x00003F54
		public EnemyData GetData(string qualifiedName)
		{
			EnemyData result = null;
			int key = Hash.Get(qualifiedName);
			if (!this.enemyDataDict.TryGetValue(key, out result))
			{
				string message = string.Format("Enemy \"{0}\" is not present in the enemy file.", qualifiedName);
				throw new ArgumentException(message);
			}
			return result;
		}

		// Token: 0x04000192 RID: 402
		private const string FILENAME = "enemy.dat";

		// Token: 0x04000193 RID: 403
		private static EnemyFile INSTANCE;

		// Token: 0x04000194 RID: 404
		private Dictionary<int, EnemyData> enemyDataDict;
	}
}
