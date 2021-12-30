using System;
using Mother4.Battle.Combatants;
using Mother4.Data.Enemies;
using Mother4.GUI.Text;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;

namespace Mother4.Battle.Actions
{
	// Token: 0x020000B4 RID: 180
	internal class EnemyDeathAction : BattleAction
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x00018A35 File Offset: 0x00016C35
		public EnemyDeathAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (this.sender as EnemyCombatant);
			this.state = EnemyDeathAction.State.Initialize;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00018A58 File Offset: 0x00016C58
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case EnemyDeathAction.State.Initialize:
				this.controller.InterfaceController.DoEnemyDeathAnimation(this.combatant.ID);
				this.state = EnemyDeathAction.State.WaitForAnimation;
				return;
			case EnemyDeathAction.State.WaitForUI:
				break;
			case EnemyDeathAction.State.WaitForAnimation:
				this.timer++;
				if (this.timer > 40)
				{
					this.timer = 0;
					this.state = EnemyDeathAction.State.Removal;
					return;
				}
				break;
			case EnemyDeathAction.State.Removal:
			{
				Console.WriteLine("Enemy got dead.");
				this.controller.RemoveCombatant(this.combatant);
				this.controller.DefeatedEnemies.Add(this.combatant.Enemy);
				this.controller.InterfaceController.OnTextboxComplete += this.TextboxComplete;
				string message = string.Empty;
				EnemyData data = EnemyFile.Instance.GetData(this.combatant.Enemy);
				string qualifiedName;
				if (data.TryGetStringQualifiedName("defeat", out qualifiedName))
				{
					RufiniString rufiniString = StringFile.Instance.Get(qualifiedName);
					if (rufiniString.Value != null)
					{
						message = TextProcessor.ProcessReplacements(rufiniString.Value, data.GetContextDictionary());
					}
				}
				this.controller.InterfaceController.ShowTextBox(message, false);
				this.state = EnemyDeathAction.State.WaitForUI;
				return;
			}
			case EnemyDeathAction.State.WaitForMovement:
				this.timer++;
				if (this.timer > 10)
				{
					this.timer = 0;
					this.state = EnemyDeathAction.State.Finish;
					return;
				}
				break;
			case EnemyDeathAction.State.Finish:
				this.controller.InterfaceController.OnTextboxComplete -= this.TextboxComplete;
				this.complete = true;
				break;
			default:
				return;
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00018BE9 File Offset: 0x00016DE9
		public void TextboxComplete()
		{
			this.state = EnemyDeathAction.State.WaitForMovement;
		}

		// Token: 0x0400059C RID: 1436
		private const int ANIMATION_WAIT_FRAMES = 40;

		// Token: 0x0400059D RID: 1437
		private const int MOVEMENT_WAIT_FRAMES = 10;

		// Token: 0x0400059E RID: 1438
		private EnemyDeathAction.State state;

		// Token: 0x0400059F RID: 1439
		private EnemyCombatant combatant;

		// Token: 0x040005A0 RID: 1440
		private int timer;

		// Token: 0x020000B5 RID: 181
		private enum State
		{
			// Token: 0x040005A2 RID: 1442
			Initialize,
			// Token: 0x040005A3 RID: 1443
			WaitForUI,
			// Token: 0x040005A4 RID: 1444
			WaitForAnimation,
			// Token: 0x040005A5 RID: 1445
			Removal,
			// Token: 0x040005A6 RID: 1446
			WaitForMovement,
			// Token: 0x040005A7 RID: 1447
			Finish
		}
	}
}
