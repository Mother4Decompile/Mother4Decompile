using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Audio;
using Carbine.Collision;
using Carbine.Flags;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Maps;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Carbine.Tiles;
using Carbine.Utility;
using Mother4.Actors;
using Mother4.Actors.NPCs;
using Mother4.Battle;
using Mother4.Battle.Background;
using Mother4.Data;
using Mother4.Data.Enemies;
using Mother4.GUI;
using Mother4.Items;
using Mother4.Overworld;
using Mother4.Scenes.Transitions;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.Types;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x0200010D RID: 269
	internal class OverworldScene : StandardScene
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0002704A File Offset: 0x0002524A
		public ScreenDimmer Dimmer
		{
			get
			{
				return this.screenDimmer;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00027052 File Offset: 0x00025252
		public PartyTrain PartyTrain
		{
			get
			{
				return this.partyTrain;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0002705A File Offset: 0x0002525A
		public IrisOverlay IrisOverlay
		{
			get
			{
				return this.GetIrisOverlay();
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00027064 File Offset: 0x00025264
		public OverworldScene(string mapName, Vector2f initialPosition, int initialDirection, bool initialRunning, bool extendParty, bool enableLoadScripts)
		{
			this.mapName = mapName;
			this.initialPosition = initialPosition;
			this.initialDirection = initialDirection;
			this.initialRunning = initialRunning;
			this.enableLoadScripts = enableLoadScripts;
			this.extendParty = extendParty;
			this.collisionResults = new ICollidable[8];
			this.initialized = false;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000270B7 File Offset: 0x000252B7
		public OverworldScene(string mapName, bool enableLoadScripts) : this(mapName, VectorMath.ZERO_VECTOR, 6, false, false, enableLoadScripts)
		{
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x000270CC File Offset: 0x000252CC
		private IrisOverlay GetIrisOverlay()
		{
			if (this.iris == null)
			{
				Vector2f origin = new Vector2f(160f, 90f);
				this.iris = new IrisOverlay(ViewManager.Instance.FinalCenter, origin, 0f);
			}
			return this.iris;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00027113 File Offset: 0x00025313
		public void SetTilesetPalette(int tilesetPalette)
		{
			if (this.initialized && this.mapGroups.Count > 0)
			{
				this.mapGroups[0].Tileset.CurrentPalette = (uint)tilesetPalette;
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00027142 File Offset: 0x00025342
		public void SetLetterboxing(bool enabled)
		{
			if (this.initialized)
			{
				if (enabled)
				{
					this.letterboxing.Show();
					return;
				}
				this.letterboxing.Hide();
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00027168 File Offset: 0x00025368
		private void SetExecutorScript(string scriptName, bool isTelepathy)
		{
			Script? script = ScriptLoader.Load(scriptName);
			if (script != null)
			{
				Script value = script.Value;
				if (isTelepathy)
				{
					RufiniAction[] array = new RufiniAction[value.Actions.Length + 2];
					Array.Copy(value.Actions, 0, array, 1, value.Actions.Length);
					array[0] = new TelepathyStartAction();
					array[array.Length - 1] = new TelepathyEndAction();
					value.Actions = array;
				}
				this.executor.PushScript(value);
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000271E4 File Offset: 0x000253E4
		private bool HandleNpcCheck(NPC npc, bool isTelepathy)
		{
			bool result = false;
			Map.NPCtext npctext = new Map.NPCtext
			{
				ID = "",
				Flag = -1
			};
			int num = int.MinValue;
			List<Map.NPCtext> list = (!isTelepathy) ? npc.Text : npc.TeleText;
			foreach (Map.NPCtext npctext2 in list)
			{
				if (FlagManager.Instance[npctext2.Flag] && npctext2.Flag > num)
				{
					num = npctext2.Flag;
					npctext = npctext2;
				}
			}
			if (npctext.Flag > -1)
			{
				double num2 = Math.Atan2((double)(npc.Position.Y - this.player.Position.Y), (double)(-(double)(npc.Position.X - this.player.Position.X)));
				int num3 = (int)Math.Round(num2 / 0.7853981633974483);
				if (num3 < 0)
				{
					num3 += 8;
				}
				npc.Direction = num3;
				Script? script = ScriptLoader.Load(npctext.ID);
				if (script != null)
				{
					result = true;
					Script value = script.Value;
					if (isTelepathy)
					{
						RufiniAction[] array = new RufiniAction[value.Actions.Length + 2];
						Array.Copy(value.Actions, 0, array, 1, value.Actions.Length);
						array[0] = new TelepathyStartAction();
						array[array.Length - 1] = new TelepathyEndAction();
						value.Actions = array;
					}
					this.executor.SetCheckedNPC(npc);
					this.executor.PushScript(value);
				}
			}
			return result;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00027390 File Offset: 0x00025590
		private void HandleCheckAction(bool isTelepathy)
		{
			bool flag = false;
			bool flag2 = false;
			Vector2f position = this.player.Position + this.player.CheckVector;
			Console.WriteLine("Checking at {0},{1}", position.X, position.Y);
			if (!this.collisionManager.PlaceFree(this.player, position, this.collisionResults))
			{
				do
				{
					Console.WriteLine("results loop start");
					for (int i = 0; i < this.collisionResults.Length; i++)
					{
						Console.Write("{0}: ", i);
						ICollidable collidable = this.collisionResults[i];
						if (collidable is NPC)
						{
							Console.WriteLine("Found NPC");
							flag2 = this.HandleNpcCheck((NPC)collidable, isTelepathy);
							flag = false;
							break;
						}
						if (!flag && collidable is SolidStatic)
						{
							Console.WriteLine("Found SolidStatic");
							flag = true;
						}
						else
						{
							Console.WriteLine("Not an NPC or SolidStatic");
							flag = false;
							if (collidable == null)
							{
								break;
							}
						}
					}
					Console.WriteLine("results loop end");
					if (flag)
					{
						Vector2f position2 = this.player.Position + this.player.CheckVector * 2f;
						Console.WriteLine("Checking again at {0},{1}", position2.X, position2.Y);
						bool flag3 = this.collisionManager.PlaceFree(this.player, position2, this.collisionResults);
					}
				}
				while (flag);
			}
			if (!flag2)
			{
				Console.WriteLine("Tried checking, but there was no script");
				this.SetExecutorScript("Default", isTelepathy);
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0002751C File Offset: 0x0002571C
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.F1)
			{
				Console.WriteLine("View position: ({0},{1})", ViewManager.Instance.FinalCenter.X, ViewManager.Instance.FinalCenter.Y);
			}
			if (!this.executor.Running)
			{
				if (b == Button.One)
				{
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
					CharacterStats.SetStats(CharacterType.Travis, new StatSet
					{
						HP = 89,
						PP = 35,
						Meter = 0f,
						Offense = 6,
						Speed = 150,
						Guts = 0,
						Level = 40
					});
					SceneManager.Instance.Push(new BattleScene(new EnemyType[]
					{
						EnemyType.NotSoDeer
					}, true));
				}
				else if (b == Button.Two)
				{
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
					CharacterStats.SetStats(CharacterType.Travis, new StatSet
					{
						HP = 110,
						PP = 80,
						Meter = 0.4f,
						Offense = 16,
						Level = 40,
						Speed = 5
					});
					CharacterStats.SetStats(CharacterType.Floyd, new StatSet
					{
						HP = 59,
						PP = 0,
						Meter = 0.58666664f,
						Offense = 14,
						Guts = int.MaxValue,
						Speed = 20
					});
					SceneManager.Instance.Push(new BattleScene(new EnemyType[]
					{
						EnemyType.Rat
					}, true));
				}
				else if (b == Button.Three)
				{
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
					CharacterStats.SetStats(CharacterType.Travis, new StatSet
					{
						HP = 178,
						PP = 80,
						Meter = 0.06666667f,
						Offense = 20,
						Level = 40,
						Speed = 5
					});
					CharacterStats.SetStats(CharacterType.Floyd, new StatSet
					{
						HP = 160,
						PP = 0,
						Meter = 0.7866667f,
						Offense = 20,
						Speed = 20
					});
					CharacterStats.SetStats(CharacterType.Meryl, new StatSet
					{
						HP = 93,
						MaxHP = 120,
						PP = 116,
						MaxPP = 116,
						Meter = 0.44f,
						Offense = 20,
						Level = 40,
						Speed = 30
					});
					SceneManager.Instance.Push(new BattleScene(new EnemyType[]
					{
						EnemyType.HermitCan,
						EnemyType.Flamingo
					}, true));
				}
				else if (b == Button.Four)
				{
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
					CharacterStats.SetStats(CharacterType.Leo, new StatSet
					{
						HP = 177,
						PP = 209,
						Meter = 0.82666665f,
						Offense = 20,
						Level = 24,
						Speed = 40
					});
					SceneManager.Instance.Push(new BattleScene(new EnemyType[]
					{
						EnemyType.AtomicPowerRobo
					}, true));
				}
				else if (b == Button.Five)
				{
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
					CharacterStats.SetStats(CharacterType.Travis, new StatSet
					{
						HP = 290,
						PP = 11,
						Meter = 0.33333334f,
						Offense = 20,
						Level = 40,
						Speed = 10
					});
					CharacterStats.SetStats(CharacterType.Floyd, new StatSet
					{
						HP = 213,
						PP = 0,
						Meter = 0.06666667f,
						Offense = 20,
						Speed = 20
					});
					CharacterStats.SetStats(CharacterType.Meryl, new StatSet
					{
						HP = 177,
						PP = 209,
						Meter = 0.97333336f,
						Offense = 20,
						Level = 40,
						Speed = 30
					});
					SceneManager.Instance.Push(new BattleScene(new EnemyType[]
					{
						EnemyType.MeltyRobot,
						EnemyType.MeltyRobot
					}, true));
				}
				else if (b == Button.Six)
				{
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Boss);
					CharacterStats.SetStats(CharacterType.Travis, new StatSet
					{
						HP = 490,
						MaxHP = 492,
						PP = 7,
						MaxPP = 380,
						Meter = 0.14666666f,
						Offense = 20,
						Level = 40,
						Speed = 5
					});
					CharacterStats.SetStats(CharacterType.Floyd, new StatSet
					{
						HP = 1,
						MaxHP = 460,
						PP = 0,
						MaxPP = 0,
						Meter = 0.93333334f,
						Offense = 20,
						Speed = 15
					});
					CharacterStats.SetStats(CharacterType.Meryl, new StatSet
					{
						HP = 14,
						MaxHP = 380,
						PP = 155,
						MaxPP = 220,
						Meter = 0.04f,
						Offense = 20,
						Level = 40,
						Speed = 40
					});
					CharacterStats.SetStats(CharacterType.Leo, new StatSet
					{
						HP = 199,
						MaxHP = 512,
						PP = 6,
						MaxPP = 180,
						Meter = 0.6666667f,
						Offense = 20,
						Level = 40,
						Speed = 30
					});
					SceneManager.Instance.Push(new BattleScene(new EnemyType[]
					{
						EnemyType.ModernMind
					}, false));
				}
				else if (b == Button.Seven)
				{
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
					EnemyType[] array = new EnemyType[Engine.Random.Next(12) + 1];
					List<EnemyType> allEnemyTypes = EnemyFile.Instance.GetAllEnemyTypes();
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = allEnemyTypes[Engine.Random.Next(allEnemyTypes.Count - 1) + 1];
					}
					SceneManager.Instance.Push(new BattleScene(array, true));
				}
				else if (b == Button.Eight)
				{
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
					CharacterStats.SetStats(CharacterType.Leo, new StatSet
					{
						HP = 121,
						MaxHP = 302,
						PP = 22,
						MaxPP = 80,
						Meter = 0.44f,
						Offense = 20,
						Level = 5,
						Speed = 15
					});
					SceneManager.Instance.Push(new BattleScene(new EnemyType[]
					{
						EnemyType.PunkAssassin,
						EnemyType.PunkAssassin,
						EnemyType.PunkEnforcer
					}, true));
				}
				else if (b == Button.Nine)
				{
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
					CharacterStats.SetStats(CharacterType.Travis, new StatSet
					{
						HP = 223,
						MaxHP = 302,
						PP = 44,
						MaxPP = 80,
						Meter = 0.29333332f,
						Offense = 20,
						Level = 5,
						Speed = 20
					});
					CharacterStats.SetStats(CharacterType.Floyd, new StatSet
					{
						HP = 152,
						MaxHP = 289,
						PP = 0,
						MaxPP = 0,
						Meter = 0.53333336f,
						Offense = 20,
						Level = 5,
						Speed = 15
					});
					CharacterStats.SetStats(CharacterType.Meryl, new StatSet
					{
						HP = 90,
						MaxHP = 205,
						PP = 120,
						MaxPP = 135,
						Meter = 0.08f,
						Offense = 20,
						Level = 5,
						Speed = 15
					});
					SceneManager.Instance.Push(new BattleScene(new EnemyType[]
					{
						EnemyType.RatDispenser,
						EnemyType.RatDispenser,
						EnemyType.Rat
					}, true));
				}
				if (b == Button.Start)
				{
					CharacterType[] array2 = PartyManager.Instance.ToArray();
					foreach (CharacterType key in array2)
					{
						Inventory inventory = InventoryManager.Instance.Get(key);
						int num = Engine.Random.Next(14);
						for (int k = 0; k < num; k++)
						{
							Item item = new Item(false);
							item.Set("name", "Test item " + (k + 1));
							inventory.Add(item);
						}
					}
					this.dontPauseMusic = true;
					this.openingMenu = true;
					MenuScene newScene = new MenuScene();
					SceneManager.Instance.Transition = new InstantTransition();
					SceneManager.Instance.Push(newScene);
					return;
				}
				if (b == Button.A)
				{
					this.HandleCheckAction(false);
					return;
				}
				if (b == Button.F6)
				{
					SaveProfile currentProfile = SaveFileManager.Instance.CurrentProfile;
					currentProfile.IsValid = true;
					currentProfile.Party = PartyManager.Instance.ToArray();
					currentProfile.MapName = this.mapName;
					currentProfile.Position = this.player.Position;
					currentProfile.Time += Engine.SessionTime;
					currentProfile.Flavor = (int)Settings.WindowFlavor;
					SaveFileManager.Instance.CurrentProfile = currentProfile;
					SaveFileManager.Instance.SaveFile();
					return;
				}
				if (b == Button.F7)
				{
					BattleSwirlOverlay renderable = new BattleSwirlOverlay(BattleSwirlOverlay.Style.Blue, 2147483547, 0.015f);
					this.pipeline.Add(renderable);
				}
			}
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00028058 File Offset: 0x00026258
		private void Initialize()
		{
			ViewManager.Instance.Reset();
			this.screenDimmer = new ScreenDimmer(this.pipeline, Color.Transparent, 0, 2147450870);
			this.textbox = new OverworldTextBox();
			this.pipeline.Add(this.textbox);
			this.letterboxing = new LetterboxingOverlay();
			this.pipeline.Add(this.letterboxing);
			this.footstepPlayer = new FootstepPlayer();
			Map map = MapLoader.Load(Paths.MAPS + this.mapName, Paths.GRAPHICS);
			if (this.initialPosition == VectorMath.ZERO_VECTOR)
			{
				this.initialPosition = new Vector2f((float)(map.Head.Width / 2), (float)(map.Head.Height / 2));
			}
			this.collisionManager = new CollisionManager(map.Head.Width, map.Head.Height);
			CharacterType[] array = PartyManager.Instance.ToArray();
			this.partyTrain = new PartyTrain(this.initialPosition, this.initialDirection, map.Head.Ocean ? TerrainType.Ocean : TerrainType.None, this.extendParty);
			this.player = new Player(this.pipeline, this.collisionManager, this.partyTrain, this.initialPosition, this.initialDirection, array[0], map.Head.Shadows, map.Head.Ocean, this.initialRunning);
			this.player.OnCollision += this.OnPlayerCollision;
			this.player.OnRunningChange += this.OnPlayerRunningChange;
			this.actorManager.Add(this.player);
			this.collisionManager.Add(this.player);
			for (int i = 1; i < array.Length; i++)
			{
				PartyFollower partyFollower = new PartyFollower(this.pipeline, this.collisionManager, this.partyTrain, array[i], this.player.Position, this.player.Direction, map.Head.Shadows);
				this.partyTrain.Add(partyFollower);
				this.collisionManager.Add(partyFollower);
			}
			List<NPC> addActors = MapPopulator.GenerateNPCs(this.pipeline, this.collisionManager, map);
			this.actorManager.AddAll<NPC>(addActors);
			IList<ICollidable> collidables = MapPopulator.GeneratePortals(map);
			this.collisionManager.AddAll<ICollidable>(collidables);
			IList<ICollidable> collidables2 = MapPopulator.GenerateTriggerAreas(map);
			this.collisionManager.AddAll<ICollidable>(collidables2);
			this.spawners = MapPopulator.GenerateSpawners(map);
			this.parallaxes = MapPopulator.GenerateParallax(map);
			this.pipeline.AddAll<ParallaxBackground>(this.parallaxes);
			this.testBack = MapPopulator.GenerateBBGOverlay(map);
			if (this.testBack != null)
			{
				this.pipeline.Add(this.testBack);
			}
			foreach (Mesh mesh in map.Mesh)
			{
				this.collisionManager.Add(new SolidStatic(mesh));
			}
			bool flag = FlagManager.Instance[1];
			this.mapGroups = map.MakeTileGroups(Paths.GRAPHICS, flag ? 1U : 0U);
			this.pipeline.AddAll<TileGroup>(this.mapGroups);
			this.backColor = (flag ? map.Head.SecondaryColor : map.Head.PrimaryColor);
			ExecutionContext context = new ExecutionContext
			{
				Pipeline = this.pipeline,
				ActorManager = this.actorManager,
				CollisionManager = this.collisionManager,
				TextBox = this.textbox,
				Player = this.player,
				Paths = map.Paths,
				Areas = map.Areas
			};
			this.executor = new ScriptExecutor(context);
			if (map.Head.Script != null && this.enableLoadScripts)
			{
				Script? script = ScriptLoader.Load(map.Head.Script);
				if (script != null)
				{
					Console.WriteLine("Executing script on load: {0}", script.Value.Name);
					this.executor.PushScript(script.Value);
				}
				else
				{
					Console.WriteLine("Could not load script \"{0}\"", map.Head.Script);
				}
			}
			else
			{
				Console.WriteLine("This map has no onload scripts, or executing scripts on load is disabled");
			}
			string text = null;
			int num = -1;
			foreach (Map.BGM bgm in map.Music)
			{
				if (FlagManager.Instance[(int)bgm.Flag] && (int)bgm.Flag > num)
				{
					text = bgm.Name;
					num = (int)bgm.Flag;
				}
			}
			if (text != null)
			{
				this.musicName = text;
				Console.WriteLine("Play BGM {0}", this.musicName);
				AudioManager.Instance.SetBGM(this.musicName);
			}
			else
			{
				Console.WriteLine((map.Music.Count > 0) ? "No BGM flags were enabled for any BGM for this map." : "This map has no BGMs set.");
			}
			this.battleStartSound = AudioManager.Instance.Use(Paths.AUDIO + "battleIntro.mp3", AudioType.Sound);
			this.initialized = true;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000285CC File Offset: 0x000267CC
		public override void Focus()
		{
			base.Focus();
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.pipeline.Each(delegate(Renderable x)
			{
				if (x is IndexedColorGraphic)
				{
					((IndexedColorGraphic)x).AnimationEnabled = true;
				}
				if (x is TileGroup)
				{
					((TileGroup)x).AnimationEnabled = true;
				}
			});
			ViewManager.Instance.FollowActor = this.player;
			ViewManager.Instance.Offset = new Vector2f(0f, (float)(-(float)((int)this.player.AABB.Size.Y) / 2));
			if (this.battleEnemies != null)
			{
				this.player.MovementLocked = false;
				foreach (EnemyNPC enemyNPC in this.battleEnemies)
				{
					this.actorManager.Remove(enemyNPC);
					this.collisionManager.Remove(enemyNPC);
				}
			}
			if (FlagManager.Instance[3])
			{
				this.HandleCheckAction(true);
				FlagManager.Instance[3] = false;
			}
			Engine.ClearColor = this.backColor;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			this.footstepPlayer.Resume();
			if (!this.dontPauseMusic)
			{
				if (this.musicName != null)
				{
					AudioManager.Instance.SetBGM(this.musicName);
					AudioManager.Instance.BGM.Play();
					AudioManager.Instance.BGM.Position = this.musicPosition;
					return;
				}
				if (AudioManager.Instance.BGM != null)
				{
					AudioManager.Instance.BGM.Stop();
					return;
				}
			}
			else
			{
				this.dontPauseMusic = false;
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0002876C File Offset: 0x0002696C
		public void GoToMap(string map, float xto, float yto, int direction, bool running, bool extendParty, ITransition transition)
		{
			Console.WriteLine("DOOR TIME! Loading {0}", map);
			SceneManager.Instance.Transition = transition;
			SceneManager.Instance.Push(new OverworldScene(map, new Vector2f(xto, yto), direction, running, extendParty, this.enableLoadScripts), true);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x000287AC File Offset: 0x000269AC
		public void GoToMap(Portal door, ITransition transition)
		{
			transition.Blocking = true;
			int direction = (door.DirectionTo < 0) ? this.player.Direction : door.DirectionTo;
			this.GoToMap(door.Map, door.PositionTo.X, door.PositionTo.Y, direction, this.player.Running, false, transition);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0002880D File Offset: 0x00026A0D
		private void OnPlayerRunningChange(Player sender)
		{
			if (sender.Running)
			{
				this.footstepPlayer.Start();
				return;
			}
			this.footstepPlayer.Stop();
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00028830 File Offset: 0x00026A30
		private void OnPlayerCollision(Player sender, ICollidable[] collisionObjects)
		{
			foreach (ICollidable collidable in collisionObjects)
			{
				if (collidable is Portal)
				{
					this.GoToMap((Portal)collidable, new ColorFadeTransition(0.5f, Color.Black));
					return;
				}
				if (collidable is TriggerArea)
				{
					string script = ((TriggerArea)collidable).Script;
					Console.WriteLine("Trigger Area - " + script);
					this.SetExecutorScript(script, false);
					this.executor.Execute();
					collidable.Solid = false;
					return;
				}
				if (collidable is EnemyNPC)
				{
					EnemyNPC enemyNPC = (EnemyNPC)collidable;
					enemyNPC.MovementLocked = true;
					enemyNPC.FreezeSpriteForever();
					this.battleEnemies = new List<EnemyNPC>();
					this.battleEnemies.Add(enemyNPC);
					this.player.MovementLocked = true;
					this.musicPosition = AudioManager.Instance.BGM.Position;
					AudioManager.Instance.BGM.Stop();
					this.battleStartSound.Play();
					SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
					SceneManager.Instance.Push(new BattleScene(new EnemyType[]
					{
						enemyNPC.Type
					}, true));
					return;
				}
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00028994 File Offset: 0x00026B94
		public override void Unfocus()
		{
			base.Unfocus();
			ViewManager.Instance.FollowActor = null;
			if (!this.openingMenu)
			{
				ViewManager.Instance.Offset = VectorMath.ZERO_VECTOR;
			}
			else
			{
				this.pipeline.Each(delegate(Renderable x)
				{
					if (x is IndexedColorGraphic)
					{
						((IndexedColorGraphic)x).AnimationEnabled = false;
					}
					if (x is TileGroup)
					{
						((TileGroup)x).AnimationEnabled = false;
					}
				});
				this.footstepPlayer.Pause();
				this.openingMenu = false;
			}
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			if (this.musicName != null && !this.dontPauseMusic)
			{
				this.musicPosition = AudioManager.Instance.BGM.Position;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00028A40 File Offset: 0x00026C40
		public override void Unload()
		{
			this.player.OnCollision -= this.OnPlayerCollision;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00028A5C File Offset: 0x00026C5C
		private void UpdateSpawners()
		{
			for (int i = 0; i < this.spawners.Count; i++)
			{
				EnemySpawner enemySpawner = this.spawners[i];
				if (!enemySpawner.Bounds.Intersects(ViewManager.Instance.Viewrect))
				{
					List<EnemyNPC> list = enemySpawner.GenerateEnemies(this.pipeline, this.collisionManager);
					if (list != null)
					{
						this.actorManager.AddAll<EnemyNPC>(list);
						this.collisionManager.AddAll<EnemyNPC>(list);
					}
				}
				else
				{
					enemySpawner.SpawnFlag = true;
				}
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00028AE0 File Offset: 0x00026CE0
		public override void Update()
		{
			base.Update();
			if (this.initialized)
			{
				if (this.rainOverlay != null)
				{
					this.rainOverlay.Update();
				}
				this.partyTrain.Update();
				this.UpdateSpawners();
				this.screenDimmer.Update();
				this.executor.Execute();
				this.textbox.Update();
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00028B40 File Offset: 0x00026D40
		public override void Draw()
		{
			if (this.testBack != null)
			{
				this.testBack.AddTranslation(this.player.Position.X - this.player.LastPosition.X, this.player.Position.Y - this.player.LastPosition.Y, 0.001f, 0.002f);
			}
			base.Draw();
			if (this.rainOverlay != null)
			{
				this.rainOverlay.Draw(this.pipeline.Target);
			}
			if (this.iris != null)
			{
				this.iris.Draw(this.pipeline.Target);
			}
			if (Engine.debugDisplay && this.collisionManager != null)
			{
				this.collisionManager.Draw(this.pipeline.Target);
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00028C14 File Offset: 0x00026E14
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					foreach (TileGroup tileGroup in this.mapGroups)
					{
						this.pipeline.Remove(tileGroup);
						tileGroup.Dispose();
					}
					this.actorManager.Clear();
					foreach (TileGroup tileGroup2 in this.mapGroups)
					{
						tileGroup2.Dispose();
					}
					foreach (ParallaxBackground parallaxBackground in this.parallaxes)
					{
						parallaxBackground.Dispose();
					}
					this.screenDimmer.Dispose();
					this.letterboxing.Dispose();
					this.textbox.Dispose();
					this.footstepPlayer.Dispose();
					this.pipeline.Clear();
					if (this.iris != null)
					{
						this.iris.Dispose();
					}
				}
				this.actorManager = null;
				this.mapGroups = null;
				this.parallaxes = null;
				this.screenDimmer = null;
				this.letterboxing = null;
				this.textbox = null;
				this.pipeline = null;
				this.collisionManager = null;
				this.footstepPlayer = null;
				this.player.OnCollision -= this.OnPlayerCollision;
				this.player = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000845 RID: 2117
		private const int COLLISION_RESULTS_SIZE = 8;

		// Token: 0x04000846 RID: 2118
		private const int DIMMER_DEPTH = 2147450870;

		// Token: 0x04000847 RID: 2119
		private Color backColor;

		// Token: 0x04000848 RID: 2120
		private CollisionManager collisionManager;

		// Token: 0x04000849 RID: 2121
		private ICollidable[] collisionResults;

		// Token: 0x0400084A RID: 2122
		private Player player;

		// Token: 0x0400084B RID: 2123
		private PartyTrain partyTrain;

		// Token: 0x0400084C RID: 2124
		private ScreenDimmer screenDimmer;

		// Token: 0x0400084D RID: 2125
		private OverworldTextBox textbox;

		// Token: 0x0400084E RID: 2126
		private ScriptExecutor executor;

		// Token: 0x0400084F RID: 2127
		private CarbineSound battleStartSound;

		// Token: 0x04000850 RID: 2128
		private string musicName;

		// Token: 0x04000851 RID: 2129
		private uint musicPosition;

		// Token: 0x04000852 RID: 2130
		private bool dontPauseMusic;

		// Token: 0x04000853 RID: 2131
		private IList<EnemySpawner> spawners;

		// Token: 0x04000854 RID: 2132
		private IList<EnemyNPC> battleEnemies;

		// Token: 0x04000855 RID: 2133
		private IList<ParallaxBackground> parallaxes;

		// Token: 0x04000856 RID: 2134
		private BattleBackgroundRenderable testBack;

		// Token: 0x04000857 RID: 2135
		private IList<TileGroup> mapGroups;

		// Token: 0x04000858 RID: 2136
		private string mapName;

		// Token: 0x04000859 RID: 2137
		private Vector2f initialPosition;

		// Token: 0x0400085A RID: 2138
		private int initialDirection;

		// Token: 0x0400085B RID: 2139
		private bool initialRunning;

		// Token: 0x0400085C RID: 2140
		private IrisOverlay iris;

		// Token: 0x0400085D RID: 2141
		private LetterboxingOverlay letterboxing;

		// Token: 0x0400085E RID: 2142
		private bool initialized;

		// Token: 0x0400085F RID: 2143
		private bool enableLoadScripts;

		// Token: 0x04000860 RID: 2144
		private bool extendParty;

		// Token: 0x04000861 RID: 2145
		private bool openingMenu;

		// Token: 0x04000862 RID: 2146
		private RainOverlay rainOverlay;

		// Token: 0x04000863 RID: 2147
		private FootstepPlayer footstepPlayer;
	}
}
