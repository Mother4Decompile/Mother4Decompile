using System;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle.Actions
{
	// Token: 0x02000063 RID: 99
	internal class TestingSmiteAction : BattleAction
	{
		// Token: 0x0600023C RID: 572 RVA: 0x0000E064 File Offset: 0x0000C264
		public TestingSmiteAction(ActionParams aparams) : base(aparams)
		{
			this.target = ((this.targets.Length > 0) ? ((PlayerCombatant)this.targets[0]) : null);
			this.damage = this.target.Stats.HP - 1;
			this.message = string.Format("You pressed a button and {0} took {1} HP of damage[p:30]. Are you happy with yourself?", CharacterNames.GetName(this.target.Character), this.damage);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000E0DC File Offset: 0x0000C2DC
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case TestingSmiteAction.State.Initialize:
				this.controller.InterfaceController.OnTextboxComplete += this.OnTextboxComplete;
				this.controller.InterfaceController.ShowTextBox(this.message, false);
				this.state = TestingSmiteAction.State.WaitForUI;
				return;
			case TestingSmiteAction.State.WaitForUI:
				break;
			case TestingSmiteAction.State.Finish:
			{
				this.controller.InterfaceController.OnTextboxComplete -= this.OnTextboxComplete;
				StatSet statChange = new StatSet
				{
					HP = -this.damage
				};
				this.target.AlterStats(statChange);
				this.complete = true;
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000E18B File Offset: 0x0000C38B
		private void OnTextboxComplete()
		{
			this.state = TestingSmiteAction.State.Finish;
		}

		// Token: 0x0400034D RID: 845
		private TestingSmiteAction.State state;

		// Token: 0x0400034E RID: 846
		private string message;

		// Token: 0x0400034F RID: 847
		private PlayerCombatant target;

		// Token: 0x04000350 RID: 848
		private int damage;

		// Token: 0x02000064 RID: 100
		private enum State
		{
			// Token: 0x04000352 RID: 850
			Initialize,
			// Token: 0x04000353 RID: 851
			WaitForUI,
			// Token: 0x04000354 RID: 852
			Finish
		}
	}
}
