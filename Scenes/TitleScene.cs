using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Carbine;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Carbine.Utility;
using Mother4.Data;
using Mother4.Data.Psi;
using Mother4.GUI;
using Mother4.GUI.Modifiers;
using Rufini.Strings;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x02000114 RID: 276
	internal class TitleScene : StandardScene
	{
		// Token: 0x060006A4 RID: 1700 RVA: 0x00029D40 File Offset: 0x00027F40
		public TitleScene()
		{
			Fonts.LoadFonts(Settings.Locale);
			string[] items;
			if (File.Exists("sav.dat"))
			{
				this.canContinue = true;
				items = new string[]
				{
					"Map Test",
					"New Game",
					"Continue",
					"Options",
					"Quit"
				};
			}
			else
			{
				this.canContinue = false;
				items = new string[]
				{
					"Map Test",
					"New Game",
					"Options",
					"Quit"
				};
			}
			this.optionList = new ScrollingList(new Vector2f(32f, 80f), 0, items, 5, 16f, 80f, Paths.GRAPHICS + "cursor.dat");
			this.optionList.ShowSelectionRectangle = false;
			this.optionList.UseHighlightTextColor = true;
			this.pipeline.Add(this.optionList);
			this.titleImage = new IndexedColorGraphic(Paths.GRAPHICS + "title.dat", "title", new Vector2f(160f, 44f), 100);
			Version version = Assembly.GetEntryAssembly().GetName().Version;
			this.versionText = new TextRegion(new Vector2f(2f, 164f), 0, Fonts.Main, string.Format("{0}.{1} {2} {3} {4}", new object[]
			{
				version.Major,
				version.Minor,
				version.Build,
				version.Revision,
				StringFile.Instance.Get("psi.symbols.alpha")
			}));
			///this.versionText.Color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 128);
			this.pipeline.Add(this.titleImage);
			this.pipeline.Add(this.versionText);
			this.mod = new GraphicTranslator(this.titleImage, new Vector2f(160f, 36f), 30);
			this.sfxCursorY = AudioManager.Instance.Use(Paths.AUDIO + "cursory.wav", AudioType.Sound);
			this.sfxConfirm = AudioManager.Instance.Use(Paths.AUDIO + "confirm.wav", AudioType.Sound);
			this.sfxCancel = AudioManager.Instance.Use(Paths.AUDIO + "cancel.wav", AudioType.Sound);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00029FC4 File Offset: 0x000281C4
		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			if (axis.Y < -0.1f)
			{
				if (this.optionList.SelectPrevious())
				{
					this.sfxCursorY.Play();
					return;
				}
			}
			else if (axis.Y > 0.1f && this.optionList.SelectNext())
			{
				this.sfxCursorY.Play();
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0002A020 File Offset: 0x00028220
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (b > Button.Start)
			{
				if (b != Button.F1)
				{
					switch (b)
					{
					case Button.One:
						goto IL_46;
					case Button.Two:
						SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
						SceneManager.Instance.Push(new TextTestScene());
						return;
					case Button.Three:
						SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
						SceneManager.Instance.Push(new PsiTestScene());
						return;
					case Button.Four:
						SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
						SceneManager.Instance.Push(new SaveScene(SaveScene.Location.Belring, SaveFileManager.Instance.CurrentProfile));
						return;
					case Button.Five:
						SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
						SceneManager.Instance.Push(new EnemyTestScene());
						return;
					case Button.Six:
					{
						List<PsiData> allPsiData = PsiFile.Instance.GetAllPsiData();
						using (List<PsiData>.Enumerator enumerator = allPsiData.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								PsiData psiData = enumerator.Current;
								Console.Write(psiData.QualifiedName);
								Console.Write(" ");
								for (int i = 0; i < psiData.Symbols.Length; i++)
								{
									Console.Write(psiData.GetSymbol(i));
									if (i < psiData.Symbols.Length - 1)
									{
										Console.Write(", ");
									}
								}
								Console.WriteLine();
							}
							return;
						}
						break;
					}
					case Button.Seven:
						break;
					case Button.Eight:

							Engine.ScreenScale = 6;
							SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
							PartyManager.Instance.AddAll(new CharacterType[]
{
					CharacterType.Travis,
					CharacterType.Floyd,
					CharacterType.Meryl
}); SceneManager.Instance.Push(new BattleScene(new EnemyType[] { EnemyType.AtomicPowerRobo, EnemyType.Flamingo }, true));
							 break;
					default:
						return;
					}
					Console.WriteLine("{0:x}\t{1}", Hash.Get("Hometown Strut"), "Hometown Strut");
					Console.WriteLine("{0:x}\t{1}", Hash.Get("Hometown Laze"), "Hometown Laze");
					Console.WriteLine("{0:x}\t{1}", Hash.Get("A House"), "A House");
					return;
				}
				IL_46:
				SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
				PartyManager.Instance.Clear();
				PartyManager.Instance.AddAll(new CharacterType[]
				{
					CharacterType.Travis,
					CharacterType.Floyd,
					CharacterType.Meryl
				});
				OverworldScene newScene = new OverworldScene("debug_room.dat", new Vector2f(256f, 128f), 6, false, false, false);
				SceneManager.Instance.Push(newScene);
				return;
			}
			if (b != Button.A && b != Button.Start)
			{
				return;
			}
			this.DoSelection();
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0002A2B0 File Offset: 0x000284B0
		private void DoSelection()
		{
			int num = this.optionList.SelectedIndex;
			if (!this.canContinue && num > 1)
			{
				num++;
			}
			switch (num)
			{
			case 0:
				this.sfxConfirm.Play();
				SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
				SceneManager.Instance.Push(new MapTestSetupScene());
				return;
			case 1:
				this.sfxConfirm.Play();
				SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
				SceneManager.Instance.Push(new NamingScene());
				return;
			case 2:
				this.sfxConfirm.Play();
				SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
				SceneManager.Instance.Push(new ProfilesScene());
				return;
			case 3:
				this.sfxConfirm.Play();
				SceneManager.Instance.Transition = new ColorFadeTransition(0.25f, Color.Black);
				SceneManager.Instance.Push(new OptionsScene());
				return;
			case 4:
				this.sfxConfirm.Play();
				SceneManager.Instance.Pop();
				return;
			default:
				return;
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002A3DC File Offset: 0x000285DC
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

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002A466 File Offset: 0x00028666
		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.AxisPressed -= this.AxisPressed;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002A49A File Offset: 0x0002869A
		public override void Update()
		{
			base.Update();
			this.mod.Update();
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0002A4B0 File Offset: 0x000286B0
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.titleImage.Dispose();
					this.optionList.Dispose();
					this.versionText.Dispose();
				}
				AudioManager.Instance.Unuse(this.sfxCursorY);
				AudioManager.Instance.Unuse(this.sfxConfirm);
				AudioManager.Instance.Unuse(this.sfxCancel);
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400089C RID: 2204
		private TextRegion versionText;

		// Token: 0x0400089D RID: 2205
		private ScrollingList optionList;

		// Token: 0x0400089E RID: 2206
		private IndexedColorGraphic titleImage;

		// Token: 0x0400089F RID: 2207
		private CarbineSound sfxCursorY;

		// Token: 0x040008A0 RID: 2208
		private CarbineSound sfxConfirm;

		// Token: 0x040008A1 RID: 2209
		private CarbineSound sfxCancel;

		// Token: 0x040008A2 RID: 2210
		private IGraphicModifier mod;

		// Token: 0x040008A3 RID: 2211
		private bool canContinue;
	}
}
