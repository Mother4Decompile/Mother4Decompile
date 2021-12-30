using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Carbine.Utility;
using Mother4.Data;
using Mother4.Data.Character;
using Mother4.Data.Config;
using Mother4.GUI.NamingMenu;
using Rufini.Strings;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x0200010B RID: 267
	internal class NamingScene : StandardScene
	{
		// Token: 0x06000640 RID: 1600 RVA: 0x000257E0 File Offset: 0x000239E0
		public NamingScene()
		{
			this.characterIndex = 0;
			this.workingName = string.Empty;
			this.CreateBackgroundVerts();
			this.CreateWindow();
			this.CreateNameWindow();
			this.SetupSounds();
			this.PopulateDontCareNames();
			RectangleShape rectangleShape = new RectangleShape(new Vector2f(320f, this.namingPanel.Size.Y - 2f));
			rectangleShape.FillColor = new Color(0, 0, 0, 68);
			this.accentBar = new ShapeGraphic(rectangleShape, new Vector2f(0f, this.namingPanel.Position.Y + 12f), VectorMath.ZERO_VECTOR, rectangleShape.Size, -1);
			this.pipeline.Add(this.accentBar);
			this.namingCharacter = new NamingCharacter(NamingScene.CHARACTER_ORDER[this.characterIndex], 1);
			this.pipeline.Add(this.namingCharacter);
			for (int i = 0; i < NamingScene.CHARACTER_ORDER.Length; i++)
			{
				CharacterNames.SetName(NamingScene.CHARACTER_ORDER[i], string.Empty);
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00025900 File Offset: 0x00023B00
		private void PopulateDontCareNames()
		{
			this.dontCareNames = new Dictionary<CharacterType, List<string>>();
			for (int i = 0; i < NamingScene.CHARACTER_ORDER.Length; i++)
			{
				CharacterType characterType = NamingScene.CHARACTER_ORDER[i];
				List<string> list = new List<string>();
				this.dontCareNames.Add(characterType, list);
				for (int j = 0; j < 6; j++)
				{
					CharacterData data = CharacterFile.Instance.GetData(characterType);
					string arg = data.QualifiedName.Substring(data.QualifiedName.LastIndexOf('.') + 1);
					string[] nameParts = new string[]
					{
						"naming",
						"defaults",
						arg + (j + 1)
					};
					string value = StringFile.Instance.Get(nameParts).Value;
					if (value != null && value.Length > 0)
					{
						list.Add(value);
					}
				}
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000259F0 File Offset: 0x00023BF0
		private void CreateBackgroundVerts()
		{
			uint vertexCount = 4U * (uint)Math.Ceiling(32.0) * (uint)Math.Ceiling(18.0);
			this.backVerts = new VertexArray(PrimitiveType.Quads, vertexCount);
			bool flag = true;
			uint num = 0U;
			int num2 = 0;
			int num3 = 0;
			while ((long)num3 < 180L)
			{
				int num4 = 0;
				while ((long)num4 < 320L)
				{
					Color color = NamingScene.CHARACTER_COLORS[NamingScene.CHARACTER_ORDER[this.characterIndex]][num2 % 2];
					this.backVerts[num++] = new Vertex(new Vector2f((float)num4, (float)num3), color);
					this.backVerts[num++] = new Vertex(new Vector2f((float)(num4 + 10), (float)num3), color);
					this.backVerts[num++] = new Vertex(new Vector2f((float)(num4 + 10), (float)(num3 + 10)), color);
					this.backVerts[num++] = new Vertex(new Vector2f((float)num4, (float)(num3 + 10)), color);
					num2++;
					num4 += 10;
				}
				if (flag)
				{
					num2++;
				}
				num3 += 10;
			}
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00025B39 File Offset: 0x00023D39
		private void CreateWindow()
		{
			this.inputPanel = new TextInputPanel(new Vector2f(8f, 64f), new Vector2f(288f, 92f));
			this.pipeline.Add(this.inputPanel);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00025B78 File Offset: 0x00023D78
		private void CreateNameWindow()
		{
			this.namingPanel = new NamingPanel(new Vector2f(128f, 8f), new Vector2f(168f, 32f));
			this.pipeline.Add(this.namingPanel);
			this.namingPanel.Description = StringFile.Instance.Get(NamingScene.CHARACTER_DESCRIPTIONS[NamingScene.CHARACTER_ORDER[this.characterIndex]]).Value;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00025BFC File Offset: 0x00023DFC
		private void SetupSounds()
		{
			this.sfxConfirm = AudioManager.Instance.Use(Paths.AUDIO + "confirm.wav", AudioType.Sound);
			this.sfxCancel = AudioManager.Instance.Use(Paths.AUDIO + "cancel.wav", AudioType.Sound);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00025C49 File Offset: 0x00023E49
		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			this.inputPanel.AxisPressed(axis);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00025C58 File Offset: 0x00023E58
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (this.characterTransition)
			{
				return;
			}
			if (b == Button.Start)
			{
				this.AdvanceScreen();
				this.sfxConfirm.Play();
				return;
			}
			object obj = this.inputPanel.ButtonPressed(b);
			if (obj is char)
			{
				char c = (char)obj;
				if (c == '\b')
				{
					if (this.workingName.Length > 0)
					{
						this.workingName = this.workingName.Remove(this.workingName.Length - 1);
						this.sfxCancel.Play();
					}
					else if (this.characterIndex > 0)
					{
						this.StartCharacterTransition(this.characterIndex - 1);
						this.sfxCancel.Play();
					}
				}
				else if (c == '\n')
				{
					this.AdvanceScreen();
					this.sfxConfirm.Play();
				}
				else if (c == '\r')
				{
					this.UseDontCareName();
					this.sfxConfirm.Play();
				}
				else if (c == '\t')
				{
					this.sfxConfirm.Play();
				}
				else if (c != '\0')
				{
					this.workingName += c;
					this.namingPanel.Name = this.workingName;
					if (this.namingPanel.NameWidth > 47)
					{
						this.workingName = this.workingName.Remove(this.workingName.Length - 1);
					}
					else
					{
						this.sfxConfirm.Play();
					}
				}
				this.namingPanel.Name = this.workingName;
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00025DC8 File Offset: 0x00023FC8
		public override void Focus()
		{
			base.Focus();
			AudioManager.Instance.SetBGM(Paths.AUDIO + "hint.mp3");
			AudioManager.Instance.BGM.Play();
			ViewManager.Instance.Center = Engine.HALF_SCREEN_SIZE;
			Engine.ClearColor = Color.Black;
			InputManager.Instance.AxisPressed += this.AxisPressed;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00025E48 File Offset: 0x00024048
		public override void Unfocus()
		{
			base.Unfocus();
			AudioManager.Instance.FadeOut(AudioManager.Instance.BGM, 500U);
			InputManager.Instance.AxisPressed -= this.AxisPressed;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00025EA0 File Offset: 0x000240A0
		private void UseDontCareName()
		{
			List<string> list = null;
			this.dontCareNames.TryGetValue(NamingScene.CHARACTER_ORDER[this.characterIndex], out list);
			if (list != null && list.Count > 0)
			{
				this.workingName = list[this.dontCareCounter];
				this.namingPanel.Name = this.workingName;
				this.dontCareCounter = (this.dontCareCounter + 1) % list.Count;
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00025F18 File Offset: 0x00024118
		private void AdvanceScreen()
		{
			if (this.characterIndex + 1 < NamingScene.CHARACTER_ORDER.Length)
			{
				this.dontCareCounter = 0;
				this.StartCharacterTransition(this.characterIndex + 1);
				return;
			}
			string startingMapName = ConfigReader.Instance.StartingMapName;
			Vector2f initialPosition = (Vector2f)ConfigReader.Instance.StartingPosition;
			PartyManager.Instance.Clear();
			PartyManager.Instance.AddAll(ConfigReader.Instance.StartingParty);
			OverworldScene newScene = new OverworldScene(startingMapName, initialPosition, 6, false, false, true);
			SceneManager.Instance.Transition = new ColorFadeTransition(3f, Color.Black);
			SceneManager.Instance.Push(newScene, true);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00025FB8 File Offset: 0x000241B8
		private void StartCharacterTransition(int nextCharacterIndex)
		{
			this.characterTransition = true;
			this.nextCharacterIndex = nextCharacterIndex;
			this.namingPanel.Description = string.Empty;
			this.namingPanel.Name = string.Empty;
			this.inputPanel.CursorVisibility = false;
			CharacterNames.SetName(NamingScene.CHARACTER_ORDER[this.characterIndex], this.workingName);
			this.workingName = string.Empty;
			this.namingCharacter.SwitchCharacters(NamingScene.CHARACTER_ORDER[this.nextCharacterIndex]);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0002604C File Offset: 0x0002424C
		private void TransitionCharacters(int nextCharacterIndex)
		{
			if (this.colorTransition <= 1f)
			{
				uint num = 0U;
				int num2 = 0;
				int num3 = 0;
				while ((long)num3 < 180L)
				{
					int num4 = 0;
					while ((long)num4 < 320L)
					{
						Color col = NamingScene.CHARACTER_COLORS[NamingScene.CHARACTER_ORDER[this.characterIndex]][num2 % 2];
						Color col2 = NamingScene.CHARACTER_COLORS[NamingScene.CHARACTER_ORDER[nextCharacterIndex]][num2 % 2];
						Color color = ColorHelper.Blend(col, col2, this.colorTransition);
						this.backVerts[num++] = new Vertex(new Vector2f((float)num4, (float)num3), color);
						this.backVerts[num++] = new Vertex(new Vector2f((float)(num4 + 10), (float)num3), color);
						this.backVerts[num++] = new Vertex(new Vector2f((float)(num4 + 10), (float)(num3 + 10)), color);
						this.backVerts[num++] = new Vertex(new Vector2f((float)num4, (float)(num3 + 10)), color);
						num2++;
						num4 += 10;
					}
					num2++;
					num3 += 10;
				}
				this.colorTransition += 0.01f;
			}
			if (this.transitionTimer < 200)
			{
				this.transitionTimer++;
				return;
			}
			this.colorTransition = 0f;
			this.transitionTimer = 0;
			this.characterTransition = false;
			this.characterIndex = nextCharacterIndex;
			this.inputPanel.CursorVisibility = true;
			this.namingPanel.Description = StringFile.Instance.Get(NamingScene.CHARACTER_DESCRIPTIONS[NamingScene.CHARACTER_ORDER[this.characterIndex]]).Value;
			this.workingName = CharacterNames.GetName(NamingScene.CHARACTER_ORDER[this.characterIndex]);
			this.namingPanel.Name = this.workingName;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00026263 File Offset: 0x00024463
		public override void Update()
		{
			base.Update();
			this.namingCharacter.Update();
			if (this.characterTransition)
			{
				this.TransitionCharacters(this.nextCharacterIndex);
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0002628A File Offset: 0x0002448A
		public override void Draw()
		{
			this.pipeline.Target.Draw(this.backVerts);
			base.Draw();
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000262A8 File Offset: 0x000244A8
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.namingPanel.Dispose();
					this.inputPanel.Dispose();
				}
				AudioManager.Instance.Unuse(this.sfxConfirm);
				AudioManager.Instance.Unuse(this.sfxCancel);
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000815 RID: 2069
		private const int MAX_NAME_WIDTH = 47;

		// Token: 0x04000816 RID: 2070
		private const int GRID_SIZE = 10;

		// Token: 0x04000817 RID: 2071
		private const int DONT_CARE_NAME_COUNT = 6;

		// Token: 0x04000818 RID: 2072
		private VertexArray backVerts;

		// Token: 0x04000819 RID: 2073
		private NamingPanel namingPanel;

		// Token: 0x0400081A RID: 2074
		private TextInputPanel inputPanel;

		// Token: 0x0400081B RID: 2075
		private NamingCharacter namingCharacter;

		// Token: 0x0400081C RID: 2076
		private ShapeGraphic accentBar;

		// Token: 0x0400081D RID: 2077
		private CarbineSound sfxConfirm;

		// Token: 0x0400081E RID: 2078
		private CarbineSound sfxCancel;

		// Token: 0x0400081F RID: 2079
		private string workingName;

		// Token: 0x04000820 RID: 2080
		private int characterIndex;

		// Token: 0x04000821 RID: 2081
		private static readonly Dictionary<CharacterType, Color[]> CHARACTER_COLORS = new Dictionary<CharacterType, Color[]>
		{
			{
				CharacterType.Travis,
				new Color[]
				{
					ColorHelper.FromInt(4293456641U),
					ColorHelper.FromInt(4282245697U)
				}
			},
			{
				CharacterType.Floyd,
				new Color[]
				{
					ColorHelper.FromInt(4294961898U),
					ColorHelper.FromInt(4293335064U)
				}
			},
			{
				CharacterType.Meryl,
				new Color[]
				{
					ColorHelper.FromInt(4294941695U),
					ColorHelper.FromInt(4292237800U)
				}
			},
			{
				CharacterType.Leo,
				new Color[]
				{
					ColorHelper.FromInt(4281348176U),
					ColorHelper.FromInt(4290164435U)
				}
			}
		};

		// Token: 0x04000822 RID: 2082
		private static readonly Dictionary<CharacterType, string> CHARACTER_DESCRIPTIONS = new Dictionary<CharacterType, string>
		{
			{
				CharacterType.Travis,
				"naming.travis"
			},
			{
				CharacterType.Floyd,
				"naming.floyd"
			},
			{
				CharacterType.Meryl,
				"naming.meryl"
			},
			{
				CharacterType.Leo,
				"naming.leo"
			}
		};

		// Token: 0x04000823 RID: 2083
		private static readonly CharacterType[] CHARACTER_ORDER = new CharacterType[]
		{
			CharacterType.Travis,
			CharacterType.Floyd,
			CharacterType.Meryl,
			CharacterType.Leo
		};

		// Token: 0x04000824 RID: 2084
		private bool characterTransition;

		// Token: 0x04000825 RID: 2085
		private int transitionTimer;

		// Token: 0x04000826 RID: 2086
		private int nextCharacterIndex;

		// Token: 0x04000827 RID: 2087
		private float colorTransition;

		// Token: 0x04000828 RID: 2088
		private int dontCareCounter;

		// Token: 0x04000829 RID: 2089
		private Dictionary<CharacterType, List<string>> dontCareNames;
	}
}
