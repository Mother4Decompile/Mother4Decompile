using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Carbine.Flags;
using Carbine.Utility;
using Mother4.Data;
using Mother4.GUI.Text.PrintActions;
using Mother4.Scripts.Actions.ParamTypes;
using Mother4.Utility;
using Rufini.Strings;
using SFML.Graphics;

namespace Mother4.GUI.Text
{
	// Token: 0x0200009A RID: 154
	internal class TextProcessor
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00014745 File Offset: 0x00012945
		public IList<PrintAction> Actions
		{
			get
			{
				return this.Process();
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0001474D File Offset: 0x0001294D
		public TextProcessor(string text, Dictionary<string, string> contextualReplacements) : this(text)
		{
			this.contextualDict = contextualReplacements;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0001475D File Offset: 0x0001295D
		public TextProcessor(string text)
		{
			this.text = text;
			this.state = TextProcessor.ReadState.Text;
			this.actions = new List<PrintAction>();
			this.readIndex = 0;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00014785 File Offset: 0x00012985
		public static string ProcessReplacements(string text)
		{
			return TextProcessor.ProcessReplacements(text, null);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00014790 File Offset: 0x00012990
		public static string ProcessReplacements(string text, Dictionary<string, string> contextualReplacements)
		{
			StringBuilder stringBuilder = new StringBuilder(text ?? string.Empty);
			stringBuilder = stringBuilder.Replace("\r", "");
			int num = 0;
			MatchCollection matchCollection = Regex.Matches(stringBuilder.ToString(), "\\[([a-zA-Z][a-zA-Z0-9]*):?(\\b[^\\]]*)\\]");
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				string value = match.Groups[1].Value;
				string value2 = match.Groups[2].Value;
				string[] array = value2.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].Trim();
				}
				string text2 = null;
				string key;
				switch (key = value)
				{
				case "cn":
				{
					CharacterType character;
					Enum.TryParse<CharacterType>(value2, out character);
					text2 = CharacterNames.GetName(character);
					break;
				}
				case "travis":
					text2 = CharacterNames.GetName(CharacterType.Travis);
					break;
				case "floyd":
					text2 = CharacterNames.GetName(CharacterType.Floyd);
					break;
				case "meryl":
					text2 = CharacterNames.GetName(CharacterType.Meryl);
					break;
				case "leo":
					text2 = CharacterNames.GetName(CharacterType.Leo);
					break;
				case "zack":
					text2 = CharacterNames.GetName(CharacterType.Zack);
					break;
				case "renee":
					text2 = CharacterNames.GetName(CharacterType.Renee);
					break;
				case "leader":
				{
					CharacterType character2 = PartyManager.Instance[0];
					text2 = CharacterNames.GetName(character2);
					break;
				}
				case "party":
					text2 = CharacterNames.GetGroup(PartyManager.Instance.ToArray());
					break;
				case "money":
				{
					RufiniString rufiniString = StringFile.Instance.Get("system.currency");
					int num3 = ValueManager.Instance[1];
					text2 = string.Format("{0}{1}", rufiniString.Value ?? string.Empty, num3);
					break;
				}
				case "str":
				{
					text2 = string.Empty;
					RufiniString rufiniString2 = StringFile.Instance.Get(array[0]);
					if (rufiniString2.Value != null)
					{
						text2 = rufiniString2.Value;
					}
					break;
				}
				case "ctx":
				{
					text2 = string.Empty;
					string text3;
					if (contextualReplacements != null && array.Length > 0 && contextualReplacements.TryGetValue(array[0], out text3))
					{
						RufiniString rufiniString3 = StringFile.Instance.Get(text3);
						if (rufiniString3.Value != null)
						{
							if (array.Length > 1 && array[1] == "true")
							{
								text2 = Capitalizer.Capitalize(rufiniString3.Value);
							}
							else
							{
								text2 = rufiniString3.Value;
							}
						}
						else
						{
							text2 = text3;
						}
					}
					break;
				}
				}
				if (text2 != null)
				{
					int num4 = match.Index - num;
					stringBuilder = stringBuilder.Remove(num4, match.Length);
					stringBuilder = stringBuilder.Insert(num4, text2);
					num += match.Length - text2.Length;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00014B64 File Offset: 0x00012D64
		private void AddAction(PrintAction action)
		{
			this.actions.Add(action);
			string text;
			if (action.Data is object[])
			{
				object[] array = (object[])action.Data;
				text = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					text = text + "\"" + array[i].ToString() + "\", ";
				}
			}
			else
			{
				text = action.Data.ToString();
			}
			Console.WriteLine("Added print action:\n\tType:\t{0}\n\tData:\t{1}", Enum.GetName(typeof(PrintActionType), action.Type), text);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00014BFC File Offset: 0x00012DFC
		private void ProcessText()
		{
			int num = this.text.IndexOf('[', this.readIndex);
			if (num == -1)
			{
				num = this.text.Length;
			}
			while (this.readIndex < num)
			{
				int num2 = this.text.IndexOf('\n', this.readIndex);
				bool flag = true;
				if (num2 == -1 || num2 > num)
				{
					num2 = num;
					flag = false;
				}
				if (num2 - this.readIndex > 0)
				{
					PrintAction action = new PrintAction(PrintActionType.PrintText, this.text.Substring(this.readIndex, num2 - this.readIndex));
					this.AddAction(action);
				}
				if (flag)
				{
					PrintAction action2 = new PrintAction(PrintActionType.LineBreak, new object[0]);
					this.AddAction(action2);
				}
				this.readIndex = num2 + 1;
			}
			this.readIndex = num;
			this.state = TextProcessor.ReadState.Tag;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00014CC0 File Offset: 0x00012EC0
		private void AddActionByTagName(string tagName, string[] args)
		{
			PrintAction? printAction = null;
			switch (tagName)
			{
			case "p":
			{
				int num2 = 0;
				int.TryParse(args[0], out num2);
				printAction = new PrintAction?(new PrintAction(PrintActionType.Pause, num2));
				goto IL_1FD;
			}
			case "t":
				printAction = new PrintAction?(new PrintAction(PrintActionType.Trigger, args));
				goto IL_1FD;
			case "g":
				printAction = new PrintAction?(new PrintAction(PrintActionType.PrintGraphic, args[0]));
				goto IL_1FD;
			case "c":
			{
				Color color = ColorHelper.FromHexString(args[0]);
				printAction = new PrintAction?(new PrintAction(PrintActionType.Color, color));
				goto IL_1FD;
			}
			case "b":
				printAction = new PrintAction?(new PrintAction(PrintActionType.Prompt, new object[0]));
				goto IL_1FD;
			case "q":
				printAction = new PrintAction?(new PrintAction(PrintActionType.PromptQuestion, args));
				goto IL_1FD;
			case "i":
			{
				int num3 = 0;
				int.TryParse(args[0], out num3);
				int num4 = 0;
				int.TryParse(args[1], out num4);
				printAction = new PrintAction?(new PrintAction(PrintActionType.PromptNumeric, new object[]
				{
					num3,
					num4
				}));
				goto IL_1FD;
			}
			case "l":
				printAction = new PrintAction?(new PrintAction(PrintActionType.PromptList, args));
				goto IL_1FD;
			case "ts":
			{
				int num5 = 0;
				int.TryParse(args[0], out num5);
				printAction = new PrintAction?(new PrintAction(PrintActionType.Sound, num5));
				goto IL_1FD;
			}
			}
			Console.WriteLine("UNKNOWN TAG: {0}", tagName);
			IL_1FD:
			if (printAction != null)
			{
				this.AddAction(printAction.Value);
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00014EE0 File Offset: 0x000130E0
		private void ProcessTag()
		{
			int num = this.text.IndexOf(']', this.readIndex);
			if (num == -1)
			{
				num = this.text.Length;
			}
			this.readIndex++;
			int num2 = this.text.IndexOf(':', this.readIndex);
			string tagName;
			string[] args;
			if (num2 != -1)
			{
				tagName = this.text.Substring(this.readIndex, num2 - this.readIndex);
				string text = this.text.Substring(num2 + 1, num - (num2 + 1));
				args = (text.Contains(',') ? text.Split(new char[]
				{
					','
				}) : new string[]
				{
					text
				});
			}
			else
			{
				tagName = this.text.Substring(this.readIndex, num - this.readIndex);
				args = null;
			}
			this.AddActionByTagName(tagName, args);
			this.readIndex = num + 1;
			this.state = TextProcessor.ReadState.Text;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00014FD4 File Offset: 0x000131D4
		public IList<PrintAction> Process()
		{
			if (this.state != TextProcessor.ReadState.Done)
			{
				this.text = TextProcessor.ProcessReplacements(this.text, this.contextualDict);
				while (this.readIndex < this.text.Length)
				{
					switch (this.state)
					{
					case TextProcessor.ReadState.Text:
						this.ProcessText();
						break;
					case TextProcessor.ReadState.Tag:
						this.ProcessTag();
						break;
					}
				}
				this.state = TextProcessor.ReadState.Done;
			}
			return this.actions;
		}

		// Token: 0x040004AD RID: 1197
		private const char NEWLINE_CHAR = '\n';

		// Token: 0x040004AE RID: 1198
		private const char BULLET_CHAR = '@';

		// Token: 0x040004AF RID: 1199
		private const char SPACE_CHAR = ' ';

		// Token: 0x040004B0 RID: 1200
		private const int PAUSE_CHAR_DURATION = 10;

		// Token: 0x040004B1 RID: 1201
		private const int BULLET_PAUSE_DURATION = 30;

		// Token: 0x040004B2 RID: 1202
		private const string SINGLE_CMD_REGEX = "\\[([a-zA-Z][a-zA-Z0-9]*):?(\\b[^\\]]*)\\]";

		// Token: 0x040004B3 RID: 1203
		private const string TRUE_STRING = "true";

		// Token: 0x040004B4 RID: 1204
		private const char TAG_START = '[';

		// Token: 0x040004B5 RID: 1205
		private const char TAG_END = ']';

		// Token: 0x040004B6 RID: 1206
		private const char TAG_COLON = ':';

		// Token: 0x040004B7 RID: 1207
		private const char TAG_SEPARATOR_CHAR = ',';

		// Token: 0x040004B8 RID: 1208
		private const string CMD_PAUSE = "p";

		// Token: 0x040004B9 RID: 1209
		private const string CMD_TRIGGER = "t";

		// Token: 0x040004BA RID: 1210
		private const string CMD_GRAPHIC = "g";

		// Token: 0x040004BB RID: 1211
		private const string CMD_COLOR = "c";

		// Token: 0x040004BC RID: 1212
		private const string CMD_PROMPT = "b";

		// Token: 0x040004BD RID: 1213
		private const string CMD_QUESTION = "q";

		// Token: 0x040004BE RID: 1214
		private const string CMD_INPUT = "i";

		// Token: 0x040004BF RID: 1215
		private const string CMD_LIST = "l";

		// Token: 0x040004C0 RID: 1216
		private const string CMD_SOUND = "ts";

		// Token: 0x040004C1 RID: 1217
		private const string CMD_CHARNAME = "cn";

		// Token: 0x040004C2 RID: 1218
		private const string CMD_TRAVIS = "travis";

		// Token: 0x040004C3 RID: 1219
		private const string CMD_FLOYD = "floyd";

		// Token: 0x040004C4 RID: 1220
		private const string CMD_MERYL = "meryl";

		// Token: 0x040004C5 RID: 1221
		private const string CMD_LEO = "leo";

		// Token: 0x040004C6 RID: 1222
		private const string CMD_ZACK = "zack";

		// Token: 0x040004C7 RID: 1223
		private const string CMD_RENEE = "renee";

		// Token: 0x040004C8 RID: 1224
		private const string CMD_PARTY_LEAD = "leader";

		// Token: 0x040004C9 RID: 1225
		private const string CMD_PARTY_GROUP = "party";

		// Token: 0x040004CA RID: 1226
		private const string CMD_MONEY = "money";

		// Token: 0x040004CB RID: 1227
		private const string CMD_STRING = "str";

		// Token: 0x040004CC RID: 1228
		private const string CMD_CONTEXTUAL = "ctx";

		// Token: 0x040004CD RID: 1229
		private string text;

		// Token: 0x040004CE RID: 1230
		private TextProcessor.ReadState state;

		// Token: 0x040004CF RID: 1231
		private IList<PrintAction> actions;

		// Token: 0x040004D0 RID: 1232
		private int readIndex;

		// Token: 0x040004D1 RID: 1233
		private Dictionary<string, string> contextualDict;

		// Token: 0x0200009B RID: 155
		private enum ReadState
		{
			// Token: 0x040004D3 RID: 1235
			Text,
			// Token: 0x040004D4 RID: 1236
			Tag,
			// Token: 0x040004D5 RID: 1237
			Done
		}
	}
}
