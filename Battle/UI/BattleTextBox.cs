using System;
using Carbine.Input;
using Carbine.Utility;
using Mother4.GUI;
using Mother4.GUI.Text;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x020000E1 RID: 225
	internal class BattleTextBox : TextBox
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x000202D4 File Offset: 0x0001E4D4
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x000202DC File Offset: 0x0001E4DC
		public bool AutoScroll
		{
			get
			{
				return this.autoScroll;
			}
			set
			{
				this.autoScroll = value;
				this.hideAdvanceArrow = this.autoScroll;
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000202F1 File Offset: 0x0001E4F1
		public BattleTextBox() : base(BattleTextBox.BOX_POSITION, BattleTextBox.BOX_SIZE, false)
		{
			this.autoScroll = true;
			this.typewriter.SetTextSound(Typewriter.BlipSound.None, false);
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0002032E File Offset: 0x0001E52E
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (!this.autoScroll && this.isWaitingOnPlayer && b == Button.A)
			{
				base.ContinueFromWait();
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00020349 File Offset: 0x0001E549
		private void OnWaitTimerEnd(int timerIndex)
		{
			if (timerIndex == this.waitTimerIndex)
			{
				this.isWaitingOnPlayer = false;
				this.advanceArrow.Visible = false;
				TimerManager.Instance.OnTimerEnd -= this.OnWaitTimerEnd;
				base.Dequeue();
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00020384 File Offset: 0x0001E584
		protected override void HandlePrompt()
		{
			this.isWaitingOnPlayer = true;
			this.advanceArrow.Visible = !this.hideAdvanceArrow;
			if (this.autoScroll)
			{
				this.waitTimerIndex = TimerManager.Instance.StartTimer(45);
				TimerManager.Instance.OnTimerEnd += this.OnWaitTimerEnd;
				this.advanceArrow.Visible = false;
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000203E8 File Offset: 0x0001E5E8
		public override void Hide()
		{
			base.Hide();
			this.Clear();
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000203F6 File Offset: 0x0001E5F6
		public override void Reset()
		{
			base.Reset();
			this.autoScroll = true;
			this.typewriter.SetTextSound(Typewriter.BlipSound.None, false);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00020412 File Offset: 0x0001E612
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000702 RID: 1794
		private const Button ADVANCE_BUTTON = Button.A;

		// Token: 0x04000703 RID: 1795
		protected const int MESSAGE_WAIT = 45;

		// Token: 0x04000704 RID: 1796
		private static readonly Vector2f BOX_SIZE = new Vector2f(248f, 43f);

		// Token: 0x04000705 RID: 1797
		private static readonly Vector2f BOX_POSITION = new Vector2f((float)(160L - (long)((int)(BattleTextBox.BOX_SIZE.X / 2f))), 0f);

		// Token: 0x04000706 RID: 1798
		private int waitTimerIndex;

		// Token: 0x04000707 RID: 1799
		private bool autoScroll;
	}
}
