using System;

namespace Mother4.Battle.Actions
{
	// Token: 0x020000B7 RID: 183
	internal class MessageAction : BattleAction
	{
		// Token: 0x060003EA RID: 1002 RVA: 0x00018DA8 File Offset: 0x00016FA8
		public MessageAction(ActionParams aparams) : base(aparams)
		{
			this.message = (string)aparams.data[0];
			this.useButton = (bool)aparams.data[1];
			this.state = MessageAction.State.Initialize;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00018DE0 File Offset: 0x00016FE0
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case MessageAction.State.Initialize:
				this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
				this.controller.InterfaceController.ShowTextBox(this.message, this.useButton);
				this.state = MessageAction.State.WaitForUI;
				return;
			case MessageAction.State.WaitForUI:
				break;
			case MessageAction.State.Finish:
				this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
				this.complete = true;
				break;
			default:
				return;
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00018E70 File Offset: 0x00017070
		public void InteractionComplete()
		{
			this.state = MessageAction.State.Finish;
		}

		// Token: 0x040005AA RID: 1450
		private const int MESSAGE_INDEX = 0;

		// Token: 0x040005AB RID: 1451
		private const int USE_BUTTON_INDEX = 1;

		// Token: 0x040005AC RID: 1452
		private MessageAction.State state;

		// Token: 0x040005AD RID: 1453
		private string message;

		// Token: 0x040005AE RID: 1454
		private bool useButton;

		// Token: 0x020000B8 RID: 184
		private enum State
		{
			// Token: 0x040005B0 RID: 1456
			Initialize,
			// Token: 0x040005B1 RID: 1457
			WaitForUI,
			// Token: 0x040005B2 RID: 1458
			Finish
		}
	}
}
