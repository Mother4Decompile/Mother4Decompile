using System;
using Carbine;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Scenes;
using Mother4.Data;
using Mother4.GUI;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x0200010C RID: 268
	internal class OptionsScene : StandardScene
	{
		// Token: 0x06000652 RID: 1618 RVA: 0x000264C0 File Offset: 0x000246C0
		public OptionsScene()
		{
			this.sfxCursorX = AudioManager.Instance.Use(Paths.AUDIO + "cursorx.wav", AudioType.Sound);
			this.sfxCursorY = AudioManager.Instance.Use(Paths.AUDIO + "cursory.wav", AudioType.Sound);
			this.sfxConfirm = AudioManager.Instance.Use(Paths.AUDIO + "confirm.wav", AudioType.Sound);
			this.sfxCancel = AudioManager.Instance.Use(Paths.AUDIO + "cancel.wav", AudioType.Sound);
			this.titleText = new TextRegion(new Vector2f(4f, 4f), 0, Fonts.Title, "Global Options");
			this.pipeline.Add(this.titleText);
			this.mainList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.MAIN_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.bgmVolumeList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.BGM_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.sfxVolumeList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.SFX_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.flavorList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.FLAVOR_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.textSpeedList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.SPEED_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.scaleList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.SCALE_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.fullscreenList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.FULLSCREEN_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.focusedList = this.mainList;
			this.pipeline.Add(this.mainList);
			this.pipeline.Add(this.bgmVolumeList);
			this.pipeline.Add(this.sfxVolumeList);
			this.pipeline.Add(this.flavorList);
			this.pipeline.Add(this.textSpeedList);
			this.pipeline.Add(this.scaleList);
			this.pipeline.Add(this.fullscreenList);
			this.bgmVolumeList.Hide();
			this.sfxVolumeList.Hide();
			this.flavorList.Hide();
			this.textSpeedList.Hide();
			this.scaleList.Hide();
			this.fullscreenList.Hide();
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00026758 File Offset: 0x00024958
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

		// Token: 0x06000654 RID: 1620 RVA: 0x000267BC File Offset: 0x000249BC
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

		// Token: 0x06000655 RID: 1621 RVA: 0x000267F4 File Offset: 0x000249F4
		private void DoMainListSelection()
		{
			if (this.mainList.SelectedIndex < OptionsScene.MAIN_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				this.mainList.Hide();
			}
			switch (this.mainList.SelectedIndex)
			{
			case 0:
				(this.focusedList = this.bgmVolumeList).Show();
				this.bgmVolumeList.SelectedIndex = 10 - (int)(Settings.MusicVolume * 11f);
				return;
			case 1:
				(this.focusedList = this.sfxVolumeList).Show();
				this.sfxVolumeList.SelectedIndex = 10 - (int)(Settings.EffectsVolume * 11f);
				return;
			case 2:
				(this.focusedList = this.flavorList).Show();
				this.flavorList.SelectedIndex = (int)Settings.WindowFlavor;
				return;
			case 3:
				(this.focusedList = this.textSpeedList).Show();
				this.textSpeedList.SelectedIndex = 1;
				for (int i = 0; i < OptionsScene.TEXT_SPEEDS.Length; i++)
				{
					if (OptionsScene.TEXT_SPEEDS[i] == Settings.TextSpeed)
					{
						this.textSpeedList.SelectedIndex = i;
						return;
					}
				}
				return;
			case 4:
				(this.focusedList = this.scaleList).Show();
				this.scaleList.SelectedIndex = (int)(Engine.ScreenScale - 1U);
				return;
			case 5:
				(this.focusedList = this.fullscreenList).Show();
				this.fullscreenList.SelectedIndex = (Engine.Fullscreen ? 1 : 0);
				return;
			case 6:
				this.sfxCancel.Play();
				SceneManager.Instance.Pop();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000269A0 File Offset: 0x00024BA0
		private void DoBGMListSelection()
		{
			if (this.bgmVolumeList.SelectedIndex < OptionsScene.BGM_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				AudioManager.Instance.MusicVolume = (float)(10 - this.bgmVolumeList.SelectedIndex) / 10f;
				Settings.MusicVolume = AudioManager.Instance.MusicVolume;
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00026A0C File Offset: 0x00024C0C
		private void DoSFXListSelection()
		{
			if (this.sfxVolumeList.SelectedIndex < OptionsScene.SFX_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				AudioManager.Instance.EffectsVolume = (float)(10 - this.sfxVolumeList.SelectedIndex) / 10f;
				Settings.EffectsVolume = AudioManager.Instance.EffectsVolume;
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00026A75 File Offset: 0x00024C75
		private void DoFlavorSelection()
		{
			if (this.flavorList.SelectedIndex < OptionsScene.FLAVOR_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				Settings.WindowFlavor = (uint)this.flavorList.SelectedIndex;
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00026AB8 File Offset: 0x00024CB8
		private void DoSpeedSelection()
		{
			if (this.textSpeedList.SelectedIndex < OptionsScene.SPEED_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				Settings.TextSpeed = OptionsScene.TEXT_SPEEDS[this.textSpeedList.SelectedIndex];
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00026B0C File Offset: 0x00024D0C
		private void DoScaleSelection()
		{
			if (this.scaleList.SelectedIndex < OptionsScene.SCALE_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				Engine.ScreenScale = (uint)(this.scaleList.SelectedIndex + 1);
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00026B5C File Offset: 0x00024D5C
		private void DoFullscreenSelection()
		{
			if (this.fullscreenList.SelectedIndex < OptionsScene.FULLSCREEN_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				Engine.Fullscreen = (this.fullscreenList.SelectedIndex > 0);
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00026BB0 File Offset: 0x00024DB0
		private void DoSelection()
		{
			if (this.focusedList == this.mainList)
			{
				this.DoMainListSelection();
				return;
			}
			if (this.focusedList == this.bgmVolumeList)
			{
				this.DoBGMListSelection();
				return;
			}
			if (this.focusedList == this.sfxVolumeList)
			{
				this.DoSFXListSelection();
				return;
			}
			if (this.focusedList == this.flavorList)
			{
				this.DoFlavorSelection();
				return;
			}
			if (this.focusedList == this.textSpeedList)
			{
				this.DoSpeedSelection();
				return;
			}
			if (this.focusedList == this.scaleList)
			{
				this.DoScaleSelection();
				return;
			}
			if (this.focusedList == this.fullscreenList)
			{
				this.DoFullscreenSelection();
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00026C4F File Offset: 0x00024E4F
		private void GoBackToMainList()
		{
			this.focusedList.Hide();
			this.mainList.Show();
			this.focusedList = this.mainList;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00026C73 File Offset: 0x00024E73
		private void DoCancel()
		{
			this.sfxCancel.Play();
			if (this.focusedList == this.mainList)
			{
				SceneManager.Instance.Pop();
				return;
			}
			this.GoBackToMainList();
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00026CA0 File Offset: 0x00024EA0
		public override void Focus()
		{
			base.Focus();
			ViewManager.Instance.Center = new Vector2f(160f, 90f);
			Engine.ClearColor = Color.Black;
			InputManager.Instance.AxisPressed += this.AxisPressed;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00026D02 File Offset: 0x00024F02
		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.AxisPressed -= this.AxisPressed;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00026D38 File Offset: 0x00024F38
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			AudioManager.Instance.Unuse(this.sfxCursorX);
			AudioManager.Instance.Unuse(this.sfxCursorY);
			AudioManager.Instance.Unuse(this.sfxConfirm);
			AudioManager.Instance.Unuse(this.sfxCancel);
		}

		// Token: 0x0400082A RID: 2090
		private const int LIST_DISPLAY_COUNT = 8;

		// Token: 0x0400082B RID: 2091
		private const int LIST_LINE_HEIGHT = 16;

		// Token: 0x0400082C RID: 2092
		private const int LIST_WIDTH = 80;

		// Token: 0x0400082D RID: 2093
		private TextRegion titleText;

		// Token: 0x0400082E RID: 2094
		private static readonly Vector2f MENU_POSITION = new Vector2f(32f, 32f);

		// Token: 0x0400082F RID: 2095
		private static readonly string CURSOR_FILE = Paths.GRAPHICS + "cursor.dat";

		// Token: 0x04000830 RID: 2096
		private ScrollingList focusedList;

		// Token: 0x04000831 RID: 2097
		private ScrollingList mainList;

		// Token: 0x04000832 RID: 2098
		private ScrollingList bgmVolumeList;

		// Token: 0x04000833 RID: 2099
		private ScrollingList sfxVolumeList;

		// Token: 0x04000834 RID: 2100
		private ScrollingList flavorList;

		// Token: 0x04000835 RID: 2101
		private ScrollingList textSpeedList;

		// Token: 0x04000836 RID: 2102
		private ScrollingList scaleList;

		// Token: 0x04000837 RID: 2103
		private ScrollingList fullscreenList;

		// Token: 0x04000838 RID: 2104
		private CarbineSound sfxCursorX;

		// Token: 0x04000839 RID: 2105
		private CarbineSound sfxCursorY;

		// Token: 0x0400083A RID: 2106
		private CarbineSound sfxConfirm;

		// Token: 0x0400083B RID: 2107
		private CarbineSound sfxCancel;

		// Token: 0x0400083C RID: 2108
		private static readonly string[] MAIN_MENU = new string[]
		{
			"BGM Volume",
			"SFX Volume",
			"Window Flavor",
			"Text Speed",
			"Window Scale",
			"Fullscreen",
			"Back"
		};

		// Token: 0x0400083D RID: 2109
		private static readonly string[] BGM_MENU = new string[]
		{
			"100%",
			"90%",
			"80%",
			"70%",
			"60%",
			"50%",
			"40%",
			"30%",
			"20%",
			"10%",
			"Mute",
			"Back"
		};

		// Token: 0x0400083E RID: 2110
		private static readonly string[] SFX_MENU = new string[]
		{
			"100%",
			"90%",
			"80%",
			"70%",
			"60%",
			"50%",
			"40%",
			"30%",
			"20%",
			"10%",
			"Mute",
			"Back"
		};

		// Token: 0x0400083F RID: 2111
		private static readonly string[] FLAVOR_MENU = new string[]
		{
			"Plain",
			"Lime",
			"Strawberry",
			"Banana",
			"Peanut",
			"Blue Raspberry",
			"Grape",
			"Doom",
			"Back"
		};

		// Token: 0x04000840 RID: 2112
		private static readonly string[] SPEED_MENU = new string[]
		{
			"Slow",
			"Average",
			"Fast",
			"Very Fast",
			"Ludicrous Speed",
			"Back"
		};

		// Token: 0x04000841 RID: 2113
		private static readonly string[] SCALE_MENU = new string[]
		{
			"1x",
			"2x",
			"3x",
			"4x",
			"5x",
			"Back"
		};

		// Token: 0x04000842 RID: 2114
		private static readonly string[] FULLSCREEN_MENU = new string[]
		{
			"Windowed",
			"Fullscreen",
			"Back"
		};

		// Token: 0x04000843 RID: 2115
		private static readonly string[] VSYNC_MENU = new string[]
		{
			"Enabled",
			"Disabled",
			"Back"
		};

		// Token: 0x04000844 RID: 2116
		private static readonly int[] TEXT_SPEEDS = new int[]
		{
			20,
			10,
			5,
			2,
			1
		};
	}
}
