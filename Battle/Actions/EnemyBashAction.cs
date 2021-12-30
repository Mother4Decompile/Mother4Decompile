using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Battle.Combatants;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.Actions
{
	// Token: 0x020000B2 RID: 178
	internal class EnemyBashAction : BattleAction
	{
		// Token: 0x060003E0 RID: 992 RVA: 0x0001864C File Offset: 0x0001684C
		public EnemyBashAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (this.sender as EnemyCombatant);
			this.power = (float)aparams.data[0];
			this.bashMessage = ((aparams.data.Length > 1) ? ((string)aparams.data[1]) : "{0}{1} bashed {2}!");
			this.messages = new Stack<string>();
			this.statSets = new Stack<Tuple<Combatant, StatSet>>();
			this.state = EnemyBashAction.State.Initialize;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000186CC File Offset: 0x000168CC
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case EnemyBashAction.State.Initialize:
			{
				Console.WriteLine("BASHMÖDE ({0})", this.combatant.Enemy);
				this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
				this.controller.InterfaceController.FlashEnemy(this.sender as EnemyCombatant, Color.White, ColorBlendMode.Screen, 8, 2);
				this.controller.InterfaceController.PreEnemyAttack.Play();
				int i = 0;
				while (i < this.targets.Length)
				{
					Combatant combatant = this.targets[i];
					if (this.controller.CombatantController.IsIdValid(combatant.ID))
					{
						goto IL_FC;
					}
					Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
					Combatant combatant2 = factionCombatants[Engine.Random.Next() % factionCombatants.Length];
					if (Array.IndexOf<Combatant>(this.targets, combatant2) < 0)
					{
						this.targets[i] = combatant2;
						combatant = combatant2;
						goto IL_FC;
					}
					IL_1FB:
					i++;
					continue;
					IL_FC:
					StatSet item = default(StatSet);
					bool flag;
					item.HP = -BattleCalculator.CalculatePhysicalDamage(this.power, this.combatant, combatant, out flag);
					this.statSets.Push(new Tuple<Combatant, StatSet>(combatant, item));
					Console.WriteLine(" Target's HP changed by {0}.", item.HP);
					string arg = "";
					switch (combatant.Faction)
					{
					case BattleFaction.PlayerTeam:
					{
						PlayerCombatant playerCombatant = (PlayerCombatant)combatant;
						arg = CharacterNames.GetName(playerCombatant.Character);
						break;
					}
					case BattleFaction.EnemyTeam:
					{
						EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
						arg = string.Format("{0}{1}", EnemyNames.GetArticle(enemyCombatant.Enemy), EnemyNames.GetName(enemyCombatant.Enemy));
						break;
					}
					case BattleFaction.NeutralTeam:
						arg = "UNIMPLEMENTED";
						break;
					}
					string item2 = string.Format(this.bashMessage, Capitalizer.Capitalize(EnemyNames.GetArticle(this.combatant.Enemy)), EnemyNames.GetName(this.combatant.Enemy), arg);
					this.messages.Push(item2);
					goto IL_1FB;
				}
				this.state = EnemyBashAction.State.PopMessage;
				return;
			}
			case EnemyBashAction.State.PopMessage:
			{
				string message = this.messages.Pop();
				this.controller.InterfaceController.ShowTextBox(message, false);
				this.state = EnemyBashAction.State.WaitForUI;
				return;
			}
			case EnemyBashAction.State.WaitForUI:
				break;
			case EnemyBashAction.State.Finish:
				this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
				this.complete = true;
				break;
			default:
				return;
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001893C File Offset: 0x00016B3C
		private void DoDamage()
		{
			Tuple<Combatant, StatSet> tuple = this.statSets.Pop();
			tuple.Item1.AlterStats(tuple.Item2);
			this.controller.InterfaceController.AddDamageNumber(tuple.Item1, tuple.Item2.HP);
			if (tuple.Item1 is PlayerCombatant)
			{
				this.controller.InterfaceController.SetCardSpring(tuple.Item1.ID, BattleCard.SpringMode.Normal, new Vector2f(0f, 8f), new Vector2f(0f, 0.5f), new Vector2f(0f, 0.95f));
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000189DE File Offset: 0x00016BDE
		private void TimerEnd(int timerIndex)
		{
			if (this.timerIndex == timerIndex)
			{
				if (this.messages.Count == 0)
				{
					this.state = EnemyBashAction.State.Finish;
					return;
				}
				this.state = EnemyBashAction.State.PopMessage;
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00018A05 File Offset: 0x00016C05
		private void InteractionComplete()
		{
			this.DoDamage();
			this.timerIndex = TimerManager.Instance.StartTimer(30);
			TimerManager.Instance.OnTimerEnd += this.TimerEnd;
		}

		// Token: 0x0400058B RID: 1419
		private const int POWER_INDEX = 0;

		// Token: 0x0400058C RID: 1420
		private const int MESSAGE_INDEX = 1;

		// Token: 0x0400058D RID: 1421
		private const int BLINK_DURATION = 8;

		// Token: 0x0400058E RID: 1422
		private const int BLINK_COUNT = 2;

		// Token: 0x0400058F RID: 1423
		private const string DEFAULT_BASH_MESSAGE = "{0}{1} bashed {2}!";

		// Token: 0x04000590 RID: 1424
		private EnemyBashAction.State state;

		// Token: 0x04000591 RID: 1425
		private float power;

		// Token: 0x04000592 RID: 1426
		private EnemyCombatant combatant;

		// Token: 0x04000593 RID: 1427
		private Stack<string> messages;

		// Token: 0x04000594 RID: 1428
		private Stack<Tuple<Combatant, StatSet>> statSets;

		// Token: 0x04000595 RID: 1429
		private int timerIndex;

		// Token: 0x04000596 RID: 1430
		private string bashMessage;

		// Token: 0x020000B3 RID: 179
		private enum State
		{
			// Token: 0x04000598 RID: 1432
			Initialize,
			// Token: 0x04000599 RID: 1433
			PopMessage,
			// Token: 0x0400059A RID: 1434
			WaitForUI,
			// Token: 0x0400059B RID: 1435
			Finish
		}
	}
}
