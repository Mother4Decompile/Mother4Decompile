using System;
using Mother4.Battle.Combatants;

namespace Mother4.Battle.Actions
{
	// Token: 0x0200005A RID: 90
	internal class GroovyAction : BattleAction
	{
		// Token: 0x0600021E RID: 542 RVA: 0x0000D3DF File Offset: 0x0000B5DF
		public GroovyAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (aparams.targets[0] as PlayerCombatant);
			this.state = GroovyAction.State.Start;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000D404 File Offset: 0x0000B604
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case GroovyAction.State.Start:
				if (this.CanGroovy())
				{
					this.controller.InterfaceController.SetCardGroovy(this.combatant.ID, true);
					this.state = GroovyAction.State.WaitForPop;
					return;
				}
				this.state = GroovyAction.State.Cancel;
				this.complete = true;
				return;
			case GroovyAction.State.WaitForPop:
				this.TimerWait(10, GroovyAction.State.Groovy);
				return;
			case GroovyAction.State.Groovy:
				this.controller.InterfaceController.DoGroovy(this.combatant.ID);
				this.state = GroovyAction.State.WaitForGroovy;
				return;
			case GroovyAction.State.WaitForGroovy:
				this.TimerWait(70, GroovyAction.State.Finish);
				return;
			case GroovyAction.State.Finish:
			{
				this.combatant.AlterStats(new StatSet
				{
					Meter = -this.combatant.Stats.Meter
				});
				ActionParams aparams = new ActionParams
				{
					actionType = typeof(PlayerDecisionAction),
					controller = this.controller,
					sender = this.combatant,
					priority = 2147483646,
					data = new object[]
					{
						true
					}
				};
				this.controller.AddDecisionAction(new PlayerDecisionAction(aparams));
				this.complete = true;
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000D54B File Offset: 0x0000B74B
		private void TimerWait(int delay, GroovyAction.State nextState)
		{
			this.timer++;
			if (this.timer > delay)
			{
				this.timer = 0;
				this.state = nextState;
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000D574 File Offset: 0x0000B774
		private bool CanGroovy()
		{
			Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
			return factionCombatants.Length > 0;
		}

		// Token: 0x0400031B RID: 795
		private const int POP_HEIGHT = 28;

		// Token: 0x0400031C RID: 796
		private const int POP_DELAY = 10;

		// Token: 0x0400031D RID: 797
		private const int GROOVY_DELAY = 70;

		// Token: 0x0400031E RID: 798
		private GroovyAction.State state;

		// Token: 0x0400031F RID: 799
		private PlayerCombatant combatant;

		// Token: 0x04000320 RID: 800
		private int timer;

		// Token: 0x0200005B RID: 91
		private enum State
		{
			// Token: 0x04000322 RID: 802
			Start,
			// Token: 0x04000323 RID: 803
			WaitForPop,
			// Token: 0x04000324 RID: 804
			Groovy,
			// Token: 0x04000325 RID: 805
			WaitForGroovy,
			// Token: 0x04000326 RID: 806
			Finish,
			// Token: 0x04000327 RID: 807
			Cancel
		}
	}
}
