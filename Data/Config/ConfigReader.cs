using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Mother4.Utility;
using SFML.System;

namespace Mother4.Data.Config
{
	// Token: 0x02000022 RID: 34
	internal class ConfigReader
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00004FE2 File Offset: 0x000031E2
		public static ConfigReader Instance
		{
			get
			{
				if (ConfigReader.instance == null)
				{
					ConfigReader.instance = new ConfigReader();
				}
				return ConfigReader.instance;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00004FFA File Offset: 0x000031FA
		public string StartingMapName
		{
			get
			{
				return this.startingMap;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00005002 File Offset: 0x00003202
		public Vector2i StartingPosition
		{
			get
			{
				return this.startingPosition;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000500A File Offset: 0x0000320A
		public CharacterType[] StartingParty
		{
			get
			{
				return this.partyList.ToArray();
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005017 File Offset: 0x00003217
		public ConfigReader()
		{
			this.stateStack = new Stack<ConfigReader.ReadState>();
			this.stateStack.Push(ConfigReader.ReadState.Root);
			this.partyList = new List<CharacterType>();
			this.Load();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005048 File Offset: 0x00003248
		private void ReadStartElement(XmlTextReader reader)
		{
			string name;
			if ((name = reader.Name) != null)
			{
				if (!(name == "map"))
				{
					if (!(name == "position"))
					{
						return;
					}
					while (reader.MoveToNextAttribute())
					{
						string name2;
						if ((name2 = reader.Name) != null)
						{
							if (!(name2 == "x"))
							{
								if (name2 == "y")
								{
									int.TryParse(reader.Value, out this.startingPosition.Y);
								}
							}
							else
							{
								int.TryParse(reader.Value, out this.startingPosition.X);
							}
						}
					}
				}
				else if (reader.MoveToNextAttribute() && reader.Name == "value")
				{
					this.startingMap = reader.Value;
					return;
				}
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005104 File Offset: 0x00003304
		private void ReadPartyElement(XmlTextReader reader)
		{
			string name;
			if ((name = reader.Name) != null)
			{
				if (!(name == "character"))
				{
					return;
				}
				string value;
				if (reader.MoveToNextAttribute() && reader.Name == "id" && (value = reader.Value) != null)
				{
					if (value == "travis")
					{
						this.partyList.Add(CharacterType.Travis);
						return;
					}
					if (value == "floyd")
					{
						this.partyList.Add(CharacterType.Floyd);
						return;
					}
					if (value == "meryl")
					{
						this.partyList.Add(CharacterType.Meryl);
						return;
					}
					if (value == "leo")
					{
						this.partyList.Add(CharacterType.Leo);
						return;
					}
					if (value == "zack")
					{
						this.partyList.Add(CharacterType.Zack);
						return;
					}
					if (!(value == "renee"))
					{
						return;
					}
					this.partyList.Add(CharacterType.Renee);
				}
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005210 File Offset: 0x00003410
		private void HandleInnerElement(XmlTextReader reader)
		{
			switch (this.stateStack.Peek())
			{
			case ConfigReader.ReadState.Start:
				this.ReadStartElement(reader);
				return;
			case ConfigReader.ReadState.Party:
				this.ReadPartyElement(reader);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000524C File Offset: 0x0000344C
		private void HandleElement(XmlTextReader reader)
		{
			string name;
			if ((name = reader.Name) != null)
			{
				if (name == "start")
				{
					this.stateStack.Push(ConfigReader.ReadState.Start);
					return;
				}
				if (name == "party")
				{
					this.stateStack.Push(ConfigReader.ReadState.Party);
					return;
				}
			}
			this.HandleInnerElement(reader);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000052A0 File Offset: 0x000034A0
		private void HandleEndElement(XmlTextReader reader)
		{
			this.stateStack.Pop();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000052B0 File Offset: 0x000034B0
		private void Load()
		{
			using (Stream stream = EmbeddedResources.GetStream("Mother4.Resources.config.xml"))
			{
				XmlTextReader xmlTextReader = new XmlTextReader(stream);
				while (xmlTextReader.Read())
				{
					XmlNodeType nodeType = xmlTextReader.NodeType;
					if (nodeType == XmlNodeType.Element)
					{
						this.HandleElement(xmlTextReader);
					}
				}
			}
		}

		// Token: 0x04000139 RID: 313
		private const string TAG_NAME_START = "start";

		// Token: 0x0400013A RID: 314
		private const string TAG_NAME_START_MAP = "map";

		// Token: 0x0400013B RID: 315
		private const string TAG_NAME_START_POSITION = "position";

		// Token: 0x0400013C RID: 316
		private const string TAG_NAME_PARTY = "party";

		// Token: 0x0400013D RID: 317
		private const string TAG_NAME_PARTY_CHARACTER = "character";

		// Token: 0x0400013E RID: 318
		private const string ATTR_NAME_VALUE = "value";

		// Token: 0x0400013F RID: 319
		private const string ATTR_NAME_ID = "id";

		// Token: 0x04000140 RID: 320
		private const string ATTR_NAME_X = "x";

		// Token: 0x04000141 RID: 321
		private const string ATTR_NAME_Y = "y";

		// Token: 0x04000142 RID: 322
		private static ConfigReader instance;

		// Token: 0x04000143 RID: 323
		private Stack<ConfigReader.ReadState> stateStack;

		// Token: 0x04000144 RID: 324
		private string startingMap;

		// Token: 0x04000145 RID: 325
		private Vector2i startingPosition;

		// Token: 0x04000146 RID: 326
		private List<CharacterType> partyList;

		// Token: 0x02000023 RID: 35
		private enum ReadState
		{
			// Token: 0x04000148 RID: 328
			Root,
			// Token: 0x04000149 RID: 329
			BaseStats,
			// Token: 0x0400014A RID: 330
			Start,
			// Token: 0x0400014B RID: 331
			Party
		}
	}
}
