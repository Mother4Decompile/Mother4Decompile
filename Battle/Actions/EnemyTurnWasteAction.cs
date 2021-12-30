using System;
using Mother4.Battle.Combatants;
using Rufini.Strings;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	// Token: 0x02000056 RID: 86
	internal class EnemyTurnWasteAction : BattleAction
	{
		// Token: 0x06000218 RID: 536 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
		public EnemyTurnWasteAction(ActionParams aparams) : base(aparams)
		{
			string qualifiedName = (string)aparams.data[0];
			this.message = (StringFile.Instance.Get(qualifiedName).Value ?? string.Empty);
			this.useButton = (aparams.data.Length > 1 && (this.useButton = ((int)aparams.data[1] > 0)));
			this.state = EnemyTurnWasteAction.State.Initialize;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000D030 File Offset: 0x0000B230
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case EnemyTurnWasteAction.State.Initialize:
				this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
				this.controller.InterfaceController.ShowTextBox(this.message, this.useButton);
				this.controller.InterfaceController.FlashEnemy(this.sender as EnemyCombatant, Color.Black, 8, 2);
				this.controller.InterfaceController.PreEnemyAttack.Play();
				this.state = EnemyTurnWasteAction.State.WaitForUI;
				return;
			case EnemyTurnWasteAction.State.WaitForUI:
				break;
			case EnemyTurnWasteAction.State.Finish:
				this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
				this.complete = true;
				break;
			default:
				return;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000D0F7 File Offset: 0x0000B2F7
		public void InteractionComplete()
		{
			this.state = EnemyTurnWasteAction.State.Finish;
		}

		// Token: 0x04000305 RID: 773
		private const int MESSAGE_INDEX = 0;

		// Token: 0x04000306 RID: 774
		private const int USE_BUTTON_INDEX = 1;

		// Token: 0x04000307 RID: 775
		private const int BLINK_DURATION = 8;

		// Token: 0x04000308 RID: 776
		private const int BLINK_COUNT = 2;

		// Token: 0x04000309 RID: 777
		private EnemyTurnWasteAction.State state;

		// Token: 0x0400030A RID: 778
		private string message;

		// Token: 0x0400030B RID: 779
		private bool useButton;

		// Token: 0x02000057 RID: 87
		private enum State
		{
			// Token: 0x0400030D RID: 781
			Initialize,
			// Token: 0x0400030E RID: 782
			WaitForUI,
			// Token: 0x0400030F RID: 783
			Finish
		}
	}
}
