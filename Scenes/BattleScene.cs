using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.Input;
using Mother4.Battle;
using Mother4.Battle.Actions;
using Mother4.Battle.Background;
using Mother4.Battle.Combatants;
using Mother4.Battle.Combos;
using Mother4.Data;
using Mother4.Data.Enemies;
using Mother4.GUI.Text;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x02000107 RID: 263
	internal class BattleScene : StandardScene
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x000239E3 File Offset: 0x00021BE3
		public BattleScene(EnemyType[] enemies, bool letterboxing)
		{
			this.enemies = enemies;
			this.letterboxing = letterboxing;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000239F9 File Offset: 0x00021BF9
		public BattleScene(EnemyType[] enemies, bool letterboxing, int bgmOverride, int bbgOverride) : this(enemies, letterboxing)
		{
			this.bgmOverride = bgmOverride;
			this.bbgOverride = bbgOverride;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00023A14 File Offset: 0x00021C14
		private void Initialize()
		{
			CharacterType[] party = PartyManager.Instance.ToArray();
			this.combatantController = new CombatantController(party, this.enemies);
			EnemyCombatant enemyCombatant = null;
			Combatant[] factionCombatants = this.combatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
			foreach (Combatant combatant in factionCombatants)
			{
				if (enemyCombatant == null || enemyCombatant.Stats.Speed < combatant.Stats.Speed)
				{
					enemyCombatant = (combatant as EnemyCombatant);
				}
			}
			Combatant[] factionCombatants2 = this.combatantController.GetFactionCombatants(BattleFaction.PlayerTeam, true);
			PlayerCombatant playerCombatant = factionCombatants2[0] as PlayerCombatant;
			PlayerCombatant playerCombatant2 = factionCombatants2[(factionCombatants2.Length > 1) ? 1 : 0] as PlayerCombatant;
			string music = EnemyMusic.GetMusic(enemyCombatant.Enemy);
			ComboSet combos = ComboLoader.Load(music);
			AudioManager.Instance.SetBGM(music);
			this.comboControl = new ComboController(combos, party);
			this.uiController = new BattleInterfaceController(this.pipeline, this.actorManager, this.combatantController, this.letterboxing);
			this.controller = new BattleController(this.uiController, this.combatantController, this.comboControl);
			this.background = new BattleBackground(EnemyBattleBackgrounds.GetFile(enemyCombatant.Enemy));
			this.GenerateIntroMessage(factionCombatants2.Length, factionCombatants.Length, playerCombatant.Character, playerCombatant2.Character, enemyCombatant.Enemy);
			this.GenerateDebugVerts();
			this.debugBgmPos = (long)((ulong)AudioManager.Instance.BGM.Position);
			this.debugLastBgmPos = this.debugBgmPos;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00023B8C File Offset: 0x00021D8C
		private void GenerateIntroMessage(int partyCount, int enemyCount, CharacterType partyLead, CharacterType partySecondary, EnemyType enemyLead)
		{
			EnemyData data = EnemyFile.Instance.GetData(enemyLead);
			string value = string.Empty;
			string text = string.Empty;
			string text2 = null;
			if (enemyCount == 2)
			{
				text2 = "system.party.enemyTwo";
			}
			else if (enemyCount > 2)
			{
				text2 = "system.party.enemyMany";
			}
			if (text2 != null)
			{
				RufiniString rufiniString = StringFile.Instance.Get(text2);
				if (rufiniString.Value != null)
				{
					Dictionary<string, string> contextDictionary = data.GetContextDictionary();
					value = TextProcessor.ProcessReplacements(rufiniString.Value, contextDictionary);
				}
			}
			string qualifiedName;
			if (data.TryGetStringQualifiedName("encounter", out qualifiedName))
			{
				RufiniString rufiniString2 = StringFile.Instance.Get(qualifiedName);
				if (rufiniString2.Value != null)
				{
					Dictionary<string, string> contextDictionary2 = data.GetContextDictionary();
					contextDictionary2.Add("enemy-party", value);
					text = TextProcessor.ProcessReplacements(rufiniString2.Value, contextDictionary2);
				}
			}
			ActionParams aparams = new ActionParams
			{
				actionType = typeof(MessageAction),
				controller = this.controller,
				sender = null,
				data = new object[]
				{
					text,
					false
				}
			};
			this.controller.AddAction(BattleAction.GetInstance(aparams));
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00023CA8 File Offset: 0x00021EA8
		private void GenerateDebugVerts()
		{
			this.debugRenderStates = new RenderStates(BlendMode.None, Transform.Identity, null, null);
			this.debugRenderStates.Transform.Translate((float)(-(float)this.debugBgmPos) * 0.1f, 0f);
			Color color = new Color(0, 0, 0, 128);
			this.debugRect = new VertexArray(PrimitiveType.Quads, 4U);
			this.debugRect[0U] = new Vertex(new Vector2f(0f, 84f), color);
			this.debugRect[1U] = new Vertex(new Vector2f(320f, 84f), color);
			this.debugRect[2U] = new Vertex(new Vector2f(320f, 96f), color);
			this.debugRect[3U] = new Vertex(new Vector2f(0f, 96f), color);
			this.debugCrosshairVerts = new VertexArray(PrimitiveType.Lines, 8U);
			this.debugCrosshairVerts[0U] = new Vertex(new Vector2f(0f, 90f), Color.White);
			this.debugCrosshairVerts[1U] = new Vertex(new Vector2f(320f, 90f), Color.White);
			this.debugCrosshairVerts[2U] = new Vertex(new Vector2f(160f, 85f), Color.White);
			this.debugCrosshairVerts[3U] = new Vertex(new Vector2f(160f, 95f), Color.White);
			this.debugCrosshairVerts[4U] = new Vertex(new Vector2f(138f, 85f), Color.Red);
			this.debugCrosshairVerts[5U] = new Vertex(new Vector2f(138f, 95f), Color.Red);
			this.debugCrosshairVerts[6U] = new Vertex(new Vector2f(182f, 85f), Color.Green);
			this.debugCrosshairVerts[7U] = new Vertex(new Vector2f(182f, 95f), Color.Green);
			this.debugBeatVerts = new VertexArray(PrimitiveType.Lines);
			foreach (ComboNode comboNode in this.comboControl.ComboSet.ComboNodes)
			{
				int num = (int)(160L + (long)((int)(comboNode.Timestamp * 0.1f)));
				int num2 = 90;
				if (comboNode.Type == ComboType.Point)
				{
					this.debugBeatVerts.Append(new Vertex(new Vector2f((float)num, (float)(num2 - 4)), Color.Cyan));
					this.debugBeatVerts.Append(new Vertex(new Vector2f((float)num, (float)(num2 + 4)), Color.Cyan));
				}
				else if (comboNode.Type == ComboType.BPMRange)
				{
					for (long num3 = 0L; num3 < (long)((ulong)comboNode.Duration); num3 += (long)(60000f / comboNode.BPM.Value))
					{
						int num4 = num + (int)((float)num3 * 0.1f);
						this.debugBeatVerts.Append(new Vertex(new Vector2f((float)num4, (float)(num2 - 4)), Color.Cyan));
						this.debugBeatVerts.Append(new Vertex(new Vector2f((float)num4, (float)(num2 + 4)), Color.Cyan));
					}
				}
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00024018 File Offset: 0x00022218
		public override void Focus()
		{
			ViewManager.Instance.Reset();
			this.Initialize();
			Engine.ClearColor = Color.Black;
			ViewManager.Instance.Reset();
			AudioManager.Instance.BGM.Play();
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			this.initialized = true;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00024078 File Offset: 0x00022278
		private void ButtonPressed(InputManager sender, Button b)
		{
			Combatant combatant = null;
			switch (b)
			{
			case Button.One:
				combatant = this.combatantController[0];
				break;
			case Button.Two:
				combatant = this.combatantController[1];
				break;
			case Button.Three:
				combatant = this.combatantController[2];
				break;
			case Button.Four:
				combatant = this.combatantController[3];
				break;
			}
			if (combatant != null)
			{
				ActionParams aparams = new ActionParams
				{
					actionType = typeof(TestingSmiteAction),
					controller = this.controller,
					targets = new Combatant[]
					{
						combatant
					},
					priority = int.MaxValue
				};
				this.controller.AddAction(BattleAction.GetInstance(aparams));
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0002413C File Offset: 0x0002233C
		public override void Unfocus()
		{
			AudioManager.Instance.BGM.Stop();
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00024163 File Offset: 0x00022363
		public override void Unload()
		{
			base.Dispose();
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0002416B File Offset: 0x0002236B
		public override void Update()
		{
			if (this.initialized)
			{
				this.uiController.Update();
				this.controller.Update();
			}
			base.Update();
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00024194 File Offset: 0x00022394
		public override void Draw()
		{
			if (this.initialized)
			{
				this.background.Draw(this.pipeline.Target);
				this.controller.InterfaceController.Draw(this.pipeline.Target);
			}
			base.Draw();
			if (Engine.debugDisplay && this.initialized)
			{
				this.debugLastBgmPos = this.debugBgmPos;
				this.debugBgmPos = (long)((ulong)AudioManager.Instance.BGM.Position);
				long num = this.debugBgmPos - this.debugLastBgmPos;
				this.debugRenderStates.Transform.Translate((float)(-(float)num) * 0.1f, 0f);
				this.pipeline.Target.Draw(this.debugRect);
				this.pipeline.Target.Draw(this.debugCrosshairVerts);
				this.pipeline.Target.Draw(this.debugBeatVerts, this.debugRenderStates);
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0002428B File Offset: 0x0002248B
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.controller.Dispose();
				this.uiController.Dispose();
				this.comboControl.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x040007E1 RID: 2017
		private const float DEBUG_SCALE = 0.1f;

		// Token: 0x040007E2 RID: 2018
		private CombatantController combatantController;

		// Token: 0x040007E3 RID: 2019
		private BattleInterfaceController uiController;

		// Token: 0x040007E4 RID: 2020
		private BattleController controller;

		// Token: 0x040007E5 RID: 2021
		private ComboController comboControl;

		// Token: 0x040007E6 RID: 2022
		private BattleBackground background;

		// Token: 0x040007E7 RID: 2023
		private VertexArray debugRect;

		// Token: 0x040007E8 RID: 2024
		private VertexArray debugCrosshairVerts;

		// Token: 0x040007E9 RID: 2025
		private VertexArray debugBeatVerts;

		// Token: 0x040007EA RID: 2026
		private RenderStates debugRenderStates;

		// Token: 0x040007EB RID: 2027
		private long debugBgmPos;

		// Token: 0x040007EC RID: 2028
		private long debugLastBgmPos;

		// Token: 0x040007ED RID: 2029
		private bool initialized;

		// Token: 0x040007EE RID: 2030
		private EnemyType[] enemies;

		// Token: 0x040007EF RID: 2031
		private bool letterboxing;

		// Token: 0x040007F0 RID: 2032
		private int bgmOverride;

		// Token: 0x040007F1 RID: 2033
		private int bbgOverride;
	}
}
