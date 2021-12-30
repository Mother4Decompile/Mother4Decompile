using System;
using System.Collections.Generic;
using System.IO;
using Carbine;
using Carbine.Audio;
using Carbine.Flags;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Mother4.Data;
using Mother4.Data.Character;
using Mother4.GUI;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x02000109 RID: 265
	internal class MapTestSetupScene : StandardScene
	{
		// Token: 0x0600062A RID: 1578 RVA: 0x000249B4 File Offset: 0x00022BB4
		public MapTestSetupScene()
		{
			string[] files = Directory.GetFiles(Paths.MAPS, "*.dat");
			this.mapItems = new string[files.Length];
			for (int i = 0; i < files.Length; i++)
			{
				this.mapItems[i] = Path.GetFileName(files[i]);
			}
			this.selectedMap = this.mapItems[0];
			string[] items = new string[]
			{
				"Select Characters",
				string.Format("Select Map ({0})", this.selectedMap),
				"Map Test Options",
				"Start",
				"Back"
			};
			this.optionList = new ScrollingList(new Vector2f(32f, 80f), 0, items, 5, 16f, 80f, Paths.GRAPHICS + "cursor.dat");
			this.optionList.ShowSelectionRectangle = false;
			this.optionList.UseHighlightTextColor = false;
			this.pipeline.Add(this.optionList);
			this.focusedList = this.optionList;
			this.mapList = new ScrollingList(new Vector2f(32f, 80f), 0, this.mapItems, 5, 16f, 256f, Paths.GRAPHICS + "cursor.dat");
			this.mapList.ShowSelectionRectangle = false;
			this.mapList.UseHighlightTextColor = false;
			this.pipeline.Add(this.mapList);
			this.mapList.Hide();
			List<CharacterType> allCharacterTypes = CharacterFile.Instance.GetAllCharacterTypes();
			string[] array = new string[allCharacterTypes.Count + 1];
			array[0] = "Remove";
			for (int j = 0; j < allCharacterTypes.Count; j++)
			{
				array[j + 1] = CharacterNames.GetName(allCharacterTypes[j]);
			}
			this.selectedCharacters = new List<CharacterType>();
			this.selectedCharacters.Add(CharacterType.Travis);
			this.ResetCharacterGraphics();
			this.charactersList = new ScrollingList(new Vector2f(32f, 80f), 0, array, 5, 16f, 256f, Paths.GRAPHICS + "cursor.dat");
			this.charactersList.ShowSelectionRectangle = false;
			this.charactersList.UseHighlightTextColor = false;
			this.pipeline.Add(this.charactersList);
			this.charactersList.Hide();
			string[] items2 = new string[]
			{
				string.Format("Run scripts on map load: {0}", this.runScriptsOnLoad),
				string.Format("Start at night: {0}", FlagManager.Instance[1]),
				"Back"
			};
			this.settingsList = new ScrollingList(new Vector2f(32f, 80f), 0, items2, 5, 16f, 256f, Paths.GRAPHICS + "cursor.dat");
			this.settingsList.ShowSelectionRectangle = false;
			this.settingsList.UseHighlightTextColor = false;
			this.pipeline.Add(this.settingsList);
			this.settingsList.Hide();
			this.titleText = new TextRegion(new Vector2f(4f, 4f), 0, Fonts.Title, "Map Test Setup");
			this.pipeline.Add(this.titleText);
			this.sfxCursorX = AudioManager.Instance.Use(Paths.AUDIO + "cursorx.wav", AudioType.Sound);
			this.sfxCursorY = AudioManager.Instance.Use(Paths.AUDIO + "cursory.wav", AudioType.Sound);
			this.sfxConfirm = AudioManager.Instance.Use(Paths.AUDIO + "confirm.wav", AudioType.Sound);
			this.sfxCancel = AudioManager.Instance.Use(Paths.AUDIO + "cancel.wav", AudioType.Sound);
			FlagManager.Instance.Reset();
			ValueManager.Instance.Reset();
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00024D90 File Offset: 0x00022F90
		private void ResetCharacterGraphics()
		{
			if (this.characterSprites != null)
			{
				for (int i = 0; i < this.characterSprites.Length; i++)
				{
					if (this.characterSprites[i] != null)
					{
						this.pipeline.Remove(this.characterSprites[i]);
						this.characterSprites[i].Dispose();
					}
				}
			}
			this.characterSprites = new IndexedColorGraphic[this.selectedCharacters.Count];
			for (int j = 0; j < this.selectedCharacters.Count; j++)
			{
				Vector2f position = new Vector2f((float)(32 + 32 * j), 64f);
				this.characterSprites[j] = new IndexedColorGraphic(CharacterGraphics.GetFile(this.selectedCharacters[j]), "walk south", position, 0);
				if (this.characterSprites[j].GetSpriteDefinition("idle south") != null)
				{
					this.characterSprites[j].SetSprite("idle south");
				}
				if (this.characterSprites[j] != null)
				{
					this.pipeline.Add(this.characterSprites[j]);
				}
			}
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00024E90 File Offset: 0x00023090
		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			if (this.focusedList != null)
			{
				if (axis.Y < -0.1f)
				{
					if (this.focusedList.SelectPrevious())
					{
						this.sfxCursorY.Play();
						return;
					}
				}
				else if (axis.Y > 0.1f && this.focusedList.SelectNext())
				{
					this.sfxCursorX.Play();
				}
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00024EF4 File Offset: 0x000230F4
		private void ButtonPressed(InputManager sender, Button b)
		{
			switch (b)
			{
			case Button.A:
			case Button.Start:
				this.DoSelection();
				return;
			case Button.B:
				this.DoCancel();
				break;
			case Button.X:
			case Button.Y:
				break;
			default:
				return;
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00024F2C File Offset: 0x0002312C
		private void DoSelection()
		{
			this.sfxConfirm.Play();
			if (this.focusedList == this.optionList)
			{
				switch (this.optionList.SelectedIndex)
				{
				case 0:
					this.optionList.Hide();
					this.charactersList.Show();
					this.focusedList = this.charactersList;
					return;
				case 1:
					this.optionList.Hide();
					this.mapList.Show();
					this.focusedList = this.mapList;
					return;
				case 2:
					this.optionList.Hide();
					this.settingsList.Show();
					this.focusedList = this.settingsList;
					return;
				case 3:
					if (this.selectedCharacters.Count > 0)
					{
						AudioManager.Instance.FadeOut(AudioManager.Instance.BGM, 1500U);
						PartyManager.Instance.Clear();
						PartyManager.Instance.AddAll(this.selectedCharacters);
						SceneManager.Instance.Transition = new ColorFadeTransition(0.5f, Color.Black);
						SceneManager.Instance.Push(new OverworldScene(this.selectedMap, this.runScriptsOnLoad));
						return;
					}
					break;
				case 4:
					SceneManager.Instance.Pop();
					return;
				default:
					return;
				}
			}
			else
			{
				if (this.focusedList == this.mapList)
				{
					this.selectedMap = this.mapItems[this.mapList.SelectedIndex];
					this.optionList.ChangeItem(1, string.Format("Select Map ({0})", this.mapItems[this.mapList.SelectedIndex]));
					this.mapList.Hide();
					this.optionList.Show();
					this.focusedList = this.optionList;
					return;
				}
				if (this.focusedList == this.charactersList)
				{
					int selectedIndex = this.charactersList.SelectedIndex;
					if (selectedIndex > 0)
					{
						this.selectedCharacters.Add(CharacterType.GetByOptionInt(selectedIndex - 1));
					}
					else if (this.selectedCharacters.Count > 0)
					{
						this.selectedCharacters.RemoveAt(this.selectedCharacters.Count - 1);
					}
					this.ResetCharacterGraphics();
					this.charactersList.Hide();
					this.optionList.Show();
					this.focusedList = this.optionList;
					return;
				}
				if (this.focusedList == this.settingsList)
				{
					switch (this.settingsList.SelectedIndex)
					{
					case 0:
						this.runScriptsOnLoad = !this.runScriptsOnLoad;
						this.settingsList.ChangeItem(0, string.Format("Run scripts on map load: {0}", this.runScriptsOnLoad));
						return;
					case 1:
						FlagManager.Instance.Toggle(1);
						this.settingsList.ChangeItem(1, string.Format("Start at night: {0}", FlagManager.Instance[1]));
						return;
					case 2:
						this.settingsList.Hide();
						this.optionList.Show();
						this.focusedList = this.optionList;
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00025212 File Offset: 0x00023412
		private void DoCancel()
		{
			if (this.focusedList == this.optionList)
			{
				SceneManager.Instance.Pop();
				return;
			}
			this.focusedList.Hide();
			this.optionList.Show();
			this.focusedList = this.optionList;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00025250 File Offset: 0x00023450
		public override void Focus()
		{
			base.Focus();
			ViewManager.Instance.Center = new Vector2f(160f, 90f);
			Engine.ClearColor = Color.Black;
			AudioManager.Instance.SetBGM(Paths.AUDIO + "test.mp3");
			AudioManager.Instance.BGM.Play();
			InputManager.Instance.AxisPressed += this.AxisPressed;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000252DA File Offset: 0x000234DA
		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.AxisPressed -= this.AxisPressed;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00025310 File Offset: 0x00023510
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			AudioManager.Instance.Unuse(this.sfxCursorX);
			AudioManager.Instance.Unuse(this.sfxCursorY);
			AudioManager.Instance.Unuse(this.sfxConfirm);
			AudioManager.Instance.Unuse(this.sfxCancel);
		}

		// Token: 0x040007F8 RID: 2040
		private const string MAP_FORMAT_SEL_MAP = "Select Map ({0})";

		// Token: 0x040007F9 RID: 2041
		private const string OPT_FORMAT_RUN_SCRIPTS = "Run scripts on map load: {0}";

		// Token: 0x040007FA RID: 2042
		private const string OPT_FORMAT_NIGHTTIME = "Start at night: {0}";

		// Token: 0x040007FB RID: 2043
		private TextRegion titleText;

		// Token: 0x040007FC RID: 2044
		private ScrollingList focusedList;

		// Token: 0x040007FD RID: 2045
		private ScrollingList optionList;

		// Token: 0x040007FE RID: 2046
		private ScrollingList mapList;

		// Token: 0x040007FF RID: 2047
		private ScrollingList charactersList;

		// Token: 0x04000800 RID: 2048
		private ScrollingList settingsList;

		// Token: 0x04000801 RID: 2049
		private CarbineSound sfxCursorX;

		// Token: 0x04000802 RID: 2050
		private CarbineSound sfxCursorY;

		// Token: 0x04000803 RID: 2051
		private CarbineSound sfxConfirm;

		// Token: 0x04000804 RID: 2052
		private CarbineSound sfxCancel;

		// Token: 0x04000805 RID: 2053
		private string selectedMap;

		// Token: 0x04000806 RID: 2054
		private List<CharacterType> selectedCharacters;

		// Token: 0x04000807 RID: 2055
		private IndexedColorGraphic[] characterSprites;

		// Token: 0x04000808 RID: 2056
		private string[] mapItems;

		// Token: 0x04000809 RID: 2057
		private bool runScriptsOnLoad = true;
	}
}
