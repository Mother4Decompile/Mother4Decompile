using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Scenes;
using Mother4.Data;
using Mother4.GUI;
using Mother4.GUI.ProfileMenu;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x0200010E RID: 270
	internal class ProfilesScene : StandardScene
	{
		// Token: 0x0600067D RID: 1661 RVA: 0x00028DB8 File Offset: 0x00026FB8
		public ProfilesScene()
		{
			this.panelList = new List<MenuPanel>();
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00028DCB File Offset: 0x00026FCB
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.A)
			{
				this.DoLoad();
				return;
			}
			if (b == Button.B)
			{
				this.sfxCancel.Play();
				SceneManager.Instance.Pop();
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00028DF4 File Offset: 0x00026FF4
		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			if (axis.Y < 0f)
			{
				this.selectedIndex = Math.Max(0, this.selectedIndex - 1);
				this.UpdateCursor();
				return;
			}
			if (axis.Y > 0f)
			{
				this.selectedIndex = Math.Min(this.panelList.Count - 1, this.selectedIndex + 1);
				this.UpdateCursor();
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00028E5E File Offset: 0x0002705E
		private void UpdateCursor()
		{
			this.cursorGraphic.Position = new Vector2f(24f, (float)(32 + 57 * this.selectedIndex));
			this.sfxCursorY.Play();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00028E90 File Offset: 0x00027090
		private void DoLoad()
		{
			if (this.profileList.ContainsKey(this.selectedIndex))
			{
				this.sfxConfirm.Play();
				SaveFileManager.Instance.LoadFile(this.selectedIndex);
				Engine.StartSession();
				OverworldScene newScene = new OverworldScene(SaveFileManager.Instance.CurrentProfile.MapName, SaveFileManager.Instance.CurrentProfile.Position, 6, false, false, true);
				SceneManager.Instance.Push(newScene, true);
				return;
			}
			this.sfxCancel.Play();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00028F10 File Offset: 0x00027110
		private void GenerateSelectionList()
		{
			this.profileList = SaveFileManager.Instance.LoadProfiles();
			int num = Math.Max(3, this.profileList.Count);
			for (int i = 0; i < num; i++)
			{
				MenuPanel item = new ProfilePanel(new Vector2f(8f, (float)(8 + 57 * i)), new Vector2f(288f, 33f), i, this.profileList.ContainsKey(i) ? this.profileList[i] : default(SaveProfile));
				this.panelList.Add(item);
			}
			this.pipeline.AddAll<MenuPanel>(this.panelList);
			this.cursorGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "cursor.dat", "right", new Vector2f(24f, (float)(32 + 57 * this.selectedIndex)), 100);
			this.pipeline.Add(this.cursorGraphic);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00029000 File Offset: 0x00027200
		public override void Focus()
		{
			base.Focus();
			if (!this.isInitialized)
			{
				this.GenerateSelectionList();
				this.sfxCursorX = AudioManager.Instance.Use(Paths.AUDIO + "cursorx.wav", AudioType.Sound);
				this.sfxCursorY = AudioManager.Instance.Use(Paths.AUDIO + "cursory.wav", AudioType.Sound);
				this.sfxConfirm = AudioManager.Instance.Use(Paths.AUDIO + "confirm.wav", AudioType.Sound);
				this.sfxCancel = AudioManager.Instance.Use(Paths.AUDIO + "cancel.wav", AudioType.Sound);
				this.isInitialized = true;
			}
			ViewManager.Instance.Center = Engine.HALF_SCREEN_SIZE;
			Engine.ClearColor = Color.Black;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			InputManager.Instance.AxisPressed += this.AxisPressed;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x000290F0 File Offset: 0x000272F0
		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			InputManager.Instance.AxisPressed -= this.AxisPressed;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00029124 File Offset: 0x00027324
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.cursorGraphic.Dispose();
					foreach (MenuPanel menuPanel in this.panelList)
					{
						menuPanel.Dispose();
					}
				}
				AudioManager.Instance.Unuse(this.sfxCursorX);
				AudioManager.Instance.Unuse(this.sfxCursorY);
				AudioManager.Instance.Unuse(this.sfxConfirm);
				AudioManager.Instance.Unuse(this.sfxCancel);
				base.Dispose(disposing);
			}
		}

		// Token: 0x04000866 RID: 2150
		private const int PANEL_WIDTH = 288;

		// Token: 0x04000867 RID: 2151
		private const int PANEL_HEIGHT = 33;

		// Token: 0x04000868 RID: 2152
		private bool isInitialized;

		// Token: 0x04000869 RID: 2153
		private CarbineSound sfxCursorX;

		// Token: 0x0400086A RID: 2154
		private CarbineSound sfxCursorY;

		// Token: 0x0400086B RID: 2155
		private CarbineSound sfxConfirm;

		// Token: 0x0400086C RID: 2156
		private CarbineSound sfxCancel;

		// Token: 0x0400086D RID: 2157
		private IDictionary<int, SaveProfile> profileList;

		// Token: 0x0400086E RID: 2158
		private IndexedColorGraphic cursorGraphic;

		// Token: 0x0400086F RID: 2159
		private IList<MenuPanel> panelList;

		// Token: 0x04000870 RID: 2160
		private int selectedIndex;
	}
}
