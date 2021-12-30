using System;
using System.Collections.Generic;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;
using Mother4.Battle.Combos;
using Mother4.Data;

namespace Mother4.Battle
{
	// Token: 0x020000C3 RID: 195
	internal sealed class BattleController : IDisposable
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0001A893 File Offset: 0x00018A93
		public BattleInterfaceController InterfaceController
		{
			get
			{
				return this.uiControl;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0001A89B File Offset: 0x00018A9B
		public CombatantController CombatantController
		{
			get
			{
				return this.combatantControl;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x0001A8A3 File Offset: 0x00018AA3
		public ComboController ComboController
		{
			get
			{
				return this.comboControl;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0001A8AB File Offset: 0x00018AAB
		public int ActionCount
		{
			get
			{
				return this.actions.Count;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0001A8B8 File Offset: 0x00018AB8
		public int DecisionCount
		{
			get
			{
				return this.decisions.Count;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0001A8C5 File Offset: 0x00018AC5
		public Dictionary<string, object> Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0001A8CD File Offset: 0x00018ACD
		public BattleStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0001A8D5 File Offset: 0x00018AD5
		public List<EnemyType> DefeatedEnemies
		{
			get
			{
				return this.defeatedEnemies;
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001A8E0 File Offset: 0x00018AE0
		public BattleController(BattleInterfaceController uicontrol, CombatantController combatantControl, ComboController comboControl)
		{
			this.uiControl = uicontrol;
			this.combatantControl = combatantControl;
			this.comboControl = comboControl;
			this.status = BattleStatus.Ongoing;
			this.actions = new List<BattleAction>();
			this.decisions = new List<DecisionAction>();
			this.defeatedEnemies = new List<EnemyType>();
			this.data = new Dictionary<string, object>();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001A93C File Offset: 0x00018B3C
		~BattleController()
		{
			this.Dispose(false);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001A98C File Offset: 0x00018B8C
		public void Update()
		{
			this.RemoveDeadEnemies();
			this.actions.Sort();
			if (this.WinConditionsMet() && !this.winHandled)
			{
				this.status = BattleStatus.Won;
				Console.WriteLine("Win conditions met; queue cleared.");
				this.actions.Clear();
				ActionParams aparams = new ActionParams
				{
					actionType = typeof(BattleWinAction),
					controller = this,
					sender = null,
					priority = 2147483646
				};
				this.actions.Add(BattleAction.GetInstance(aparams));
				this.winHandled = true;
			}
			else if (this.uiControl.RunAttempted)
			{
				this.uiControl.RunAttempted = false;
				if (BattleCalculator.RunSuccess(this.combatantControl, this.turnNumber))
				{
					this.status = BattleStatus.Ran;
					Console.WriteLine("Ran away succesfully; queue cleared.");
					this.actions.Clear();
					this.decisions.Clear();
					ActionParams aparams2 = new ActionParams
					{
						actionType = typeof(MessageAction),
						controller = this,
						sender = null,
						priority = int.MaxValue,
						data = new object[]
						{
							"You tried to run away...[p:60] and did!",
							false
						}
					};
					this.actions.Add(BattleAction.GetInstance(aparams2));
					ActionParams aparams3 = new ActionParams
					{
						actionType = typeof(BattleEscapeAction),
						controller = this,
						sender = null,
						priority = 2147483646
					};
					this.actions.Add(BattleAction.GetInstance(aparams3));
				}
				else
				{
					Console.WriteLine("Run away failed; removing player team actions from queue");
					this.actions.RemoveAll((BattleAction action) => action.Sender.Faction == BattleFaction.PlayerTeam);
					this.decisions.RemoveAll((DecisionAction decision) => decision.Sender.Faction == BattleFaction.PlayerTeam);
					this.decisionCounter = 0;
					ActionParams aparams4 = new ActionParams
					{
						actionType = typeof(MessageAction),
						controller = this,
						sender = null,
						priority = int.MaxValue,
						data = new object[]
						{
							"You tried to run away...[p:60] but couldn't!",
							false
						}
					};
					this.actions.Add(BattleAction.GetInstance(aparams4));
				}
			}
			if (this.focus != null && !this.focus.Complete)
			{
				this.focus.Update();
				return;
			}
			this.HandleGroovy();
			if (this.decisionCounter < this.decisions.Count)
			{
				this.focus = this.decisions[this.decisionCounter];
				this.decisionCounter++;
				return;
			}
			if (this.actions.Count > 0)
			{
				this.uiControl.HideButtonBar();
				this.focus = this.actions[0];
				this.actions.RemoveAt(0);
				return;
			}
			Console.WriteLine("All actions performed; building new queue from DecisionActions.");
			this.BuildNewDecisionQueue();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001ACB8 File Offset: 0x00018EB8
		private void RemoveDeadEnemies()
		{
			Combatant[] factionCombatants = this.combatantControl.GetFactionCombatants(BattleFaction.EnemyTeam);
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.Stats.HP <= 0)
				{
					ActionParams aparams = new ActionParams
					{
						actionType = typeof(EnemyDeathAction),
						controller = this,
						sender = (combatant as EnemyCombatant),
						priority = int.MaxValue
					};
					this.actions.Add(BattleAction.GetInstance(aparams));
				}
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001AD6C File Offset: 0x00018F6C
		private void HandleGroovy()
		{
			Combatant[] factionCombatants = this.combatantControl.GetFactionCombatants(BattleFaction.PlayerTeam);
			Combatant[] factionCombatants2 = this.combatantControl.GetFactionCombatants(BattleFaction.EnemyTeam);
			if (factionCombatants2.Length > 0)
			{
				Combatant[] array = factionCombatants;
				for (int i = 0; i < array.Length; i++)
				{
					Combatant combatant = array[i];
					BattleAction battleAction = this.actions.Find((BattleAction x) => x is GroovyAction && x.Sender == combatant);
					if (combatant.Stats.HP > 0 && combatant.Stats.Meter >= 1f && battleAction == null)
					{
						ActionParams aparams = new ActionParams
						{
							actionType = typeof(GroovyAction),
							controller = this,
							sender = combatant,
							targets = new Combatant[]
							{
								combatant
							},
							priority = 2147483646
						};
						this.actions.Add(BattleAction.GetInstance(aparams));
						this.actions.Sort();
					}
				}
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001AE97 File Offset: 0x00019097
		public void RemoveCombatant(Combatant combatant)
		{
			this.RemoveCombatantActions(combatant);
			this.uiControl.RemoveEnemy(combatant.ID);
			this.combatantControl.Remove(combatant);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001AED8 File Offset: 0x000190D8
		private void RemoveCombatantActions(Combatant combatant)
		{
			this.actions.RemoveAll((BattleAction action) => action.Sender == combatant);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001AF0C File Offset: 0x0001910C
		private bool WinConditionsMet()
		{
			int num = 0;
			foreach (Combatant combatant in this.combatantControl.CombatantList)
			{
				if (combatant.Faction == BattleFaction.EnemyTeam)
				{
					num++;
				}
			}
			return num == 0;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001AF70 File Offset: 0x00019170
		public void SetFinalStatSets()
		{
			Combatant[] factionCombatants = this.combatantControl.GetFactionCombatants(BattleFaction.PlayerTeam);
			foreach (Combatant combatant in factionCombatants)
			{
				PlayerCombatant playerCombatant = combatant as PlayerCombatant;
				StatSet stats = CharacterStats.GetStats(playerCombatant.Character);
				stats.HP = combatant.Stats.HP;
				stats.PP = combatant.Stats.PP;
				CharacterStats.SetStats(playerCombatant.Character, stats);
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001AFEC File Offset: 0x000191EC
		private void BuildNewDecisionQueue()
		{
			this.turnNumber++;
			this.decisions.Clear();
			this.decisionCounter = 0;
			for (int i = 0; i < this.combatantControl.CombatantList.Count; i++)
			{
				Combatant combatant = this.combatantControl.CombatantList[i];
				this.decisions.Add(combatant.GetDecisionAction(this, this.combatantControl.CombatantList.Count - i));
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001B06A File Offset: 0x0001926A
		public void AddAction(BattleAction action)
		{
			this.actions.Add(action);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001B078 File Offset: 0x00019278
		public void AddActions(BattleAction[] actions)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				this.AddAction(actions[i]);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001B09C File Offset: 0x0001929C
		public void AddDecisionAction(DecisionAction action)
		{
			this.decisions.Add(action);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001B0AC File Offset: 0x000192AC
		public void AddDecisionActions(DecisionAction[] actions)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				this.AddDecisionAction(actions[i]);
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001B0D0 File Offset: 0x000192D0
		public bool RevertDecision()
		{
			if (this.decisionCounter > 1)
			{
				DecisionAction decisionAction = this.decisions[this.decisionCounter - 1];
				DecisionAction decisionAction2 = this.decisions[this.decisionCounter - 2];
				if (decisionAction2.Sender.Faction == BattleFaction.PlayerTeam)
				{
					PlayerCombatant playerCombatant = (PlayerCombatant)decisionAction2.Sender;
					PlayerCombatant playerCombatant2 = (PlayerCombatant)decisionAction.Sender;
					foreach (BattleAction battleAction in this.actions.ToArray())
					{
						if (battleAction.Sender == playerCombatant)
						{
							this.actions.Remove(battleAction);
						}
					}
					this.decisionCounter -= 2;
					this.decisions[this.decisionCounter] = playerCombatant.GetDecisionAction(this, this.decisionCounter, true);
					this.decisions[this.decisionCounter + 1] = playerCombatant2.GetDecisionAction(this, this.decisionCounter + 1);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001B1CA File Offset: 0x000193CA
		public bool CanRevert()
		{
			return this.decisionCounter > 1;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001B1D5 File Offset: 0x000193D5
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001B1E4 File Offset: 0x000193E4
		private void Dispose(bool disposing)
		{
			if (!this.disposed && !disposing)
			{
				this.uiControl.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x04000608 RID: 1544
		private bool disposed;

		// Token: 0x04000609 RID: 1545
		private BattleStatus status;

		// Token: 0x0400060A RID: 1546
		private List<BattleAction> actions;

		// Token: 0x0400060B RID: 1547
		private List<DecisionAction> decisions;

		// Token: 0x0400060C RID: 1548
		private BattleAction focus;

		// Token: 0x0400060D RID: 1549
		private int decisionCounter;

		// Token: 0x0400060E RID: 1550
		private bool winHandled;

		// Token: 0x0400060F RID: 1551
		private List<EnemyType> defeatedEnemies;

		// Token: 0x04000610 RID: 1552
		private int turnNumber;

		// Token: 0x04000611 RID: 1553
		private BattleInterfaceController uiControl;

		// Token: 0x04000612 RID: 1554
		private CombatantController combatantControl;

		// Token: 0x04000613 RID: 1555
		private ComboController comboControl;

		// Token: 0x04000614 RID: 1556
		private Dictionary<string, object> data;
	}
}
