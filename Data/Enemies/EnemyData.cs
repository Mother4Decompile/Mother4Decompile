using System;
using System.Collections.Generic;
using fNbt;
using Mother4.Battle;
using Mother4.Battle.Actions;

namespace Mother4.Data.Enemies
{
	// Token: 0x02000024 RID: 36
	internal class EnemyData
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000086 RID: 134 RVA: 0x0000530A File Offset: 0x0000350A
		public string QualifiedName
		{
			get
			{
				return this.qualifiedName;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00005312 File Offset: 0x00003512
		public string BackgroundName
		{
			get
			{
				return this.bbgName;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000531A File Offset: 0x0000351A
		public string MusicName
		{
			get
			{
				return this.bgmName;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00005322 File Offset: 0x00003522
		public string SpriteName
		{
			get
			{
				return this.spriteName;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000532A File Offset: 0x0000352A
		public int Experience
		{
			get
			{
				return this.experience;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00005332 File Offset: 0x00003532
		public int Reward
		{
			get
			{
				return this.reward;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000533A File Offset: 0x0000353A
		public int MoverType
		{
			get
			{
				return this.moverType;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00005342 File Offset: 0x00003542
		public EnemyOptions Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000534A File Offset: 0x0000354A
		public EnemyImmunities Immunities
		{
			get
			{
				return this.immunities;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00005352 File Offset: 0x00003552
		public int Level
		{
			get
			{
				return this.level;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000535A File Offset: 0x0000355A
		public int HP
		{
			get
			{
				return this.hp;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00005362 File Offset: 0x00003562
		public int PP
		{
			get
			{
				return this.pp;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000536A File Offset: 0x0000356A
		public int Offense
		{
			get
			{
				return this.offense;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00005372 File Offset: 0x00003572
		public int Defense
		{
			get
			{
				return this.defense;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000537A File Offset: 0x0000357A
		public int Speed
		{
			get
			{
				return this.speed;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00005382 File Offset: 0x00003582
		public int Guts
		{
			get
			{
				return this.guts;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000538A File Offset: 0x0000358A
		public int IQ
		{
			get
			{
				return this.iq;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00005392 File Offset: 0x00003592
		public int Luck
		{
			get
			{
				return this.luck;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000098 RID: 152 RVA: 0x0000539A File Offset: 0x0000359A
		public float ModifierElectric
		{
			get
			{
				return this.modElectric;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000053A2 File Offset: 0x000035A2
		public float ModifierExplosive
		{
			get
			{
				return this.modExplosive;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000053AA File Offset: 0x000035AA
		public float ModifierFire
		{
			get
			{
				return this.modFire;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000053B2 File Offset: 0x000035B2
		public float ModifierIce
		{
			get
			{
				return this.modIce;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000053BA File Offset: 0x000035BA
		public float ModifierNausea
		{
			get
			{
				return this.modNausea;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000053C2 File Offset: 0x000035C2
		public float ModifierPhysical
		{
			get
			{
				return this.modPhysical;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000053CA File Offset: 0x000035CA
		public float ModifierPoison
		{
			get
			{
				return this.modPoison;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000053D2 File Offset: 0x000035D2
		public float ModifierWater
		{
			get
			{
				return this.modWater;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000053DA File Offset: 0x000035DA
		public int AiType
		{
			get
			{
				return this.aiType;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000053E4 File Offset: 0x000035E4
		public EnemyData(NbtCompound tag)
		{
			if (tag.Name == null || tag.Name.Length == 0)
			{
				throw new ArgumentException("Cannot load enemy data from an unnamed tag.");
			}
			this.strings = new Dictionary<string, string>();
			this.aiProperties = new Dictionary<string, object>();
			this.actionList = new List<ActionParams>();
			this.LoadBaseAttributes(tag);
			NbtCompound tag2 = null;
			if (tag.TryGet<NbtCompound>("stat", out tag2))
			{
				this.LoadStats(tag2);
			}
			NbtCompound tag3 = null;
			if (tag.TryGet<NbtCompound>("mod", out tag3))
			{
				this.LoadModifiers(tag3);
			}
			NbtCompound stringsTag = null;
			if (tag.TryGet<NbtCompound>("str", out stringsTag))
			{
				this.LoadStrings(stringsTag);
			}
			NbtCompound aiTag = null;
			if (tag.TryGet<NbtCompound>("bhv", out aiTag))
			{
				this.LoadAi(aiTag);
			}
			NbtList actionsTag = null;
			if (tag.TryGet<NbtList>("act", out actionsTag))
			{
				this.LoadActions(actionsTag);
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000054BC File Offset: 0x000036BC
		private void LoadBaseAttributes(NbtCompound tag)
		{
			this.qualifiedName = tag.Name;
			NbtTag nbtTag;
			if (tag.TryGet("exp", out nbtTag))
			{
				this.experience = nbtTag.IntValue;
			}
			if (tag.TryGet("rwd", out nbtTag))
			{
				this.reward = nbtTag.IntValue;
			}
			if (tag.TryGet("bbg", out nbtTag))
			{
				this.bbgName = nbtTag.StringValue;
			}
			if (tag.TryGet("bgm", out nbtTag))
			{
				this.bgmName = nbtTag.StringValue;
			}
			if (tag.TryGet("spr", out nbtTag))
			{
				this.spriteName = nbtTag.StringValue;
			}
			if (tag.TryGet("mov", out nbtTag))
			{
				this.moverType = nbtTag.IntValue;
			}
			if (tag.TryGet("opt", out nbtTag))
			{
				this.options = (EnemyOptions)nbtTag.IntValue;
			}
			if (tag.TryGet("imm", out nbtTag))
			{
				this.immunities = (EnemyImmunities)nbtTag.IntValue;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000055B0 File Offset: 0x000037B0
		private void LoadStats(NbtCompound tag)
		{
			NbtTag nbtTag;
			if (tag.TryGet("hp", out nbtTag))
			{
				this.hp = nbtTag.IntValue;
			}
			if (tag.TryGet("pp", out nbtTag))
			{
				this.pp = nbtTag.IntValue;
			}
			if (tag.TryGet("lvl", out nbtTag))
			{
				this.level = nbtTag.IntValue;
			}
			if (tag.TryGet("off", out nbtTag))
			{
				this.offense = nbtTag.IntValue;
			}
			if (tag.TryGet("def", out nbtTag))
			{
				this.defense = nbtTag.IntValue;
			}
			if (tag.TryGet("spd", out nbtTag))
			{
				this.speed = nbtTag.IntValue;
			}
			if (tag.TryGet("gut", out nbtTag))
			{
				this.guts = nbtTag.IntValue;
			}
			if (tag.TryGet("iq", out nbtTag))
			{
				this.iq = nbtTag.IntValue;
			}
			if (tag.TryGet("lck", out nbtTag))
			{
				this.luck = nbtTag.IntValue;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000056B0 File Offset: 0x000038B0
		private void LoadModifiers(NbtCompound tag)
		{
			NbtTag nbtTag;
			if (tag.TryGet("elec", out nbtTag))
			{
				this.modElectric = nbtTag.FloatValue;
			}
			if (tag.TryGet("expl", out nbtTag))
			{
				this.modExplosive = nbtTag.FloatValue;
			}
			if (tag.TryGet("fire", out nbtTag))
			{
				this.modFire = nbtTag.FloatValue;
			}
			if (tag.TryGet("ice", out nbtTag))
			{
				this.modIce = nbtTag.FloatValue;
			}
			if (tag.TryGet("naus", out nbtTag))
			{
				this.modNausea = nbtTag.FloatValue;
			}
			if (tag.TryGet("phys", out nbtTag))
			{
				this.modPhysical = nbtTag.FloatValue;
			}
			if (tag.TryGet("pois", out nbtTag))
			{
				this.modPoison = nbtTag.FloatValue;
			}
			if (tag.TryGet("wet", out nbtTag))
			{
				this.modWater = nbtTag.FloatValue;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005798 File Offset: 0x00003998
		private void LoadStrings(NbtCompound stringsTag)
		{
			foreach (NbtTag nbtTag in stringsTag)
			{
				if (nbtTag is NbtString)
				{
					this.strings.Add(nbtTag.Name, nbtTag.StringValue);
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000057F8 File Offset: 0x000039F8
		private object GetPropertyObject(NbtTag propertyTag)
		{
			object result = null;
			if (propertyTag is NbtByte || propertyTag is NbtShort || propertyTag is NbtInt)
			{
				result = propertyTag.IntValue;
			}
			else if (propertyTag is NbtFloat || propertyTag is NbtDouble)
			{
				result = propertyTag.FloatValue;
			}
			else if (propertyTag is NbtString)
			{
				result = propertyTag.StringValue;
			}
			return result;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000585C File Offset: 0x00003A5C
		private void LoadAi(NbtCompound aiTag)
		{
			NbtTag nbtTag;
			if (aiTag.TryGet("ai", out nbtTag))
			{
				this.aiType = nbtTag.IntValue;
			}
			NbtCompound nbtCompound;
			if (aiTag.TryGet<NbtCompound>("parm", out nbtCompound))
			{
				foreach (NbtTag nbtTag2 in nbtCompound)
				{
					object propertyObject = this.GetPropertyObject(nbtTag2);
					if (propertyObject != null)
					{
						this.aiProperties.Add(nbtTag2.Name, propertyObject);
					}
				}
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005900 File Offset: 0x00003B00
		private void LoadActions(NbtList actionsTag)
		{
			foreach (NbtTag nbtTag in actionsTag)
			{
				if (nbtTag is NbtCompound)
				{
					NbtCompound nbtCompound = (NbtCompound)nbtTag;
					int actionType = -1;
					int actionWeight = 0;
					NbtTag nbtTag2;
					if (nbtCompound.TryGet("typ", out nbtTag2))
					{
						actionType = nbtTag2.IntValue;
					}
					if (nbtCompound.TryGet("wt", out nbtTag2))
					{
						actionWeight = nbtTag2.IntValue;
					}
					object[] array = null;
					NbtCompound nbtCompound2;
					if (nbtCompound.TryGet<NbtCompound>("parm", out nbtCompound2))
					{
						array = new object[nbtCompound2.Count];
						List<NbtTag> list = new List<NbtTag>(nbtCompound2.Tags);
						list.Sort((NbtTag x, NbtTag y) => x.Name.CompareTo(y.Name));
						for (int i = 0; i < list.Count; i++)
						{
							array[i] = this.GetPropertyObject(list[i]);
						}
					}
					ActionParams item = ActionParamsFactory.Build(actionType, actionWeight, array);
					this.actionList.Add(item);
				}
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005A24 File Offset: 0x00003C24
		public List<ActionParams> GetActionParams()
		{
			return this.actionList;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005A2C File Offset: 0x00003C2C
		public StatSet GetStatSet()
		{
			return new StatSet
			{
				Level = this.level,
				HP = this.hp,
				MaxHP = this.hp,
				PP = this.pp,
				MaxPP = this.pp,
				Offense = this.offense,
				Defense = this.defense,
				Speed = this.speed,
				Guts = this.guts,
				IQ = this.iq,
				Luck = this.luck
			};
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005AD1 File Offset: 0x00003CD1
		public Dictionary<string, string> GetContextDictionary()
		{
			return new Dictionary<string, string>(this.strings);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005AE0 File Offset: 0x00003CE0
		public string GetStringQualifiedName(string stringName)
		{
			string empty;
			if (!this.strings.TryGetValue(stringName, out empty))
			{
				empty = string.Empty;
			}
			return empty;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005B04 File Offset: 0x00003D04
		public bool TryGetStringQualifiedName(string stringName, out string qualifiedName)
		{
			return this.strings.TryGetValue(stringName, out qualifiedName);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005B14 File Offset: 0x00003D14
		public object GetAiProperty(string propertyName)
		{
			object result = null;
			if (!this.aiProperties.TryGetValue(propertyName, out result))
			{
				string message = string.Format("The AI property \"{0}\" is not present.", propertyName);
				throw new KeyNotFoundException(message);
			}
			return result;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005B47 File Offset: 0x00003D47
		public bool TryGetAiProperty(string propertyName, out object value)
		{
			return this.aiProperties.TryGetValue(propertyName, out value);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005B58 File Offset: 0x00003D58
		public bool TryGetAiProperty<T>(string propertyName, out T value)
		{
			object obj;
			bool result = this.aiProperties.TryGetValue(propertyName, out obj);
			value = (T)((object)obj);
			return result;
		}

		// Token: 0x0400014C RID: 332
		public const string STRING_DEFEAT = "defeat";

		// Token: 0x0400014D RID: 333
		public const string STRING_ENCOUNTER = "encounter";

		// Token: 0x0400014E RID: 334
		public const string STRING_NAME = "name";

		// Token: 0x0400014F RID: 335
		public const string STRING_TELEPATHY = "telepathy";

		// Token: 0x04000150 RID: 336
		public const string STRING_THOUGHTS = "thoughts";

		// Token: 0x04000151 RID: 337
		private const string STATS_TAG = "stat";

		// Token: 0x04000152 RID: 338
		private const string MODIFIERS_TAG = "mod";

		// Token: 0x04000153 RID: 339
		private const string STRINGS_TAG = "str";

		// Token: 0x04000154 RID: 340
		private const string BEHAVIOR_TAG = "bhv";

		// Token: 0x04000155 RID: 341
		private const string ACTIONS_TAG = "act";

		// Token: 0x04000156 RID: 342
		private const string MOVER_TAG = "mov";

		// Token: 0x04000157 RID: 343
		private const string EXPERIENCE_TAG = "exp";

		// Token: 0x04000158 RID: 344
		private const string REWARD_TAG = "rwd";

		// Token: 0x04000159 RID: 345
		private const string BBG_TAG = "bbg";

		// Token: 0x0400015A RID: 346
		private const string BGM_TAG = "bgm";

		// Token: 0x0400015B RID: 347
		private const string SPRITE_TAG = "spr";

		// Token: 0x0400015C RID: 348
		private const string OPTION_TAG = "opt";

		// Token: 0x0400015D RID: 349
		private const string IMMUNITIES_TAG = "imm";

		// Token: 0x0400015E RID: 350
		private const string LEVEL_TAG = "lvl";

		// Token: 0x0400015F RID: 351
		private const string HP_TAG = "hp";

		// Token: 0x04000160 RID: 352
		private const string PP_TAG = "pp";

		// Token: 0x04000161 RID: 353
		private const string OFFENSE_TAG = "off";

		// Token: 0x04000162 RID: 354
		private const string DEFENSE_TAG = "def";

		// Token: 0x04000163 RID: 355
		private const string SPEED_TAG = "spd";

		// Token: 0x04000164 RID: 356
		private const string GUTS_TAG = "gut";

		// Token: 0x04000165 RID: 357
		private const string IQ_TAG = "iq";

		// Token: 0x04000166 RID: 358
		private const string LUCK_TAG = "lck";

		// Token: 0x04000167 RID: 359
		private const string MOD_ELEC_TAG = "elec";

		// Token: 0x04000168 RID: 360
		private const string MOD_EXPL_TAG = "expl";

		// Token: 0x04000169 RID: 361
		private const string MOD_FIRE_TAG = "fire";

		// Token: 0x0400016A RID: 362
		private const string MOD_ICE_TAG = "ice";

		// Token: 0x0400016B RID: 363
		private const string MOD_NAUS_TAG = "naus";

		// Token: 0x0400016C RID: 364
		private const string MOD_PHYS_TAG = "phys";

		// Token: 0x0400016D RID: 365
		private const string MOD_POIS_TAG = "pois";

		// Token: 0x0400016E RID: 366
		private const string MOD_WET_TAG = "wet";

		// Token: 0x0400016F RID: 367
		private const string AI_TAG = "ai";

		// Token: 0x04000170 RID: 368
		private const string PARAMETERS_TAG = "parm";

		// Token: 0x04000171 RID: 369
		private const string TYPE_TAG = "typ";

		// Token: 0x04000172 RID: 370
		private const string WEIGHT_TAG = "wt";

		// Token: 0x04000173 RID: 371
		private string qualifiedName;

		// Token: 0x04000174 RID: 372
		private string bbgName;

		// Token: 0x04000175 RID: 373
		private string bgmName;

		// Token: 0x04000176 RID: 374
		private string spriteName;

		// Token: 0x04000177 RID: 375
		private int aiType;

		// Token: 0x04000178 RID: 376
		private Dictionary<string, object> aiProperties;

		// Token: 0x04000179 RID: 377
		private List<ActionParams> actionList;

		// Token: 0x0400017A RID: 378
		private int experience;

		// Token: 0x0400017B RID: 379
		private int reward;

		// Token: 0x0400017C RID: 380
		private int moverType;

		// Token: 0x0400017D RID: 381
		private EnemyOptions options;

		// Token: 0x0400017E RID: 382
		private EnemyImmunities immunities;

		// Token: 0x0400017F RID: 383
		private int level;

		// Token: 0x04000180 RID: 384
		private int hp;

		// Token: 0x04000181 RID: 385
		private int pp;

		// Token: 0x04000182 RID: 386
		private int offense;

		// Token: 0x04000183 RID: 387
		private int defense;

		// Token: 0x04000184 RID: 388
		private int speed;

		// Token: 0x04000185 RID: 389
		private int guts;

		// Token: 0x04000186 RID: 390
		private int iq;

		// Token: 0x04000187 RID: 391
		private int luck;

		// Token: 0x04000188 RID: 392
		private float modElectric;

		// Token: 0x04000189 RID: 393
		private float modExplosive;

		// Token: 0x0400018A RID: 394
		private float modFire;

		// Token: 0x0400018B RID: 395
		private float modIce;

		// Token: 0x0400018C RID: 396
		private float modNausea;

		// Token: 0x0400018D RID: 397
		private float modPhysical;

		// Token: 0x0400018E RID: 398
		private float modPoison;

		// Token: 0x0400018F RID: 399
		private float modWater;

		// Token: 0x04000190 RID: 400
		private Dictionary<string, string> strings;
	}
}
